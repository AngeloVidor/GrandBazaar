using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.BLL.Messaging.Cart.Messages
{
    public class CartProductListRequest
    {
        public string CorrelationId { get; set; }
        public long User_Id { get; set; }
    }
}