using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sellers.DAL.Context;
using Sellers.DAL.Interfaces.Provider;
using Sellers.Domain.Entities;

namespace Sellers.DAL.Repositories.Provider
{
    public class SellerProviderRepository : ISellerProviderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SellerProviderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SellerDetails> GetSellerByIdAsync(long sellerId)
        {
            return await _dbContext.Sellers.FirstOrDefaultAsync(x => x.Seller_Id == sellerId);
        }
    }
}