namespace MonitoringApp.Services.SignalRConsumerService
{
    public interface ISignalRConsumerService
    {
        public Task Connect(string signalRUrl);
        public Task Disconnect();
    }
}
