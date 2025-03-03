using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Messaging.Products.Interfaces
{
    public interface IProductsRequestPublisher
    {
        Task Publish(long costumerId);
    }
}