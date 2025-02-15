using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.Domain.Entities;

namespace Products.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddNewProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
    }
}