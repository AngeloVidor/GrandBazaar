using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Entities;

namespace Orders.BLL.DTOs
{
    public class OrderItemDto
    {
        public long Product_Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public long Order_Id { get; set; }
    }
}