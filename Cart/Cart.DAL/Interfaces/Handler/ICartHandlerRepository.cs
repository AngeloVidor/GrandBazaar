using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.DAL.Interfaces.Handler
{
    public interface ICartHandlerRepository
    {
        Task<long> GetCartIdByUserIdAsync(long userId);
        Task<long> GetBuyerIdByCartIdAsync(long cartId);
    }
}