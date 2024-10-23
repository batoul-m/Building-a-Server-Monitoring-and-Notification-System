using RabbitMQ.Client;
using System;
using System.Text;
namespace MonitoringApp.Utilities.ReusableRabbitMQClientLibrary
{
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQMessagePublisher(string hostName, string userName, string password)
        {
            var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish(string exchange, string routingKey, byte[] messageBody)
        {
            _channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: messageBody);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, Encoding.UTF8.GetString(messageBody));
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
