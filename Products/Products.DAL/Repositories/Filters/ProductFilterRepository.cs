using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.DAL.Context;
using Products.DAL.Interfaces.Filters;
using Products.Domain.Entities;

namespace Products.DAL.Repositories.Filters
{
    public class ProductFilterRepository : IProductFilterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductFilterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int category)
        {
            throw new NotImplementedException();
        }
    }
}