using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orders.BLL.Messaging.Costumer.Interfaces;
using Orders.BLL.Messaging.Costumer.Messages;
using RabbitMQ.Client;

namespace Orders.BLL.Messaging.Costumer.Services
{
    public class UserIdentificationPub : IUserIdentificationPub
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public UserIdentificationPub()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "buyer.to.order.request", durable: true, exclusive: false, arguments: null, autoDelete: false);
            _channel.QueueDeclare(queue: "buyer.to.order.response", durable: true, exclusive: false, arguments: null, autoDelete: false);

        }

        public void Publish(long userId)
        {
            Console.WriteLine("Lets pub that one!!");
            var message = new UserIdentificationRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                User_Id = userId
            };
            Console.WriteLine($"Message: {message.User_Id}");

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            var replyTo = properties.ReplyTo = "buyer.to.order.response";

            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);
            Console.WriteLine("Message sent!");


        }
    }
}