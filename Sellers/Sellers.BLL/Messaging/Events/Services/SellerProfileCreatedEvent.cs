using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Sellers.BLL.Messaging.Events
{
    public class SellerProfileCreatedEvent : ISellerProfileCreatedEvent
    {
        private IConnection _connection;
        private IModel _channel;


        public SellerProfileCreatedEvent()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "seller.profile.created",
                      durable: true,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);
        }

        public void Pub(long sellerId, long userId)
        {
            Console.WriteLine("Publish...");

            var message = new Message
            {
                User_Id = userId,
                Seller_Id = sellerId
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            try
            {
                _channel.BasicPublish(exchange: "", routingKey: "seller.profile.created", basicProperties: null, body: body);
                Console.WriteLine($" [x] Sent '{JsonConvert.SerializeObject(message)}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] Error sending message: {ex.Message}");
            }


        }
    }
}