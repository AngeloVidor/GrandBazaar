using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.BLL.Interfaces.Management;
using Buyers.DAL.Interfaces.Management;

namespace Buyers.BLL.Services.Management
{
    public class BuyerManagementService : IBuyerManagementService
    {
        private readonly IBuyerManagementRepository _buyerManagementRepository;

        public BuyerManagementService(IBuyerManagementRepository buyerManagementRepository)
        {
            _buyerManagementRepository = buyerManagementRepository;
        }

        public async Task<long> GetBuyerIdByUserIdAsync(long userId)
        {
            if (userId <= 0)
            {
                throw new InvalidOperationException("Invalid userId");
            }
            return await _buyerManagementRepository.GetBuyerIdByUserIdAsync(userId);
        }
    }
}