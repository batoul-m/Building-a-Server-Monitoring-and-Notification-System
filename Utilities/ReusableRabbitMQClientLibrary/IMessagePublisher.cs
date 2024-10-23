namespace MonitoringApp.Utilities.ReusableRabbitMQClientLibrary
{
    public interface IMessagePublisher
    {
        void Publish(string exchange, string routingKey, byte[] messageBody);
    }

}