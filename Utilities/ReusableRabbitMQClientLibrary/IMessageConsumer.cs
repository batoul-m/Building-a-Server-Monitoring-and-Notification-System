namespace MonitoringApp.Utilities.ReusableRabbitMQClientLibrary
{
    public interface IMessageConsumer
    {
        void Consume(string queueName, Action<byte[]> handleMessage);
    }


}