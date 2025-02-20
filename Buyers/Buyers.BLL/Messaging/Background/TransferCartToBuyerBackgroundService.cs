using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.BLL.Messaging.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Buyers.BLL.Messaging.Background
{
    public class TransferCartToBuyerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TransferCartToBuyerBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var transferUserToSellerEvent = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IBuyerIdentificationPublisher>();
            transferUserToSellerEvent.Consume();
        }
    }
}