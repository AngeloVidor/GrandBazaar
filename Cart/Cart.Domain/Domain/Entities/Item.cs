using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cart.Domain.Domain.Entities
{
    public class Item
    {
        [Key]
        public long Item_Id { get; set; }
        public long Cart_Id { get; set; }
        public long Product_Id { get; set; } //async communication with product service
        public long Buyer_Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    }
}


