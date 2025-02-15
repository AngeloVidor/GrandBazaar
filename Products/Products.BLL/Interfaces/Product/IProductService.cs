using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.BLL.DTOs;

namespace Products.BLL.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddNewProductAsync(ProductDto product);
        Task<UpdateProductDto> UpdateProductAsync(UpdateProductDto product);
        Task<ProductDto> RemoveProductAsync(long productId);
    }
}