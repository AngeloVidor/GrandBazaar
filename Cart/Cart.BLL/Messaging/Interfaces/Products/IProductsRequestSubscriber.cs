using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Messages.Products;

namespace Cart.BLL.Messaging.Interfaces.Products
{
    public interface IProductsRequestSubscriber
    {
        Task Consume();
        Task Publish(ProductResponse response, string replyTo);
    }
}