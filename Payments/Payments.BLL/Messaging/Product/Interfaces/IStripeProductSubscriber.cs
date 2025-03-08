using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payments.BLL.Messaging.Product.Messages;

namespace Payments.BLL.Messaging.Product.Interfaces
{
    public interface IStripeProductSubscriber
    {
        Task Consume();
        Task Publish(StripeProductResponse response, string replyTo);
    }
}