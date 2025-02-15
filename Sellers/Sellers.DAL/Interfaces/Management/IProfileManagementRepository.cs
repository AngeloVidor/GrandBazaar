using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sellers.DAL.Interfaces
{
    public interface IProfileManagementRepository
    {
        Task<bool> UserHasSellerProfileAsync(long userId);
         Task<long> GetSellerProfileIdByUserIdAsync(long userId);
    }
}