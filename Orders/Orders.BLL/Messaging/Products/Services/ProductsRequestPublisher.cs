using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orders.BLL.Messaging.Products.Interfaces;
using Orders.BLL.Messaging.Products.Messages;
using RabbitMQ.Client;

namespace Orders.BLL.Messaging.Products.Services
{
    public class ProductsRequestPublisher : IProductsRequestPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ProductsRequestPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "orders.products.request", durable: true, exclusive: false, arguments: null, autoDelete: false);

        }


        public async Task Publish(long costumerId)
        {
            var message = new ProductsRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Costumer_Id = costumerId,
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            properties.ReplyTo = "orders.products.response";

            _channel.BasicPublish(exchange: "", routingKey: "orders.products.request", basicProperties: properties, body: body);

        }
    }
}