using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.BLL.DTOs;
using Auth.BLL.Interfaces.Tokens;
using Auth.DAL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.BLL.Services.Tokens
{
    public class BearerTokenManagement : IBearerTokenManagement
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserManagementRepository _userManagement;

        public BearerTokenManagement(IConfiguration configuration, IMapper mapper, IUserManagementRepository userManagement)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManagement = userManagement;
        }

        public async Task<string> GenerateTokenAsync(LoginDto user)
        {

            var userIdentity = await _userManagement.GetUserByEmailAsync(user.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.User_ID.ToString()),
                new Claim(ClaimTypes.Name, userIdentity.FirstName),
                new Claim(ClaimTypes.Role, userIdentity.AccountType.ToString())
            };

            var secretKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:Audience"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(
                                _configuration.GetValue<int>("Jwt:DurationInMinutes")
                            ),
                            signingCredentials: creds
                        );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}