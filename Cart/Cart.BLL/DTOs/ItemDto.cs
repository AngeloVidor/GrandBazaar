using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.DTOs
{
    public class ItemDto
    {
        [Key]
        public long Item_Id { get; set; }
        public long Cart_Id { get; set; }
        public long Product_Id { get; set; }
        public long Buyer_Id { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}