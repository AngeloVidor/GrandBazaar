using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.BLL.Messaging.Events.Messages.BuyerIdentification;

namespace Products.BLL.Messaging.Events.Interfaces.BuyerIdentification
{
    public interface IProductValidatorConsumer
    {
        void Consume();
        void Publish(ProductValidatorResponse response, string replyTo);
        Task<bool> ValidateProductAsync(ProductValidatorRequest request);
    }
}