using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Products.BLL.Messaging.Events.Interfaces;
using Products.BLL.Messaging.Events.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Products.BLL.Messaging.Events.Services
{
    public class TransferUserToSellerEvent : ITransferUserToSellerEvent
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TransferUserToSellerEvent()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "user.to.seller.request", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageBody = Encoding.UTF8.GetString(body);
                var result = JsonConvert.DeserializeObject<UserToSellerResponse>(messageBody);
                var response = new UserToSellerResponse
                {
                    CorrelationId = result.CorrelationId,
                    User_Id = result.User_Id,
                    Seller_Id = result.Seller_Id
                };
            };

            _channel.BasicConsume(queue: "seller.to.user.response", autoAck: false, consumer: consumer);
        }

        public void Publish(long userId)
        {
            var request = new UserToSellerRequest
            {
                User_Id = userId,
                CorrelationId = Guid.NewGuid().ToString()
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));

            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = request.CorrelationId;
            properties.ReplyTo = "seller.to.user.response";

            _channel.BasicPublish(exchange: "", routingKey: "user.to.seller.request", basicProperties: properties, body: body);

        }
    }
}