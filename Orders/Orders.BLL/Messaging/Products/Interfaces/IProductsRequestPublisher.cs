using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.Messaging.Products.Messages;

namespace Orders.BLL.Messaging.Products.Interfaces
{
    public interface IProductsRequestPublisher
    {
        Task<ProductResponse> Publish(long costumerId);
    }
}