using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Auth.BLL.Interfaces.Management;
using Auth.BLL.Messaging.Events;
using Auth.DAL.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.BLL.Messaging
{
    public class Sub : IHostedService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public Sub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "user.validation.request", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task Consume()
        {
            Console.WriteLine("Start listening....");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var _userManagement = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
                    try
                    {
                        var request = System.Text.Json.JsonSerializer.Deserialize<UserValidationRequest>(message);
                        long userId = _userManagement.GetUserIdFromContext();
                        if (userId <= 0)
                        {
                            throw new KeyNotFoundException("Not found");
                        }

                        Console.WriteLine("Logado");

                        var correlationId = ea.BasicProperties.CorrelationId;
                        var replyTo = ea.BasicProperties.ReplyTo;
                        await Publish(userId, replyTo, correlationId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.Message}");
                    }
                }
            };

            _channel.BasicConsume(queue: "user.validation.request", autoAck: true, consumer: consumer);
        }

        public async Task Publish(long userId, string replyTo, string correlationId)
        {
            var response = new UserValidationResponse
            {
                User_Id = userId,
                CorrelationId = correlationId
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;

            _channel.BasicPublish(exchange: "", routingKey: "user.validation.response", basicProperties: properties, body: body);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }
    }
}