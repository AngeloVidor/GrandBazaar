using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages.Products
{
    public class ProductRequest
    {
        public string CorrelationId { get; set; }
        public long Costumer_Id { get; set; }
    }
}