using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.BLL.DTOs;
using Cart.BLL.Interfaces.Handler;
using Cart.BLL.Interfaces.ProductHandler;
using Cart.BLL.Messaging.Interfaces.Payments;
using Cart.BLL.Messaging.Messages.Payments;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.BLL.Messaging.Services.Payments
{
    public class ProductRequestSubscriber : IProductRequestSubscriber
    {

        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public ProductRequestSubscriber(IServiceScopeFactory serviceScopeFactory)
        {
            var _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "cart.product.list.request", durable: true, autoDelete: false, arguments: null, exclusive: false);
            _channel.QueueDeclare(queue: "cart.product.list.response", durable: true, autoDelete: false, arguments: null, exclusive: false);

            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Consume()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("Ok, i'm listening..");
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _cartHandlerService = scope.ServiceProvider.GetRequiredService<ICartHandlerService>();
                        var _productHandler = scope.ServiceProvider.GetRequiredService<IProductHandlerService>();


                        var body = ea.Body.ToArray();
                        var requestBody = Encoding.UTF8.GetString(body);
                        var requestMessage = JsonConvert.DeserializeObject<CartProductListRequest>(requestBody);
                        string replyTo = ea.BasicProperties.ReplyTo;


                        long cartId = await _cartHandlerService.GetCartIdByUserIdAsync(requestMessage.User_Id);
                        var productList = await _cartHandlerService.GetItemsFromCartByCartIdAsync(cartId);

                        var allProducts = await _productHandler.GetAllProductsAsync();


                        var products = productList.Select(product =>
                        {
                            var selectedroduct = allProducts.FirstOrDefault(p => p.Product_Id == product.Product_Id);

                            if (product != null)
                            {
                                return new ProductList
                                {
                                    Item_Id = product.Item_Id,
                                    Cart_Id = product.Cart_Id,
                                    Product_Id = product.Product_Id,
                                    ProductName = selectedroduct.ProductName,
                                    Quantity = product.Quantity,
                                    Price = product.Price
                                };
                            }
                            return null;
                        }).Where(p => p != null).ToList();

                        var response = new CartProductListResponse
                        {
                            CorrelationId = requestMessage.CorrelationId,
                            Products = products.ToList()
                        };

                        await Publish(response, replyTo);
                    }
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                }
            };
            _channel.BasicConsume(queue: "cart.product.list.request", autoAck: false, consumer: consumer);
        }

        public Task Publish(CartProductListResponse response, string replyTo)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = response.CorrelationId;

            _channel.BasicPublish(exchange: "", routingKey: replyTo, body: body, basicProperties: properties);
            Console.WriteLine("Message sent back");
            return Task.CompletedTask;
        }

    }
}