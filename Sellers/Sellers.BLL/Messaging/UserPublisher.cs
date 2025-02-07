using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sellers.BLL.Messaging.Events;

namespace Sellers.BLL.Messaging
{
    public class UserPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly Dictionary<string, TaskCompletionSource<UserValidationResponse>> _pendingResponses = new();

        public UserPublisher()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var correlationId = ea.BasicProperties.CorrelationId;
                var body = ea.Body.ToArray();

                var responseMessage = Encoding.UTF8.GetString(body);

                var response = JsonConvert.DeserializeObject<UserValidationResponse>(responseMessage);

                if (_pendingResponses.TryGetValue(correlationId, out var tcs))
                {
                    tcs.SetResult(response);
                    _pendingResponses.Remove(correlationId);
                }
                _channel.BasicConsume(queue: "user.validation.response", autoAck: true, consumer: consumer);
            };
        }

        public async Task<UserValidationResponse> Publish(UserValidationRequest message)
        {
            var tcs = new TaskCompletionSource<UserValidationResponse>();
            _pendingResponses[message.CorrelationId] = tcs;

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = message.CorrelationId;
            properties.ReplyTo = "user.validation.response";

            _channel.BasicPublish(exchange: "", routingKey: "user.validation.request", basicProperties: properties, body: body);
            Console.WriteLine("Message sent!");
            
            return await tcs.Task;
        }

        public async Task<(long UserId, string CorrelationId)> ValidateUserIdAsync(long user_id)
        {
            var message = new UserValidationRequest
            {
                User_Id = user_id,
                CorrelationId = Guid.NewGuid().ToString()
            };

            var response = await Publish(message);
            return (response.User_Id, response.CorrelationId);
        }
    }
}