using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.Domain.Entities;

namespace Sellers.DAL.Interfaces.Provider
{
    public interface ISellerProviderRepository
    {
        Task<SellerDetails> GetSellerByIdAsync(long sellerId);
        Task<IEnumerable<SellerDetails>> GetAllSellersAsync();
    }
}