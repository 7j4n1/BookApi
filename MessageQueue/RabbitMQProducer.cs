
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace BookApi {

    public class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory(){
                // HostName = "u9v0uc.stackhero-network.com",
                // Port = 5671,
                // UserName = "admin",
                // Password = "xTFlgoEdfzfeQT1cn7zRWvLYDJeESbHk",
                Uri = new Uri(ConfigurationManager.AppSetting["RabbitMqUrl"])
            };

            var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue:"books", 
                durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: "books", basicProperties: null, body: body);
            }
        }
    }

}