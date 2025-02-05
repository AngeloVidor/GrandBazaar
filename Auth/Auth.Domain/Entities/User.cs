using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public class User
    {
        [Key]
        public long User_ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public AccountType AccountType { get; set; }
    }
}