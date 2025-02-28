using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyers.BLL.Messaging.Costumer.Interfaces;
using Buyers.BLL.Messaging.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Buyers.BLL.Messaging.Background
{
    public class Background : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Background(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var userToSeller = scope.ServiceProvider.GetRequiredService<IBuyerIdentificationPublisher>();
                    var buyerToOrder = scope.ServiceProvider.GetRequiredService<IUserIdentificationSub>();

                    userToSeller.Consume();
                    buyerToOrder.Consume();

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
            }
        }
    }
}