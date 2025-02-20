using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Events.Interfaces
{
    public interface ITransferBuyerToCartEvent
    {
        Task<long> Publish(long userId);
        Task<long> GetBuyerIdAsync(long userId);
    }
}