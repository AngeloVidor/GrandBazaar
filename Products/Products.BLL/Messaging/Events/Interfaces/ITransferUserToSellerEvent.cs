using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.BLL.Messaging.Events.Messages;

namespace Products.BLL.Messaging.Events.Interfaces
{
    public interface ITransferUserToSellerEvent
    {
        void Publish(long userId);
    }
}