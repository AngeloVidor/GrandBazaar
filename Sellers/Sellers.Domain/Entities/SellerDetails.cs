using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sellers.Domain.Entities
{
    public class SellerDetails
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

    }
}