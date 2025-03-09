using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.BLL.Messaging.Cart.Messages
{
    public class ProductList
    {
        public long Item_Id { get; set; }
        public long Cart_Id { get; set; }
        public long Product_Id { get; set; }
        public long Buyer_Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}