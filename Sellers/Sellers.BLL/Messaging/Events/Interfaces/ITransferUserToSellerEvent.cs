using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sellers.BLL.Messaging.Events.Message;

namespace Sellers.BLL.Messaging.Events.Interfaces
{
    public interface ITransferUserToSellerEvent
    {
        void Consume();
        Task<long> GetSellerIdAsync(long userId);
        Task Publish(UserToSellerResponse response, string replyTo);
    }
}