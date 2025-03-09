using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.BLL.Messaging.Messages.Payments
{
    public class CartProductListResponse
    {

        public string CorrelationId { get; set; }
        public List<ProductList> Products = new List<ProductList>();
    }
}