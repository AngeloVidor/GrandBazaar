using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Messaging.Products.Messages
{
    public class ProductResponse
    {
        public string CorrelationId { get; set; }
        public long Costumer_Id { get; set; }
        public List<RequestedProducts> Products { get; set; } = new List<RequestedProducts>();

    }
}