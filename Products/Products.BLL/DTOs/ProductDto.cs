using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.Domain.Entities;

namespace Products.BLL.DTOs
{
    public class ProductDto
    {

        public long Product_Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public long Seller_Id { get; set; }
        public IECategory Category { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public IEQuality Quality { get; set; }
    }
}