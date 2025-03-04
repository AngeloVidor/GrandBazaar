using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Products.BLL.Interfaces.Provider;
using Products.BLL.Messaging.Interfaces.ProductHandler;
using Products.BLL.Messaging.Messages.ProductHandler;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Products.BLL.Messaging.Services.ProductHandler
{
    public class ProductHandlerSub : IProductHandlerSub
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProductHandlerSub(IServiceScopeFactory serviceScopeFactory)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "cart.product.handler.request", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "cart.product.handler.response", durable: true, exclusive: false, autoDelete: false, arguments: null);

            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task Consume()
        {
            Console.WriteLine("Ayo, i'm listening...");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _productProvider = scope.ServiceProvider.GetRequiredService<IProductProviderService>();

                        var body = ea.Body.ToArray();
                        var bodyMessage = Encoding.UTF8.GetString(body);
                        var requestMessage = JsonConvert.DeserializeObject<ProductRequest>(bodyMessage);
                        string replyTo = ea.BasicProperties.ReplyTo;

                        var products = await _productProvider.GetAllProductsForDisplayAsync();

                        var productsMapper = products.Select(p => new ProductProvider
                        {
                            Product_Id = p.Product_Id,
                            ProductName = p.ProductName,
                            Price = p.Price,
                            StockQuantity = p.StockQuantity,
                            Seller_Id = p.Seller_Id
                        });

                        var response = new ProductResponse
                        {
                            CorrelationId = requestMessage.CorrelationId,
                            Products = productsMapper.ToList()
                        };

                        await Publish(response, replyTo);
                        _channel.BasicAck(ea.DeliveryTag, multiple: false);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException);
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                    return;
                }
            };
            _channel.BasicConsume(queue: "cart.product.handler.request", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        public Task Publish(ProductResponse response, string replyTo)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = response.CorrelationId;
            properties.ReplyTo = replyTo;

            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);
            Console.WriteLine("Message sent back!");

            return Task.CompletedTask;
        }
    }
}