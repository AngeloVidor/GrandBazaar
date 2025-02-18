using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Buyers.BLL.DTOs;
using Buyers.BLL.Interfaces;
using Buyers.DAL.Interfaces;
using Buyers.Domain.Domain;

namespace Buyers.BLL.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IMapper _mapper;

        public BuyerService(IBuyerRepository buyerRepository, IMapper mapper)
        {
            _buyerRepository = buyerRepository;
            _mapper = mapper;
        }

        public async Task<BuyerDto> AddNewBuyerAsync(BuyerDto buyer)
        {
            var buyerEntity = _mapper.Map<Buyer>(buyer);
            var response = await _buyerRepository.AddNewBuyerAsync(buyerEntity);
            return _mapper.Map<BuyerDto>(response);
        }
    }
}