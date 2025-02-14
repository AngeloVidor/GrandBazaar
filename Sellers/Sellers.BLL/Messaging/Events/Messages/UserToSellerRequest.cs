using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sellers.BLL.Messaging.Events.Message
{
    public class UserToSellerRequest
    {
        public long User_Id { get; set; }
        public string CorrelationId { get; set; }
    }
}