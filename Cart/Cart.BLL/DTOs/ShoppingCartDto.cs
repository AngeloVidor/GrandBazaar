using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cart.BLL.DTOs
{
    public class ShoppingCartDto
    {
        [Key]
        public long Cart_Id { get; set; }
        public long Buyer_Id { get; set; }
        public long User_Id { get; set; }
        
        [JsonIgnore]
        public bool Is_Active { get; set; }

    }
}