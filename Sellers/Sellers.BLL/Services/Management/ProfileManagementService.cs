using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.BLL.Interfaces;
using Sellers.DAL.Interfaces;

namespace Sellers.BLL.Services
{
    public class ProfileManagementService : IProfileManagementService
    {
        private readonly IProfileManagementRepository _profileManagementRepository;

        public ProfileManagementService(IProfileManagementRepository profileManagementRepository)
        {
            _profileManagementRepository = profileManagementRepository;
        }

        public async Task<long> GetSellerProfileIdByUserIdAsync(long userId)
        {
            return await _profileManagementRepository.GetSellerProfileIdByUserIdAsync(userId);
        }
    }
}