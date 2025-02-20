using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Interfaces.Handler
{
    public interface ICartHandlerService
    {
        Task<long> GetCartIdByUserIdAsync(long userId);
        Task<long> GetBuyerIdByCartIdAsync(long cartId);
    }
}