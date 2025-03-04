using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public long OrderItem_Id { get; set; }
        public long Product_Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public long Order_Id { get; set; }

    }
}