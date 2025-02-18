using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Buyers.Domain.Domain
{
    public class Buyer
    {
        [Key]
        public long Buyer_Id { get; set; }
        public string Username { get; set; }
        public long User_Id { get; set; }
        public string Biography { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public string ImageUrl { get; set; }
    }
}