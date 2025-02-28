using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyers.BLL.Messaging.Messages
{
    public class TransferCartToBuyerResponse
    {
        public string CorrelationId {get; set;}
        public long User_Id {get; set;}
        public long Buyer_Id {get; set;}
    }
}