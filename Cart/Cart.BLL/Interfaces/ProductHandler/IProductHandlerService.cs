using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Messages.ProductHandler;

namespace Cart.BLL.Interfaces.ProductHandler
{
    public interface IProductHandlerService
    {
        Task<List<ProductProvider>> GetAllProductsAsync();
    }
}