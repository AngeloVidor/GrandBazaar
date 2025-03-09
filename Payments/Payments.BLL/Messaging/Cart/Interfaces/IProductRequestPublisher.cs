using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payments.BLL.Messaging.Cart.Messages;

namespace Payments.BLL.Messaging.Cart.Interfaces
{
    public interface IProductRequestPublisher
    {
        Task<List<ProductList>> Publish(long userId);
    }
}