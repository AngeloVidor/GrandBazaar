using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.DAL.Context;
using Sellers.DAL.Interfaces;
using Sellers.Domain.Entities;

namespace Sellers.DAL.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SellerDetails> AddSellerProfileAsync(SellerDetails sellerDetails)
        {
            await _dbContext.AddAsync(sellerDetails);
            await _dbContext.SaveChangesAsync();
            return sellerDetails;
        }
    }
}