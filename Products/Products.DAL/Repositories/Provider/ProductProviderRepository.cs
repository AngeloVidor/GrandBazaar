using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.DAL.Context;
using Products.DAL.Interfaces.Provider;
using Products.Domain.Entities;

namespace Products.DAL.Repositories.Provider
{
    public class ProductProviderRepository : IProductProviderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductProviderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsForDisplayAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(long productId)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Product_Id == productId);
        }
    }
}