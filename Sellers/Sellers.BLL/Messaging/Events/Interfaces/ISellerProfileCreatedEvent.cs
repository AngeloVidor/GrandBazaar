using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sellers.BLL.Messaging.Events
{
    public interface ISellerProfileCreatedEvent
    {
        void Pub(long sellerId, long userId);
    }
}