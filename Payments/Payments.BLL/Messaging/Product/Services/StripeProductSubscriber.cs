using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Payments.BLL.Interfaces;
using Payments.BLL.Messaging.Product.Interfaces;
using Payments.BLL.Messaging.Product.Messages;
using Payments.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Payments.BLL.Messaging.Product.Services
{
    public class StripeProductSubscriber : IStripeProductSubscriber
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StripeProductSubscriber(IServiceScopeFactory serviceScopeFactory)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _serviceScopeFactory = serviceScopeFactory;

            _channel.QueueDeclare(queue: "stripe.product.request", exclusive: false, durable: true, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "stripe.product.response", exclusive: false, durable: true, autoDelete: false, arguments: null);
        }

        public Task Consume()
        {
            Console.WriteLine("Yo... listening!!");
            var consumer = new EventingBasicConsumer(_channel);
            Console.WriteLine("Consumer Created!!");

            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("Consumer received");
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _productService = scope.ServiceProvider.GetRequiredService<IStripeProductService>();

                        var body = ea.Body.ToArray();
                        var requestBody = Encoding.UTF8.GetString(body);
                        var request = JsonConvert.DeserializeObject<StripeProductRequest>(requestBody);
                        var replyTo = ea.BasicProperties.ReplyTo;

                        var product = new AppProduct
                        {
                            Product_Id = request.Product_Id,
                            ProductName = request.ProductName,
                            Description = request.Description,
                            UnitAmount = request.UnitAmount,
                            StockQuantity = request.StockQuantity
                        };
                        bool createdProduct = await _productService.CreateProductAsync(product);
                        if (createdProduct)
                        {
                            var response = new StripeProductResponse
                            {
                                CorrelationId = request.CorrelationId,
                                ProductCreated = createdProduct
                            };

                            await Publish(response, replyTo);
                        }

                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);

                }
            };
            _channel.BasicConsume(queue: "stripe.product.request", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        public Task Publish(StripeProductResponse response, string replyTo)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();

            properties.CorrelationId = response.CorrelationId;
            properties.ReplyTo = replyTo;

            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);
            return Task.CompletedTask;
        }
    }
}