using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Products.BLL.DTOs;
using Products.BLL.Interfaces.Provider;
using Products.DAL.Interfaces.Provider;
using Products.Domain.Entities;

namespace Products.BLL.Services.Provider
{
    public class ProductProviderService : IProductProviderService
    {
        private readonly IProductProviderRepository _productProviderRepo;
        private readonly IMapper _mapper;

        public ProductProviderService(IProductProviderRepository productProviderRepo, IMapper mapper)
        {
            _productProviderRepo = productProviderRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDisplayDto>> GetAllProductsForDisplayAsync()
        {
            var products = await _productProviderRepo.GetAllProductsForDisplayAsync();
            if (products == null)
            {
                throw new InvalidOperationException("Failed to retrieve the product list for display.");
            }

            var productDtos = _mapper.Map<List<ProductDisplayDto>>(products);
            foreach (var dto in productDtos)
            {
                dto.CategoryValue = Enum.GetName(typeof(IECategory), dto.Category) ?? "Unknown";
                dto.QualityValue = Enum.GetName(typeof(IEQuality), dto.Quality) ?? "Unknown";
            }

            return productDtos;

        }
    }
}