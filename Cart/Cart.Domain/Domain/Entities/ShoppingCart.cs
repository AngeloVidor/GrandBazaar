using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.Domain.Domain.Entities
{
    public class ShoppingCart
    {
        [Key]
        public long Cart_Id { get; set; }
        public long Buyer_Id { get; set; }
        public long User_Id { get; set; }
        public bool Is_Active { get; set; }
        public decimal TotalPrice { get; set; }
    }
}