using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sellers.DAL.Context;
using Sellers.DAL.Interfaces;

namespace Sellers.DAL.Repositories
{
    public class ProfileManagementRepository : IProfileManagementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileManagementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UserHasSellerProfileAsync(long userId)
        {
            var alreadyExists = await _dbContext.Sellers.FirstOrDefaultAsync(x => x.User_Id == userId);
            if (alreadyExists != null)
            {
                throw new InvalidOperationException("User already has a seller profile.");
            }
            return false;
        }

        public async Task<long> GetSellerProfileIdByUserIdAsync(long userId)
        {
            var user = await _dbContext.Sellers.FirstOrDefaultAsync(x => x.User_Id == userId);
            return user.Seller_Id;
        }
    }
}