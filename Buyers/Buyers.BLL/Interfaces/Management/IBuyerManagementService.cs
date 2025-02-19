using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyers.BLL.Interfaces.Management
{
    public interface IBuyerManagementService
    {
        Task<long> GetBuyerIdByUserIdAsync(long userId);

    }
}