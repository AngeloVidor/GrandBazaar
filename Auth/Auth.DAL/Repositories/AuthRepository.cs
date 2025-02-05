using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.DAL.Context;
using Auth.DAL.Interfaces;
using Auth.Domain.Entities;

namespace Auth.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> RegisterNewUserAsync(User registration)
        {
            await _dbContext.Users.AddAsync(registration);
            await _dbContext.SaveChangesAsync();
            return registration;
        }
    }
}