using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.DAL.Context;
using Buyers.DAL.Interfaces.Management;
using Buyers.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Buyers.DAL.Repositories.Management
{
    public class BuyerManagementRepository : IBuyerManagementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BuyerManagementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> GetBuyerIdByUserIdAsync(long userId)
        {
            var user = await _dbContext.Buyers.FirstOrDefaultAsync(x => x.User_Id == userId);
            return user.Buyer_Id;
        }
    }
}