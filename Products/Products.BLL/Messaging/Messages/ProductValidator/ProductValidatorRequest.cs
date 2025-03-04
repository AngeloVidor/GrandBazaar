using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.BLL.Messaging.Events.Messages.BuyerIdentification
{
    public class ProductValidatorRequest
    {
        public string CorrelationId { get; set; }
        public int Quantity { get; set; }
        public long Product_Id { get; set; }
    }
}