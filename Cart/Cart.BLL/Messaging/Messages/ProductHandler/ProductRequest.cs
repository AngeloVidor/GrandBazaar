using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages.ProductHandler
{
    public class ProductRequest
    {
        public string CorrelationId { get; set; }
    }
}