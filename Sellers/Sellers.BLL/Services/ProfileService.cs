using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sellers.BLL.DTOs;
using Sellers.BLL.Interfaces;
using Sellers.BLL.Messaging;
using Sellers.DAL.Interfaces;
using Sellers.Domain.Entities;

namespace Sellers.BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        public UserPublisher _publisher;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper, UserPublisher publisher)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<SellerDetailsDto> AddSellerProfileAsync(SellerDetailsDto sellerDetails)
        {
            var user = await _publisher.ValidateUserIdAsync(sellerDetails.User_Id);
            if (user.UserId <= 0)
            {
                throw new KeyNotFoundException("User not found");
            }

            sellerDetails.User_Id = user.UserId;

            var sellerEntity = _mapper.Map<SellerDetails>(sellerDetails);
            var response = await _profileRepository.AddSellerProfileAsync(sellerEntity);
            return _mapper.Map<SellerDetailsDto>(response);
        }
    }
}
