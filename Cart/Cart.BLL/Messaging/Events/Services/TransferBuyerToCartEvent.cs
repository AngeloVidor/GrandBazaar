using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cart.BLL.Messaging.Events.Interfaces;
using Cart.BLL.Messaging.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.BLL.Messaging.Events.Services
{
    public class TransferBuyerToCartEvent : ITransferBuyerToCartEvent
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<long>> _pendingResponses = new ConcurrentDictionary<string, TaskCompletionSource<long>>();


        public TransferBuyerToCartEvent()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "cart.to.buyer.request", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "cart.to.buyer.response", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var result = JsonConvert.DeserializeObject<CartToBuyerResponse>(message);
                    if (result == null)
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, requeue: false);
                        return;
                    }

                    Console.WriteLine("Trying to access dictionary...");
                    if (_pendingResponses.TryGetValue(result.CorrelationId, out var tcs))
                    {
                        tcs.TrySetResult(result.Buyer_Id);
                        _pendingResponses.TryRemove(result.CorrelationId, out _);
                    }
                    else
                    {
                        Console.WriteLine($"CorrelationId not found: {result.CorrelationId}");
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error proccessing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                }
            };

            _channel.BasicConsume(queue: "cart.to.buyer.response", autoAck: false, consumer: consumer);
        }

        public async Task<long> Publish(long userId)
        {
            System.Console.WriteLine("Publishing...");
            var message = new CartToBuyerRequest
            {
                User_Id = userId,
                CorrelationId = Guid.NewGuid().ToString()
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            Console.WriteLine("CorrelationId sent: " + properties.CorrelationId);

            properties.ReplyTo = "cart.to.buyer.response";

            var tcs = new TaskCompletionSource<long>();
            _pendingResponses.TryAdd(message.CorrelationId, tcs);

            Console.WriteLine("Published!");
            _channel.BasicPublish(exchange: "", routingKey: "cart.to.buyer.request", basicProperties: properties, body: body);

            return await tcs.Task;
        }

        public async Task<long> GetBuyerIdAsync(long userId)
        {
            return await Publish(userId);
        }
    }
}