using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Auth.BLL.Messaging.Events
{
    public class SellerProfileCreatedBackgroundService : BackgroundService
    {
        private readonly ISellerProfileCreatedConsumer _consumer;

        public SellerProfileCreatedBackgroundService(ISellerProfileCreatedConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting RabbitMQ listener...");
            _consumer.StartListening();
            return Task.CompletedTask;
        }
    }
}