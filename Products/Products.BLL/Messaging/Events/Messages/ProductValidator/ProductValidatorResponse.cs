using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.BLL.Messaging.Events.Messages.BuyerIdentification
{
    public class ProductValidatorResponse
    {
        public string CorrelationId { get; set; }
        public int Quantity { get; set; }
        public long Product_Id { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public string ErrorMessage { get; set; }
    }
}