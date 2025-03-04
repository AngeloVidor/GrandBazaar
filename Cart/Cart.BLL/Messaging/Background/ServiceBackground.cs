using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Interfaces.Products;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cart.BLL.Messaging.Background
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
            var consumer = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IProductsRequestSubscriber>();
            consumer.Consume();
        }
    }
}