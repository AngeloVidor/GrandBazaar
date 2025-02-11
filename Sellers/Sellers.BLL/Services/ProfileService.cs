using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sellers.BLL.DTOs;
using Sellers.BLL.Interfaces;
using Sellers.DAL.Interfaces;
using Sellers.Domain.Entities;

namespace Sellers.BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        public async Task<SellerDetailsDto> AddSellerProfileAsync(SellerDetailsDto sellerDetails)
        {
            var sellerEntity = _mapper.Map<SellerDetails>(sellerDetails);
            var response = await _profileRepository.AddSellerProfileAsync(sellerEntity);
            return _mapper.Map<SellerDetailsDto>(response);
        }
    }
}
