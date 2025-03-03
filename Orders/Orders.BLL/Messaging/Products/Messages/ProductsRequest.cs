using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Messaging.Products.Messages
{
    public class ProductsRequest
    {
        public string CorrelationId { get; set; }
        public long Costumer_Id { get; set; }
    }
}