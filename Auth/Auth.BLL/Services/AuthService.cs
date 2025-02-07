using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.BLL.DTOs;
using Auth.BLL.Interfaces;
using Auth.DAL.Interfaces;
using Auth.Domain.Entities;
using AutoMapper;

namespace Auth.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserManagementRepository _userManagement;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, IMapper mapper, IUserManagementRepository userManagement)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _userManagement = userManagement;
        }

        public async Task<RegistrationDto> AddNewUserAsync(RegistrationDto registration)
        {
            //ToDo: Need to validate all the entry properties
            var email = await _userManagement.GetUserByEmailAsync(registration.Email);
            if (email != null)
            {
                throw new InvalidOperationException($"Email {email} is already in use");
            }
            registration.Password = BCrypt.Net.BCrypt.HashPassword(registration.Password);

            var entity = _mapper.Map<User>(registration);
            var response = await _authRepository.RegisterNewUserAsync(entity);
            return _mapper.Map<RegistrationDto>(response);
        }

        public async Task<LoginDto> SignInAsync(string email, string password)
        {
            var userEntity = await _userManagement.GetUserByEmailAsync(email);
            if (userEntity == null || !BCrypt.Net.BCrypt.Verify(password, userEntity.Password))
            {
                return null;
            }
            return _mapper.Map<LoginDto>(userEntity);
        }
    }
}