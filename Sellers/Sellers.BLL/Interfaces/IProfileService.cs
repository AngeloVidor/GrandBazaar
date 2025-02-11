using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.BLL.DTOs;

namespace Sellers.BLL.Interfaces
{
    public interface IProfileService
    {
        Task<SellerDetailsDto> AddSellerProfileAsync(SellerDetailsDto sellerDetails);
        Task<SellerDetailsDto> GetMyProfileAsync(long userId);

    }
}