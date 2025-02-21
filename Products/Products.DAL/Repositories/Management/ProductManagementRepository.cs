using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.DAL.Context;
using Products.DAL.Interfaces.Management;
using Products.Domain.Entities;

namespace Products.DAL.Repositories.Management
{
    public class ProductManagementRepository : IProductManagementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductManagementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> ValidateProductAsync(int quantity, long productId)
        {
            throw new NotImplementedException();

        }
    }
}