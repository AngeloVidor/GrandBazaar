using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sellers.BLL.DTOs;
using Sellers.BLL.Interfaces.Filters;
using Sellers.DAL.Interfaces.Filters;

namespace Sellers.BLL.Services.Filters
{
    public class SellersFiltersService : ISellersFiltersService
    {
        private readonly ISellersFiltersRepository _sellersFiltersRepository;
        private readonly IMapper _mapper;

        public SellersFiltersService(ISellersFiltersRepository sellersFiltersRepository, IMapper mapper)
        {
            _sellersFiltersRepository = sellersFiltersRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SellerDetailsDto>> GetSellersByCategoryAsync(int category)
        {
            if (category < 0 || category > 13)
            {
                throw new KeyNotFoundException("Category must be between 0 and 13");
            }

            var products = await _sellersFiltersRepository.GetSellersByCategoryAsync(category);
            return _mapper.Map<List<SellerDetailsDto>>(products);
        }
    }
}