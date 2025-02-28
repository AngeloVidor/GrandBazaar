using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyers.BLL.Messaging.Costumer.Messages
{
    public class UserIdentificationRequest
    {
        public string CorrelationId { get; set; }
        public long User_Id { get; set; }
    }
}