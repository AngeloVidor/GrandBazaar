using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.BLL.Interfaces.Checkout
{
    public interface ICheckoutSessionService
    {
        Task CreateCheckoutSessionAsync();
    }
}