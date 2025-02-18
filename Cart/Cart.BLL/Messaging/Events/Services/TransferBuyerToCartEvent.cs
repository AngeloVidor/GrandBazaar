using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Events.Interfaces;
using Cart.BLL.Messaging.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.BLL.Messaging.Events.Services
{
    public class TransferBuyerToCartEvent : ITransferBuyerToCartEvent
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly TaskCompletionSource<long> _responseTaskSource = new TaskCompletionSource<long>();


        public TransferBuyerToCartEvent()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "cart.to.buyer.request", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var result = JsonConvert.DeserializeObject<CartToBuyerResponse>(message);
                var response = new CartToBuyerResponse
                {
                    User_Id = result.User_Id,
                    Buyer_Id = result.Buyer_Id,
                    CorrelationId = result.CorrelationId
                };

                _channel.BasicAck(ea.DeliveryTag, false);
                _responseTaskSource.TrySetResult(result.Buyer_Id);
            };
            _channel.BasicConsume(queue: "cart.to.buyer.response", autoAck: false, consumer: consumer);
        }

        public void Publish(long userId)
        {
            System.Console.WriteLine("Publishing...");
            var message = new CartToBuyerRequest
            {
                User_Id = userId,
                CorrelationId = Guid.NewGuid().ToString()
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            properties.ReplyTo = "cart.to.buyer.response";

            _channel.BasicPublish(exchange: "", routingKey: "cart.to.buyer.request", basicProperties: properties, body: body);

        }

        public async Task<long> GetBuyerIdAsync(long userId)
        {
            Publish(userId);
            return await _responseTaskSource.Task;
        }
    }
}