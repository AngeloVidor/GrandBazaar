using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sellers.DAL.Context;
using Sellers.DAL.Interfaces.Filters;
using Sellers.Domain.Entities;

namespace Sellers.DAL.Repositories.Filters
{
    public class SellersFiltersRepository : ISellersFiltersRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SellersFiltersRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SellerDetails>> GetSellersByCategoryAsync(int category)
        {
            var categoryEnum = (IECategory)category;
            return await _dbContext.Sellers.Where(x => x.MainCategory == categoryEnum).ToListAsync();
        }
    }
}