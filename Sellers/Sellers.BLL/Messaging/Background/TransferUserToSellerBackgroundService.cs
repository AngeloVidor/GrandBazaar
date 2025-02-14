using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sellers.BLL.Messaging.Events.Interfaces;

namespace Sellers.BLL.Messaging.Background
{
    public class TransferUserToSellerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TransferUserToSellerBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var transferUserToSellerEvent = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ITransferUserToSellerEvent>();
            transferUserToSellerEvent.Consume();
        }


    }
}