using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.BLL.Messaging.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.BLL.Messaging
{
    public class SellerProfileCreatedConsumer : ISellerProfileCreatedConsumer
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly Dictionary<long, long> _recievedIds = new();

        public SellerProfileCreatedConsumer()
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

        public void StartListening()
        {
            Console.WriteLine("Listening...");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receivedData = JsonConvert.DeserializeObject<Message>(message);

                if (receivedData != null)
                {
                    Console.WriteLine($" [x] Received UserId: {receivedData.User_Id}, SellerId: {receivedData.Seller_Id}");
                    _recievedIds[receivedData.User_Id] = receivedData.Seller_Id;
                }
            };
            _channel.BasicConsume(queue: "seller.profile.created", autoAck: true, consumer: consumer);

        }


    }
}