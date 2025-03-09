using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payments.BLL.Interfaces.Checkout;
using Payments.Domain.Entities;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace Payments.BLL.Services.Checkout
{
    public class CheckoutSessionService : ICheckoutSessionService
    {
        private readonly Stripe.Checkout.SessionService _session;

        public CheckoutSessionService(Stripe.Checkout.SessionService session)
        {
            _session = session;
        }

        // this method must receive a list of products
        public async Task CreateCheckoutSessionAsync()
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = "",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        Price = "1",
                        Quantity = 2
                    },
                },
                Mode = "payment",
            };
            await _session.CreateAsync(options);
        }

        // private async Task<List<AppProduct>> GetProductDataAsync(long userId)
        // {   
                //Publish.....................................
        // }



    }


}