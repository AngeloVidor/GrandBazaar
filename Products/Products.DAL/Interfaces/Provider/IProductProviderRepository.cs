using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.Domain.Entities;

namespace Products.DAL.Interfaces.Provider
{
    public interface IProductProviderRepository
    {
        Task<IEnumerable<Product>> GetAllProductsForDisplayAsync();
        Task<Product> GetProductByIdAsync(long productId);
    }
}