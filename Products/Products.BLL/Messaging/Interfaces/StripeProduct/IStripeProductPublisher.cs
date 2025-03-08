using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.BLL.DTOs;
using Products.BLL.Messaging.Messages.StripeProduct;

namespace Products.BLL.Messaging.Interfaces.StripeProduct
{
    public interface IStripeProductPublisher
    {
        Task<StripeProductResponse> Publish(ProductDto product);
        Task<bool> CreateStripeProductAsync(ProductDto product);
    }
}