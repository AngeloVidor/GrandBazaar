using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Payments.BLL.Messaging.Cart.Interfaces;
using Payments.BLL.Messaging.Cart.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Payments.BLL.Messaging.Cart.Services
{
    public class ProductRequestPublisher : IProductRequestPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ConcurrentDictionary<string, TaskCompletionSource<List<ProductList>>> _pendingTasks = new ConcurrentDictionary<string, TaskCompletionSource<List<ProductList>>>();

        public ProductRequestPublisher()
        {
            var _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "cart.product.list.request", durable: true, autoDelete: false, arguments: null, exclusive: false);
            _channel.QueueDeclare(queue: "cart.product.list.response", durable: true, autoDelete: false, arguments: null, exclusive: false);


            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var responseBody = Encoding.UTF8.GetString(body);
                var responseMessage = JsonConvert.DeserializeObject<CartProductListResponse>(responseBody);

                if (_pendingTasks.TryGetValue(responseMessage.CorrelationId, out var tcs))
                {
                    tcs.TrySetResult(responseMessage.Products);
                    _pendingTasks.TryRemove(responseMessage.CorrelationId, out _);
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "cart.product.list.response", autoAck: false, consumer: consumer);
        }

        public async Task<List<ProductList>> Publish(long userId)
        {
            Console.WriteLine("Publishing...");
            var message = new CartProductListRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                User_Id = userId
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            properties.ReplyTo = "cart.product.list.response";

            var tcs = new TaskCompletionSource<List<ProductList>>();
            _pendingTasks.TryAdd(message.CorrelationId, tcs);

            _channel.BasicPublish(exchange: "", routingKey: "cart.product.list.request", basicProperties: properties, body: body);

            return await tcs.Task;
        }
    }
}