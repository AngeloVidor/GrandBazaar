using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.BLL.Messaging.Events
{
    public class UserValidationResponse
    {
        public long User_Id { get; set; }
        public string CorrelationId { get; set; }
        public bool IsValid { get; set; }

    }
}