using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.BLL.Messaging.Costumer.Messages;

namespace Buyers.BLL.Messaging.Costumer.Interfaces
{
    public interface IUserIdentificationSub
    {
        void Consume();
        Task Publish(string replyTo, long costumerId, UserIdentificationResponse response);
        Task<long> GetCostumerIdAsync(long userId);
    }
}