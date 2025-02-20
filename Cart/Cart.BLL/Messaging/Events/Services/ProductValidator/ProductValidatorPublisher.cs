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

namespace Cart.BLL.Messaging.Events.Services.ProductValidator
{
    public class ProductValidatorPublisher : IProductValidatorPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ConcurrentDictionary<string, TaskCompletionSource<long>> _pendingResponses = new ConcurrentDictionary<string, TaskCompletionSource<long>>();

        public ProductValidatorPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "product.validator.request", durable: true, autoDelete: false, exclusive: false, arguments: null);
            _channel.QueueDeclare(queue: "product.validator.response", durable: true, autoDelete: false, exclusive: false, arguments: null);

        }

        public void Publish(int quantity, long productId)
        {
            Console.WriteLine("Publishing...");
            var message = new ProductValidatorRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Quantity = quantity,
                Product_Id = productId
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var props = _channel.CreateBasicProperties();
            props.CorrelationId = message.CorrelationId;
            props.ReplyTo = "product.validator.response";

            _channel.BasicPublish(exchange: "", routingKey: "product.validator.request", basicProperties: props, body: body);

        }
    }
}