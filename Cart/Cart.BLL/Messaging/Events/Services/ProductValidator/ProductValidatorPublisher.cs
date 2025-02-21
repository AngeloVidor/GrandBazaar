using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Events.Interfaces.ProductValidator;
using Cart.BLL.Messaging.Messages.ProductValidator;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.BLL.Messaging.Events.Services.ProductValidator
{
    public class ProductValidatorPublisher : IProductValidatorPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ConcurrentDictionary<string, TaskCompletionSource<ProductValidatorResponse>> _pendingResponses = new ConcurrentDictionary<string, TaskCompletionSource<ProductValidatorResponse>>();

        public ProductValidatorPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "product.validator.request", durable: true, autoDelete: false, exclusive: false, arguments: null);
            _channel.QueueDeclare(queue: "product.validator.response", durable: true, autoDelete: false, exclusive: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var response = JsonConvert.DeserializeObject<ProductValidatorResponse>(message);

                if (_pendingResponses.TryGetValue(response.CorrelationId, out var tcs))
                {
                    tcs.SetResult(response);
                    _pendingResponses.TryRemove(response.CorrelationId, out _);
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "product.validator.response", autoAck: false, consumer: consumer);

        }

        public async Task<ProductValidatorResponse> Publish(int quantity, long productId)
        {
            Console.WriteLine("Publishing...");
            var message = new ProductValidatorRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Quantity = quantity,
                Product_Id = productId
            };

            Console.WriteLine($"Message data: {message.CorrelationId}, {message.Product_Id}, {message.Quantity}");
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var props = _channel.CreateBasicProperties();

            props.CorrelationId = message.CorrelationId;
            props.ReplyTo = "product.validator.response";

            Console.WriteLine("Preparing to publish...");
            _channel.BasicPublish(exchange: "", routingKey: "product.validator.request", basicProperties: props, body: body);
            Console.WriteLine("Message sent!");


            var tcs = new TaskCompletionSource<ProductValidatorResponse>();
            _pendingResponses.TryAdd(message.CorrelationId, tcs);
            return await tcs.Task;
        }
    }
}