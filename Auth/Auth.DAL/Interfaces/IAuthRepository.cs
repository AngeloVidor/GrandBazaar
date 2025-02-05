using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Domain.Entities;

namespace Auth.DAL.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> RegisterNewUserAsync(User registration);
    }
}