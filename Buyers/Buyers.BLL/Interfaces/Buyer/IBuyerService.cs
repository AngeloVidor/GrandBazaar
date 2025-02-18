using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.BLL.DTOs;

namespace Buyers.BLL.Interfaces
{
    public interface IBuyerService
    {
        Task<BuyerDto> AddNewBuyerAsync(BuyerDto buyer);
        Task<BuyerDto> GetMyProfileAsync(long buyerId);
    }
}