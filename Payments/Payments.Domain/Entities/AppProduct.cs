using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Domain.Entities
{
    public class AppProduct
    {
        public long Product_Id { get; set; } //fk original product
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal UnitAmount { get; set; }
        public int StockQuantity { get; set; }
    
    }
}