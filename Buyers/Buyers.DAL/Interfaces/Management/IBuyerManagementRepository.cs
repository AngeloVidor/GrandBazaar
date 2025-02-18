using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.Domain.Domain;

namespace Buyers.DAL.Interfaces.Management
{
    public interface IBuyerManagementRepository
    {
        Task<long> GetBuyerIdByUserIdAsync(long userId);
    }
}