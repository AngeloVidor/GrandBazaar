using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Products.BLL.DTOs;
using Products.BLL.Interfaces.Filters;
using Products.DAL.Interfaces.Filters;

namespace Products.BLL.Services.Filters
{
    public class ProductFilterService : IProductFilterService
    {
        private readonly IProductFilterRepository _productFilterRepository;
        private readonly IMapper _mapper;

        public ProductFilterService(IProductFilterRepository productFilterRepository, IMapper mapper)
        {
            _productFilterRepository = productFilterRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int category)
        {
            if (category < 0 || category > 13)
            {
                throw new KeyNotFoundException("Category must be between 0 and 13");
            }
            var products = await _productFilterRepository.GetProductsByCategoryAsync(category);
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}