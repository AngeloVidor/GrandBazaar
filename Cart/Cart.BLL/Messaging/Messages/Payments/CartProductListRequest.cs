using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages.Payments
{
    public class CartProductListRequest
    {
        public string CorrelationId { get; set; }
        public long User_Id { get; set; }
    }
}