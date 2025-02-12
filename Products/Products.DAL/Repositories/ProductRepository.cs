using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.DAL.Context;
using Products.DAL.Interfaces;
using Products.Domain.Entities;

namespace Products.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> AddNewProductAsync(Product product)
        {
            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}