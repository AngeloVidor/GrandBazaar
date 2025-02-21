using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Messages.ProductValidator;

namespace Cart.BLL.Messaging.Events.Interfaces.ProductValidator
{
    public interface IProductValidatorPublisher
    {
        Task<ProductValidatorResponse> Publish(int quantity, long productId);
    }
}