using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Orders.BLL.Messaging.Costumer.Interfaces
{
    public interface IUserIdentificationPub
    {
        Task<long> Publish(long userId);
        Task<long> GetCostumerIdAsync(long userId);
    }
}