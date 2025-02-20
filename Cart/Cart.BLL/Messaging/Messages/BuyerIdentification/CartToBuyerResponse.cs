using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages
{
    public class CartToBuyerResponse
    {
        public long Buyer_Id { get; set; }
        public long User_Id { get; set; }
        public string CorrelationId { get; set; }
    }
}