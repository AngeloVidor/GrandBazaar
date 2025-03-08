using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.BLL.Messaging.Product.Messages
{
    public class StripeProductResponse
    {
        public string CorrelationId { get; set; }
        public bool ProductCreated { get; set; }
    }
}