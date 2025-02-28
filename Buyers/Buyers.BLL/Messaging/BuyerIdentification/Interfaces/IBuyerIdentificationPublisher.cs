using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.BLL.Messaging.Messages;

namespace Buyers.BLL.Messaging.Events.Interfaces
{
    public interface IBuyerIdentificationPublisher
    {
        void Consume();
        Task Publish(TransferCartToBuyerResponse response, string replyTo);
    }
}