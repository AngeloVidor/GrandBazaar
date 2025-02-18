using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Buyers.BLL.DTOs;
using Buyers.BLL.Interfaces;
using Buyers.DAL.Interfaces;
using Buyers.DAL.Interfaces.Management;
using Buyers.Domain.Domain;

namespace Buyers.BLL.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IBuyerManagementRepository _buyerManagementRepository;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IMapper _mapper;

        public BuyerService(IBuyerRepository buyerRepository, IMapper mapper, IBuyerManagementRepository buyerManagementRepository)
        {
            _buyerRepository = buyerRepository;
            _mapper = mapper;
            _buyerManagementRepository = buyerManagementRepository;
        }

        public async Task<BuyerDto> AddNewBuyerAsync(BuyerDto buyer)
        {
            var buyerEntity = _mapper.Map<Buyer>(buyer);
            var response = await _buyerRepository.AddNewBuyerAsync(buyerEntity);
            return _mapper.Map<BuyerDto>(response);
        }

        public async Task<BuyerDto> GetMyProfileAsync(long userId)
        {
            var buyer = await _buyerRepository.GetMyProfileAsync(userId);
            if(buyer == null)
            {
                throw new KeyNotFoundException("Buyer not found");
            }
            return _mapper.Map<BuyerDto>(buyer);
        }
    }
}