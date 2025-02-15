using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sellers.BLL.Interfaces
{
    public interface IProfileManagementService
    {
        Task<long> GetSellerProfileIdByUserIdAsync(long userId);
    }
}