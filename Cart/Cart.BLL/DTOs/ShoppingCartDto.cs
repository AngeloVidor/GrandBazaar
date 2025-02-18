using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.DTOs
{
    public class ShoppingCartDto
    {
        [Key]
        public long Cart_Id { get; set; }
        public long Buyer_Id { get; set; }
    }
}