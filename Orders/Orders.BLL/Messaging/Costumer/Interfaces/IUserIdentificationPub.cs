using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Orders.BLL.Messaging.Costumer.Interfaces
{
    public interface IUserIdentificationPub
    {
        void Publish(long userId);
    }
}