using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.BLL.Messaging.Events.Messages
{
    public class UserToSellerResponse
    {
        public long User_Id { get; set; }
        public long Seller_Id { get; set; }
        public string CorrelationId { get; set; }
    }
}