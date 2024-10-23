using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MonitoringApp.Utilities.ReusableRabbitMQClientLibrary
{
    public class RabbitMQMessageConsumer : IMessageConsumer
    {
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQMessageConsumer(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Consume(string queueName, Action<byte[]> handleMessage)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            handleMessage(body);
            Console.WriteLine(" [x] Received {0}", Encoding.UTF8.GetString(body));
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
    }    

}