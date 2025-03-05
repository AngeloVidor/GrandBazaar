using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Entities;

namespace Orders.BLL.DTOs
{
    public class OrderDto
    {
        [Key]
        public long Order_Id { get; set; }
        public long Costumer_Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItemDto> Products { get; set; }

    }
}