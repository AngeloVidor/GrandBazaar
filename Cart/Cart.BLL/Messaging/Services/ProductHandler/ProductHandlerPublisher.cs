using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Interfaces.ProductHandler;
using Cart.BLL.Messaging.Messages.ProductHandler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.BLL.Messaging.Services.ProductHandler
{
    public class ProductHandlerPublisher : IProductHandlerPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ConcurrentDictionary<string, TaskCompletionSource<List<ProductProvider>>> _requests = new ConcurrentDictionary<string, TaskCompletionSource<List<ProductProvider>>>();

        public ProductHandlerPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "cart.product.handler.request", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "cart.product.handler.response", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var bodyMessage = Encoding.UTF8.GetString(body);
                var response = JsonConvert.DeserializeObject<ProductResponse>(bodyMessage);

                if (_requests.TryGetValue(response.CorrelationId, out var tcs))
                {
                    tcs.TrySetResult(response.Products);
                    _requests.TryRemove(response.CorrelationId, out _);
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "cart.product.handler.response", autoAck: false, consumer: consumer);

        }

        public Task<List<ProductProvider>> Publish()
        {
            var request = new ProductRequest
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));

            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = request.CorrelationId;
            properties.ReplyTo = "cart.product.handler.response";

            var tcs = new TaskCompletionSource<List<ProductProvider>>();
            _requests.TryAdd(request.CorrelationId, tcs);

            _channel.BasicPublish(exchange: "", routingKey: "cart.product.handler.request", basicProperties: properties, body: body);

            return tcs.Task;
        }
    }
}