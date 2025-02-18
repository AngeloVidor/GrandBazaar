using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Buyers.BLL.DTOs
{
    public class BuyerDto
    {
        [Key]
        public long Buyer_Id { get; set; }
        public string Username { get; set; }

        [Required]
        public long User_Id { get; set; }
        public string Biography { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}