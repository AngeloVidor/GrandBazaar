using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.BLL.DTOs;

namespace Products.BLL.Interfaces.Filters
{
    public interface IProductFilterService
    {
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int category);
    }
}