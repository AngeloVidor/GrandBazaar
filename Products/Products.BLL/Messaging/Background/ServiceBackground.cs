using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.BLL.Messaging.Events.Interfaces.BuyerIdentification;
using Products.BLL.Messaging.Interfaces.ProductHandler;

namespace Products.BLL.Messaging.Background
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
            var consumer = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IProductValidatorConsumer>();
            var productHandler = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IProductHandlerSub>();
            consumer.Consume();
            await productHandler.Consume();
        }
    }
}