using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.Domain.Entities;

namespace Sellers.DAL.Interfaces
{
    public interface IProfileRepository
    {
        Task<SellerDetails> AddSellerProfileAsync(SellerDetails sellerDetails);
        Task<SellerDetails> UpdateSellerProfileAsync(SellerDetails sellerDetails);
        Task<SellerDetails> GetMyProfileAsync(long userId);
    }
}