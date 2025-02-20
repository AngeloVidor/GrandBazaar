using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages.ProductValidator
{
    public class ProductValidatorResponse
    {
        public string CorrelationId { get; set; }
        public int Quantity { get; set; }
        public long Product_Id { get; set; }
        public bool IsAvailable { get; set; }
    }
}