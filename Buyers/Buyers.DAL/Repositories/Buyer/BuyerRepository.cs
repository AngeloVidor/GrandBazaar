using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.DAL.Context;
using Buyers.DAL.Interfaces;
using Buyers.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Buyers.DAL.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BuyerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Buyer> AddNewBuyerAsync(Buyer buyer)
        {
            await _dbContext.Buyers.AddAsync(buyer);
            await _dbContext.SaveChangesAsync();
            return buyer;
        }

        public async Task<Buyer> GetMyProfileAsync(long userId)
        {
            return await _dbContext.Buyers.FirstOrDefaultAsync(x => x.User_Id == userId);
        }
    }
}