using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.BLL.Messaging.Messages.StripeProduct
{
    public class StripeProductRequest
    {
        public string CorrelationId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal UnitAmount { get; set; }
        public int StockQuantity { get; set; }
        public long Product_Id { get; set; }
    }
}