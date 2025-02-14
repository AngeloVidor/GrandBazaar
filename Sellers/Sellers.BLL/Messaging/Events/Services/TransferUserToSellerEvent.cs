using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sellers.BLL.Interfaces;
using Sellers.BLL.Messaging.Events.Interfaces;
using Sellers.BLL.Messaging.Events.Message;

namespace Sellers.BLL.Messaging.Events.Services
{
    public class TransferUserToSellerEvent : ITransferUserToSellerEvent
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IProfileManagementService _profileManagementService;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TransferUserToSellerEvent(IProfileManagementService profileManagementService, IServiceScopeFactory serviceScopeFactory)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _serviceScopeFactory = serviceScopeFactory;
            _profileManagementService = profileManagementService;

            _channel.QueueDeclare(queue: "user.to.seller.request", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }
        public void Consume()
        {

            Console.WriteLine("Listening...");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var body = ea.Body.ToArray();
                    var messageBody = Encoding.UTF8.GetString(body);
                    var message = JsonConvert.DeserializeObject<UserToSellerRequest>(messageBody);

                    var properties = _channel.CreateBasicProperties();
                    var replyTo = properties.ReplyTo;

                    
                    var sellerId = await GetSellerIdAsync(message.User_Id);


                    var response = new UserToSellerResponse
                    {
                        User_Id = message.User_Id,
                        Seller_Id = sellerId,
                        CorrelationId = message.CorrelationId
                    };

                    Console.WriteLine($"SellerID: {response.Seller_Id}");

                    await Publish(response, replyTo);
                }

            };
            _channel.BasicConsume(queue: "user.to.seller.request", autoAck: false, consumer: consumer);
        }

        public Task Publish(UserToSellerResponse response, string replyTo)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = response.CorrelationId;

            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);

            Console.WriteLine("Response sent!");

            return Task.CompletedTask;
        }


        public async Task<long> GetSellerIdAsync(long userId)
        {
            return await _profileManagementService.GetSellerProfileIdByUserIdAsync(userId);
        }
    }
}