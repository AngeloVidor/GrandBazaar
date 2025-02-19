using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buyers.BLL.Interfaces.Management;
using Buyers.BLL.Messaging.Events.Interfaces;
using Buyers.BLL.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Buyers.BLL.Messaging.Events.Services
{
    public class TransferCartToBuyerEvent : ITransferCartToBuyerEvent
    {
        private readonly IBuyerManagementService _buyerManagementService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TransferCartToBuyerEvent(IBuyerManagementService buyerManagementService, IServiceScopeFactory serviceScopeFactory)
        {
            var factory = new ConnectionFactory { HostName = "Localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "cart.to.buyer.request", durable: true, exclusive: false, arguments: null, autoDelete: false);
            _buyerManagementService = buyerManagementService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Consume()
        {
            System.Console.WriteLine("Listening...");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var body = ea.Body.ToArray();
                        var messageBody = Encoding.UTF8.GetString(body);
                        var content = JsonConvert.DeserializeObject<TransferCartToBuyerRequest>(messageBody);

                        var replyTo = ea.BasicProperties.ReplyTo;
                        var correlationId = ea.BasicProperties.CorrelationId;

                        var buyerId = await GetBuyerIdByUserIdAsync(content.User_Id);

                        var response = new TransferCartToBuyerResponse
                        {
                            Buyer_Id = buyerId,
                            User_Id = content.User_Id,
                            CorrelationId = correlationId
                        };

                        Console.WriteLine($"Sending message: {response.CorrelationId}, {response.Buyer_Id}, {response.User_Id}");
                        Publish(response, replyTo);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };
            _channel.BasicConsume(queue: "cart.to.buyer.request", autoAck: false, consumer: consumer);
        }

        public Task Publish(TransferCartToBuyerResponse response, string replyTo)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();

            properties.CorrelationId = response.CorrelationId;
            properties.ReplyTo = replyTo;

            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);
            return Task.CompletedTask;
        }

        public async Task<long> GetBuyerIdByUserIdAsync(long userId)
        {
            return await _buyerManagementService.GetBuyerIdByUserIdAsync(userId);
        }
    }
}