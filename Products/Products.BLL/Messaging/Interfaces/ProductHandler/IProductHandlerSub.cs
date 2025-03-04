using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.BLL.Messaging.Messages.ProductHandler;

namespace Products.BLL.Messaging.Interfaces.ProductHandler
{
    public interface IProductHandlerSub
    {
        Task Consume();
        Task Publish(ProductResponse response, string replyTo);
    }
}