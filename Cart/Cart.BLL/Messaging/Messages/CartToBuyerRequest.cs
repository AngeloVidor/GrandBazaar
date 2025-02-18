using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages
{
    public class CartToBuyerRequest
    {
        public long User_Id { get; set; }
        public string CorrelationId { get; set; }
    }
}