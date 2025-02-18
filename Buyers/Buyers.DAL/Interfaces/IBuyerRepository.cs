using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.Domain.Domain;

namespace Buyers.DAL.Interfaces
{
    public interface IBuyerRepository
    {
        Task<Buyer> AddNewBuyerAsync(Buyer buyer);
    }
}