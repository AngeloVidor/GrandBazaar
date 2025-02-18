using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Buyers.BLL.DTOs;
using Buyers.BLL.Interfaces;
using Buyers.BLL.Interfaces.S3;
using Buyers.DAL.Interfaces;
using Buyers.DAL.Interfaces.Management;
using Buyers.Domain.Domain;

namespace Buyers.BLL.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IMapper _mapper;
        private readonly IS3StorageService _s3StorageService;

        public BuyerService(IBuyerRepository buyerRepository, IMapper mapper, IS3StorageService s3StorageService)
        {
            _buyerRepository = buyerRepository;
            _mapper = mapper;
            _s3StorageService = s3StorageService;
        }

        public async Task<BuyerDto> AddNewBuyerAsync(BuyerDto buyer)
        {
            var buyerEntity = _mapper.Map<Buyer>(buyer);
            
            var imageUrl = await _s3StorageService.UploadImageAsync(buyer.ImageFile);
            buyerEntity.ImageUrl = imageUrl;
            
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