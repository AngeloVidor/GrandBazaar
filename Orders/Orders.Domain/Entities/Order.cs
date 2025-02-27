using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Domain.Entities
{
    public class Order
    {
        [Key]
        public long Order_Id { get; set; }
        public long Costumer_Id { get; set; } //buyer_id
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Products { get; set; }

    }
}