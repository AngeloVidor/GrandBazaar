using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.BLL.Messaging.Messages.ProductHandler
{
    public class ProductResponse
    {
        public string CorrelationId { get; set; }
        public List<ProductProvider> Products = new List<ProductProvider>();
    }
}