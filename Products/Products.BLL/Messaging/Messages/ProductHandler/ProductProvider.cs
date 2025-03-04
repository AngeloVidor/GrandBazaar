using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.BLL.Messaging.Messages.ProductHandler
{
    public class ProductProvider
    {
        public long Product_Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public long Seller_Id { get; set; }
    }
}