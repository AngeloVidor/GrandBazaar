using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payments.BLL.Messaging.Product.Interfaces;

namespace Payments.BLL.Messaging.Background
{
    public class ServiceBackground : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ServiceBackground(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _stripeProduct = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IStripeProductSubscriber>();
            await _stripeProduct.Consume();
        }
    }
}