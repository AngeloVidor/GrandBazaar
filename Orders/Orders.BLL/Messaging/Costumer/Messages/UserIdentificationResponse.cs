using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Messaging.Costumer.Messages
{
    public class UserIdentificationResponse
    {
        public string CorrelationId { get; set; }
        public long User_Id { get; set; }
        public long Costumer_Id { get; set; }
    }
}