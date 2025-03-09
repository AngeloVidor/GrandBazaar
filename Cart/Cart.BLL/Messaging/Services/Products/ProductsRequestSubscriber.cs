using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Azure;
using Cart.BLL.Interfaces.Handler;
using Cart.BLL.Interfaces.Management;
using Cart.BLL.Interfaces.ProductHandler;
using Cart.BLL.Messaging.Interfaces.Products;
using Cart.BLL.Messaging.Messages.Products;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.BLL.Messaging.Services.Products
{
    public class ProductsRequestSubscriber : IProductsRequestSubscriber
    {
        private readonly IConnection _connection;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IModel _channel;


        public ProductsRequestSubscriber(IServiceScopeFactory serviceScopeFactory)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _serviceScopeFactory = serviceScopeFactory;

            _channel.QueueDeclare(queue: "orders.products.request", durable: true, exclusive: false, arguments: null, autoDelete: false);
            _channel.QueueDeclare(queue: "orders.products.response", durable: true, exclusive: false, arguments: null, autoDelete: false);
        }

        public Task Consume()
        {
            Console.WriteLine("Listening...");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _cartHandler = scope.ServiceProvider.GetRequiredService<ICartHandlerService>();
                        var _productHandler = scope.ServiceProvider.GetRequiredService<IProductHandlerService>();


                        var body = ea.Body.ToArray();
                        var bodyMessage = Encoding.UTF8.GetString(body);
                        var message = JsonConvert.DeserializeObject<ProductRequest>(bodyMessage);
                        string replyTo = ea.BasicProperties.ReplyTo;

                        Console.WriteLine($"CostumerId: {message.Costumer_Id}");

                        long cartId = await _cartHandler.GetCartIdByCostumerIdAsync(message.Costumer_Id);
                        if (cartId <= 0)
                        {
                            Console.WriteLine("Invalid CartId");
                            return;
                        }

                        var items = await _cartHandler.GetItemsFromCartByCartIdAsync(cartId);
                        if (items == null)
                        {
                            Console.WriteLine("Any item found");
                            return;
                        }

                        var allProducts = await _productHandler.GetAllProductsAsync();

                        var requestedProducts = items.Select(item =>
                        {
                            var product = allProducts.FirstOrDefault(p => p.Product_Id == item.Product_Id);
                            if (product != null)
                            {
                                return new RequestedProducts
                                {
                                    Product_Id = item.Product_Id,
                                    ProductName = product.ProductName,
                                    Quantity = item.Quantity,
                                    Price = item.Price
                                };
                            }
                            return null;
                        }).Where(p => p != null).ToList();

                        if (!requestedProducts.Any())
                        {
                            Console.WriteLine("No products found for cart items");
                            return;
                        }

                        var response = new ProductResponse
                        {
                            CorrelationId = message.CorrelationId,
                            Costumer_Id = message.Costumer_Id,
                            Products = requestedProducts
                        };


                        await Publish(response, replyTo);

                        _channel.BasicAck(ea.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };
            _channel.BasicConsume(queue: "orders.products.request", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        public Task Publish(ProductResponse response, string replyTo)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();

            properties.CorrelationId = response.CorrelationId;
            properties.ReplyTo = replyTo; //response queue

            Console.WriteLine("Ready to pub:");
            Console.WriteLine(response.Costumer_Id);
            if (response.Products == null)
            {
                Console.WriteLine("Empty list");
            }
            else
            {
                Console.WriteLine("Products list:");
                foreach (var product in response.Products)
                {
                    Console.WriteLine($"Product: {product.ToString()}");
                }
            }


            _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);
            return Task.CompletedTask;
        }
    }
}