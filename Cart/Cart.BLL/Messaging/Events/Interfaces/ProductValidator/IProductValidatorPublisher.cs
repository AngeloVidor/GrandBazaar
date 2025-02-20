using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Events.Interfaces.ProductValidator
{
    public interface IProductValidatorPublisher
    {
        void Publish(int quantity, long productId);
    }
}