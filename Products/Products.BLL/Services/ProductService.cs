using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Products.BLL.DTOs;
using Products.BLL.Interfaces;
using Products.BLL.Messaging.Events.Interfaces;
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
    }
}