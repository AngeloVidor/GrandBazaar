using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<SellerDetails> GetMyProfileAsync(long userId)
        {
            return await _dbContext.Sellers.FirstOrDefaultAsync(x => x.User_Id == userId);
        }

        public async Task<SellerDetails> UpdateSellerProfileAsync(SellerDetails sellerDetails)
        {
            var profile = await _dbContext.Sellers.FirstOrDefaultAsync(x => x.Seller_Id == sellerDetails.Seller_Id);
            if (profile == null)
            {
                throw new InvalidOperationException("SellerID not found");
            }
            profile.storeEmail = sellerDetails.storeEmail;
            profile.Phone = sellerDetails.Phone;
            profile.Biography = sellerDetails.Biography;
            profile.image_url = sellerDetails.image_url;
            profile.StoreName = sellerDetails.StoreName;
            profile.MainCategory = sellerDetails.MainCategory;

            _dbContext.Sellers.Update(profile);
            await _dbContext.SaveChangesAsync();
            return profile;
        }
    }
}