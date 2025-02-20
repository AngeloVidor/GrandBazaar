using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Domain.Domain.Entities;

namespace Cart.DAL.Interfaces.Management
{
    public interface ICartManagementRepository
    {
        Task<Item> AddItemIntoCartAsync(Item item);
    }
}