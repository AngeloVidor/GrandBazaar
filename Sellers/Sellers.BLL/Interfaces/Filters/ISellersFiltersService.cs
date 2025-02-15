using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.BLL.DTOs;

namespace Sellers.BLL.Interfaces.Filters
{
    public interface ISellersFiltersService
    {
        Task<IEnumerable<SellerDetailsDto>> GetSellersByCategoryAsync(int category);
    }
}