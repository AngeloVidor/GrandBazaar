using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Interfaces.Payments
{
    public interface IProductRequestSubscriber
    {
        Task Consume();
    }
}