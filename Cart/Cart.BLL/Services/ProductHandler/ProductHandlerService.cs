using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.Interfaces.ProductHandler;
using Cart.BLL.Messaging.Interfaces.ProductHandler;
using Cart.BLL.Messaging.Messages.ProductHandler;

namespace Cart.BLL.Services.ProductHandler
{
    public class ProductHandlerService : IProductHandlerService
    {
        private readonly IProductHandlerPublisher _pub;

        public ProductHandlerService(IProductHandlerPublisher pub)
        {
            _pub = pub;
        }

        public async Task<List<ProductProvider>> GetAllProductsAsync()
        {
            var response = await _pub.Publish();
            return response;
        }
    }
}