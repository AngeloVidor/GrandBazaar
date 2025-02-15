using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sellers.BLL.DTOs;
using Sellers.BLL.Interfaces.Provider;
using Sellers.DAL.Interfaces.Provider;

namespace Sellers.BLL.Services.Provider
{
    public class SellerProviderService : ISellerProviderService
    {
        private readonly ISellerProviderRepository _sellerProviderRepository;
        private readonly IMapper _mapper;

        public SellerProviderService(ISellerProviderRepository sellerProviderRepository, IMapper mapper)
        {
            _sellerProviderRepository = sellerProviderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SellerDetailsDto>> GetAllSellersAsync()
        {
            var sellers = await _sellerProviderRepository.GetAllSellersAsync();
            if (sellers == null)
            {
                throw new InvalidOperationException("Failed to retrieve sellers from repository.");

            }
            return _mapper.Map<List<SellerDetailsDto>>(sellers);
        }

        public async Task<SellerDetailsDto> GetSellerByIdAsync(long sellerId)
        {
            if (sellerId <= 0)
            {
                throw new InvalidOperationException("SellerId must be a positive integer");
            }
            var seller = await _sellerProviderRepository.GetSellerByIdAsync(sellerId);
            return _mapper.Map<SellerDetailsDto>(seller);
        }
    }
}