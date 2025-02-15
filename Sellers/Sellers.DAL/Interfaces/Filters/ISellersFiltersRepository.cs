using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.Domain.Entities;

namespace Sellers.DAL.Interfaces.Filters
{
    public interface ISellersFiltersRepository
    {
        Task<IEnumerable<SellerDetails>> GetSellersByCategoryAsync(int category);
    }
}