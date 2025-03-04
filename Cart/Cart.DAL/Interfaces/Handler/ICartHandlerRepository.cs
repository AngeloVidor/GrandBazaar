using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Domain.Domain.Entities;

namespace Cart.DAL.Interfaces.Handler
{
    public interface ICartHandlerRepository
    {
        Task<long> GetCartIdByUserIdAsync(long userId);
        Task<long> GetBuyerIdByCartIdAsync(long cartId);
        Task<bool> CartAlreadyExistsAsync(long userId);
        Task<long> GetCartIdByCostumerIdAsync(long userId);
        Task<List<Item>> GetItemsFromCartByCartIdAsync(long cartId);
    }
}