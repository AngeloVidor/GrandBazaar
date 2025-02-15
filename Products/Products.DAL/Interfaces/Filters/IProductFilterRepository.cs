using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.Domain.Entities;

namespace Products.DAL.Interfaces.Filters
{
    public interface IProductFilterRepository
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int category);
        Task<IEnumerable<Product>> GetProductByQualityAsync(int quality);
        
    }
}