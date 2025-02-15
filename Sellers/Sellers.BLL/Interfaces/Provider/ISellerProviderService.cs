using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.BLL.DTOs;

namespace Sellers.BLL.Interfaces.Provider
{
    public interface ISellerProviderService
    {
        Task<SellerDetailsDto> GetSellerByIdAsync(long sellerId);
        Task<IEnumerable<SellerDetailsDto>> GetAllSellersAsync();
    }
}