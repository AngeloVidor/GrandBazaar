using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.BLL.DTOs;
using Products.Domain.Entities;

namespace Products.BLL.Interfaces.Provider
{
    public interface IProductProviderService
    {
        Task<IEnumerable<ProductDisplayDto>> GetAllProductsForDisplayAsync();
        Task<ProductDisplayDto> GetProductByIdAsync(long productId);

    }
}