using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Products.BLL.DTOs;
using Products.BLL.Interfaces;
using Products.BLL.Messaging.Events.Interfaces;
using Products.BLL.Messaging.Events.Services;
using Products.DAL.Interfaces;
using Products.Domain.Entities;

namespace Products.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> AddNewProductAsync(ProductDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            var response = await _productRepository.AddNewProductAsync(productEntity);
            return _mapper.Map<ProductDto>(response);
        }

        public async Task<UpdateProductDto> UpdateProductAsync(UpdateProductDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            var updatedProduct = await _productRepository.UpdateProductAsync(productEntity);
            return _mapper.Map<UpdateProductDto>(updatedProduct);
        }

        public async Task<ProductDto> RemoveProductAsync(long productId)
        {
            if (productId <= 0)
            {
                throw new InvalidOperationException("ProductID must be a positive integer");
            }
            var deletedProduct = await _productRepository.RemoveProductAsync(productId);
            return _mapper.Map<ProductDto>(deletedProduct);
        }
    }
}