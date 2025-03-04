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
using RabbitMQ.Client.Events;

namespace Orders.BLL.Messaging.Products.Services
{
    public class ProductsRequestPublisher : IProductsRequestPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ConcurrentDictionary<string, TaskCompletionSource<ProductResponse>> _requests = new ConcurrentDictionary<string, TaskCompletionSource<ProductResponse>>();

        public ProductsRequestPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "orders.products.request", durable: true, exclusive: false, arguments: null, autoDelete: false);
            _channel.QueueDeclare(queue: "orders.products.response", durable: true, exclusive: false, arguments: null, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    Console.WriteLine("I'm listening...");
                    var body = ea.Body.ToArray();
                    var bodyMessage = Encoding.UTF8.GetString(body);
                    var message = JsonConvert.DeserializeObject<ProductResponse>(bodyMessage);

                    if (_requests.TryGetValue(message.CorrelationId, out var tcs))
                    {
                        tcs.TrySetResult(message);
                        _requests.TryRemove(message.CorrelationId, out _);
                    }
                    else
                    {
                        Console.WriteLine($"CorrelationId not found: {message.CorrelationId}");
                    }
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error proccessing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                }
            };
            _channel.BasicConsume(queue: "orders.products.response", autoAck: false, consumer: consumer);

        }


        public async Task<ProductResponse> Publish(long costumerId)
        {
            var message = new ProductRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Costumer_Id = costumerId,
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            properties.ReplyTo = "orders.products.response";

            var tcs = new TaskCompletionSource<ProductResponse>();
            _requests.TryAdd(message.CorrelationId, tcs);

            _channel.BasicPublish(exchange: "", routingKey: "orders.products.request", basicProperties: properties, body: body);
            return await tcs.Task;
        }
    }
}