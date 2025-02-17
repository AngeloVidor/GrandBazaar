using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sellers.Domain.Entities;

namespace Sellers.BLL.DTOs
{
    public class UpdatedSellerDetailsDto
    {
        [Key]
        public long Seller_Id { get; set; }
        public string StoreName { get; set; }
        public string Biography { get; set; }
        public string storeEmail { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate = DateTime.UtcNow;
        public long User_Id { get; set; }
        public IECategory MainCategory { get; set; }
        public string image_url { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}