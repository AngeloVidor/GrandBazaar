using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buyers.BLL.Interfaces.Management;
using Buyers.BLL.Messaging.Costumer.Interfaces;
using Buyers.BLL.Messaging.Costumer.Messages;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Buyers.BLL.Messaging.Costumer.Services
{
    public class UserIdentificationSub : IUserIdentificationSub
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public UserIdentificationSub(IServiceScopeFactory serviceScopeFactory)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "buyer.to.order.request", durable: true, exclusive: false, arguments: null, autoDelete: false);
            _channel.QueueDeclare(queue: "buyer.to.order.response", durable: true, exclusive: false, arguments: null, autoDelete: false);
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Consume()
        {
            Console.WriteLine("Listening...");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _buyerManagementService = scope.ServiceProvider.GetRequiredService<IBuyerManagementService>();
                        var body = ea.Body.ToArray();
                        var messageBody = Encoding.UTF8.GetString(body);
                        var message = JsonConvert.DeserializeObject<UserIdentificationRequest>(messageBody);

                        var correlationId = ea.BasicProperties.CorrelationId;
                        var replyTo = ea.BasicProperties.ReplyTo;

                        var costumerId = await _buyerManagementService.GetBuyerIdByUserIdAsync(message.User_Id);

                        var response = new UserIdentificationResponse
                        {
                            CorrelationId = correlationId,
                            User_Id = message.User_Id,
                            Costumer_Id = costumerId
                        };

                        await Publish(replyTo, costumerId, response);

                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: "buyer.to.order.request", autoAck: false, consumer: consumer);
        }

        public async Task<long> GetCostumerIdAsync(long userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _buyerManagementService = scope.ServiceProvider.GetRequiredService<IBuyerManagementService>();
                return await _buyerManagementService.GetBuyerIdByUserIdAsync(userId);
            }
        }

        public Task Publish(string replyTo, long costumerId, UserIdentificationResponse response)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = response.CorrelationId;

            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);

            return Task.CompletedTask;
        }
    }
}