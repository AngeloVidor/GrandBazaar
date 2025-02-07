using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Middlewares
{
    public class JwtAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            List<string> PublicRoutes = new List<string>
            {
                "/api/Auth/Register",
                "/api/Auth/SignIn",
            };



            if (PublicRoutes.Contains(context.Request.Path.Value, StringComparer.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var tokenAccess = context
                .Request.Headers["Authorization"]
                .FirstOrDefault()
                ?.Split(" ")[1];
            if (string.IsNullOrEmpty(tokenAccess))
            {
                throw new Exception("Token to provided");
            }

            var isValidToken = ValidateToken(tokenAccess);
            if (!isValidToken)
            {
                throw new Exception("Invalid token");
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(tokenAccess) as JwtSecurityToken;
            var userIdClaims = jwtToken?.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier
            );

            if (userIdClaims == null)
            {
                throw new Exception("User ID claim not found");
            }

            context.Items["userId"] = userIdClaims.Value;
            await _next(context);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var secretKey = _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new Exception("JWT Secret Key is not configured");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = handler.ValidateToken(
                    token,
                    tokenValidationParameters,
                    out var validatedToken
                );

                if (!(validatedToken is JwtSecurityToken jwtSecurityToken))
                {
                    throw new Exception("Invalid token");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token validation failed: {ex.Message}");
            }
        }
    }
}