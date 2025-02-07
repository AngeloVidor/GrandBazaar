using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.BLL.DTOs
{
    public class LoginDto
    {
        public long User_ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}