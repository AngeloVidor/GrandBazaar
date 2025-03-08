using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Products.BLL.DTOs;
using Products.BLL.Messaging.Interfaces.StripeProduct;
using Products.BLL.Messaging.Messages.StripeProduct;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Products.BLL.Messaging.Services.StripeProduct
{
    public class StripeProductPublisher : IStripeProductPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ConcurrentDictionary<string, TaskCompletionSource<StripeProductResponse>> _pendingTasks = new ConcurrentDictionary<string, TaskCompletionSource<StripeProductResponse>>();

        public StripeProductPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "stripe.product.request", exclusive: false, durable: true, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "stripe.product.response", exclusive: false, durable: true, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var messageBody = Encoding.UTF8.GetString(body);
                    var response = JsonConvert.DeserializeObject<StripeProductResponse>(messageBody);

                    if (_pendingTasks.TryGetValue(response.CorrelationId, out var tcs))
                    {
                        tcs.TrySetResult(response);
                        _pendingTasks.TryRemove(response.CorrelationId, out _);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error proccessing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "stripe.product.response", autoAck: false, consumer: consumer);


        }
        public async Task<StripeProductResponse> Publish(ProductDto product)
        {
            Console.WriteLine("Publishing....");
            var message = new StripeProductRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ProductName = product.ProductName,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                UnitAmount = product.Price,
                Product_Id = product.Product_Id
            };


            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = _channel.CreateBasicProperties();

            properties.CorrelationId = message.CorrelationId;
            var replyTo = properties.ReplyTo = "stripe.product.response";

            _channel.BasicPublish(exchange: "", routingKey: "stripe.product.request", basicProperties: properties, body: body);

            var tcs = new TaskCompletionSource<StripeProductResponse>();
            _pendingTasks.TryAdd(message.CorrelationId, tcs);
            return await tcs.Task;
        }

        public async Task<bool> CreateStripeProductAsync(ProductDto product)
        {
            var response = await Publish(product);
            return response.ProductCreated;
        }
    }
}