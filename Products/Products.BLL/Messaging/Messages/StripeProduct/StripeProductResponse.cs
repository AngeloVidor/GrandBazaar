using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.BLL.Messaging.Messages.StripeProduct
{
    public class StripeProductResponse
    {
        public string CorrelationId { get; set; }
        public bool ProductCreated { get; set; }
    }
}