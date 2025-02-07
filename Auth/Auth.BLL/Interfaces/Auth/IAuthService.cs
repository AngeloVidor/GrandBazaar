using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.BLL.DTOs;

namespace Auth.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<RegistrationDto> AddNewUserAsync(RegistrationDto registration);
        Task<LoginDto> SignInAsync(string email, string password);
    }
}