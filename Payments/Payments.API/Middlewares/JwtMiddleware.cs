using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Payments.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var token = GetTokenFromHeader(context);
            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }

            var userId = ValidateToken(token);
            if (userId == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
                return;
            }

            context.Items["userId"] = userId;
            await _next(context);
        }

        private string GetTokenFromHeader(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                return null;
            }

            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return null;
            }

            return authorizationHeader.Split(" ").Last();
        }

        private string ValidateToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var secretKey = _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    return null;
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

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

                var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    return null;
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    userId = principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                }

                return userId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}