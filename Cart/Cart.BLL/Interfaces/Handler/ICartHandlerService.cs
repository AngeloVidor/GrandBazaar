using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.DTOs;
using Cart.Domain.Domain.Entities;

namespace Cart.BLL.Interfaces.Handler
{
    public interface ICartHandlerService
    {
        Task<long> GetCartIdByUserIdAsync(long userId);
        Task<long> GetBuyerIdByCartIdAsync(long cartId);
        Task<long> GetCartIdByCostumerIdAsync(long userId);
        Task<List<ItemDto>> GetItemsFromCartByCartIdAsync(long cartId);

    }
}