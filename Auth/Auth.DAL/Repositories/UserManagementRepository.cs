using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.DAL.Context;
using Auth.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Auth.Domain.Entities;

namespace Auth.DAL.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserManagementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

    }
}