
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BookApi
{
    public class MessageQueueConsumer(IMessageStorageService messageStorage) : BackgroundService
    {
        private readonly IMessageStorageService _messageStorage = messageStorage;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory(){
                Uri = new Uri(ConfigurationManager.AppSetting["RabbitMqUrl"])
            };

            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "new_book", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[+] Received {message}");
                    // store the message content
                    await _messageStorage.StoreMessageAsync(message);
                };

                channel.BasicConsume("new_book", true, consumer);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }

        }
    }
}
