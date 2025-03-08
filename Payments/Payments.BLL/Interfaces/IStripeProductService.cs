using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payments.Domain.Entities;
using Stripe;

namespace Payments.BLL.Interfaces
{
    public interface IStripeProductService
    {
        Task<bool> CreateProductAsync(AppProduct product);
    }
}