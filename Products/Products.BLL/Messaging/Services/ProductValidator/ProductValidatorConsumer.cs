using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Products.BLL.Interfaces.Provider;
using Products.BLL.Messaging.Events.Interfaces.BuyerIdentification;
using Products.BLL.Messaging.Events.Messages.BuyerIdentification;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Products.BLL.Messaging.Events.Services.BuyerIdentification
{
    public class ProductValidatorConsumer : IProductValidatorConsumer
    {
        private readonly IProductProviderService _productProviderService;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> _pendingRequests = new ConcurrentDictionary<string, TaskCompletionSource<bool>>();
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ProductValidatorConsumer(IProductProviderService productProviderService)
        {
            var factory = new ConnectionFactory { HostName = "Localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "product.validator.request", durable: true, autoDelete: false, exclusive: false, arguments: null);
            _channel.QueueDeclare(queue: "product.validator.response", durable: true, autoDelete: false, exclusive: false, arguments: null);

            _productProviderService = productProviderService;
        }

        public void Publish(ProductValidatorResponse response, string replyTo)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = response.CorrelationId;
            properties.ReplyTo = replyTo;

            try
            {
                _channel.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: properties, body: body);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while publishing message", ex);
            }
        }

        public void Consume()
        {
            Console.WriteLine("Start listening...");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var response = JsonConvert.DeserializeObject<ProductValidatorRequest>(message);
                    Console.WriteLine($"Recieved: {response.Product_Id}, {response.Quantity}");

                    var replyTo = ea.BasicProperties.ReplyTo;
                    var correlationId = ea.BasicProperties.CorrelationId;

                    bool isValid = await ValidateProductAsync(response);
                    if (!isValid)
                    {
                        var errorResponse = new ProductValidatorResponse
                        {
                            CorrelationId = correlationId,
                            IsAvailable = false,
                            ErrorMessage = "Product is not valid or not enough stock"
                        };
                        Publish(errorResponse, replyTo);
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        return;
                    }

                    var product = await _productProviderService.GetProductByIdAsync(response.Product_Id);
                    if (product == null)
                    {
                        var errorResponse = new ProductValidatorResponse
                        {
                            CorrelationId = correlationId,
                            IsAvailable = false,
                            ErrorMessage = "Product not found"
                        };
                        Publish(errorResponse, replyTo);
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        return;
                    }
                    var responseMessage = new ProductValidatorResponse
                    {
                        CorrelationId = correlationId,
                        Quantity = product?.StockQuantity ?? 0,
                        Product_Id = product?.Product_Id ?? 0,
                        IsAvailable = isValid,
                        Price = product?.Price ?? 0
                    };
                    Publish(responseMessage, replyTo);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    var errorResponse = new ProductValidatorResponse
                    {
                        CorrelationId = ea.BasicProperties.CorrelationId,
                        IsAvailable = false,
                        ErrorMessage = ex.Message
                    };

                    Publish(errorResponse, ea.BasicProperties.ReplyTo);
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);


                }
            };
            _channel.BasicConsume(queue: "product.validator.request", autoAck: false, consumer: consumer);
        }

        public async Task<bool> ValidateProductAsync(ProductValidatorRequest request)
        {
            var product = await _productProviderService.GetProductByIdAsync(request.Product_Id);
            if (product == null)
            {
                return false;
            }
            if (product.StockQuantity < request.Quantity)
            {
                return false;
            }
            return true;


        }
    }
}