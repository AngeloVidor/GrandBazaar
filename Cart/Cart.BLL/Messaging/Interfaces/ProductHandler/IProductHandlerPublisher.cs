using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Messages.ProductHandler;

namespace Cart.BLL.Messaging.Interfaces.ProductHandler
{
    public interface IProductHandlerPublisher
    {
        Task<List<ProductProvider>> Publish();
    }

}