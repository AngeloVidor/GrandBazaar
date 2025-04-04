using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages.Products
{
    public class RequestedProducts
    {
        public long Product_Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{ProductName} - {Price:C}";
        }
    }
}