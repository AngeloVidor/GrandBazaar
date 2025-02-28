using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orders.BLL.Messaging.Costumer.Interfaces;
using Orders.BLL.Messaging.Costumer.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Orders.BLL.Messaging.Costumer.Services
{
    public class UserIdentificationPub : IUserIdentificationPub
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<long>> _pendingTasks = new ConcurrentDictionary<string, TaskCompletionSource<long>>();

        public UserIdentificationPub()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "buyer.to.order.request", durable: true, exclusive: false, arguments: null, autoDelete: false);
            _channel.QueueDeclare(queue: "buyer.to.order.response", durable: true, exclusive: false, arguments: null, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var response = JsonConvert.DeserializeObject<UserIdentificationResponse>(message);
                Console.WriteLine($"Response content {response.Costumer_Id}, {response.CorrelationId}");

                string correlationId = response.CorrelationId;

                if (_pendingTasks.TryGetValue(correlationId, out var tcs))
                {
                    tcs.TrySetResult(response.Costumer_Id);
                    _pendingTasks.TryRemove(response.CorrelationId, out _);
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "buyer.to.order.response", autoAck: false, consumer: consumer);
        }

        public Task<long> Publish(long userId)
        {
            var message = new UserIdentificationRequest
            {
                CorrelationId = Guid.NewGuid().ToString(),
                User_Id = userId
            };

            var tcs = new TaskCompletionSource<long>();
            _pendingTasks.TryAdd(message.CorrelationId, tcs);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            properties.ReplyTo = "buyer.to.order.response";

            _channel.BasicPublish(exchange: "", routingKey: "buyer.to.order.request", basicProperties: properties, body: body);

            Console.WriteLine($"Message sent: {message.CorrelationId}, {message.User_Id}");
            return tcs.Task;
        }

        public async Task<long> GetCostumerIdAsync(long userId)
        {
            return await Publish(userId);
        }
    }
}