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
        private readonly IProfileManagementRepository _profileManagement;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper, IProfileManagementRepository profileManagement)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _profileManagement = profileManagement;
        }

        public async Task<SellerDetailsDto> AddSellerProfileAsync(SellerDetailsDto sellerDetails)
        {
            var alreadyExists = await _profileManagement.UserHasSellerProfileAsync(sellerDetails.User_Id);
            if (alreadyExists)
            {
                throw new InvalidOperationException("User already has a seller profile.");
            }

            var sellerEntity = _mapper.Map<SellerDetails>(sellerDetails);
            var response = await _profileRepository.AddSellerProfileAsync(sellerEntity);
            return _mapper.Map<SellerDetailsDto>(response);
        }

        public async Task<SellerDetailsDto> GetMyProfileAsync(long userId)
        {

            var sellerProfile = await _profileRepository.GetMyProfileAsync(userId);
            var result = _mapper.Map<SellerDetailsDto>(sellerProfile);
            return result;

        }
    }
}
