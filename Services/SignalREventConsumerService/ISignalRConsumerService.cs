using Microsoft.AspNetCore.SignalR.Client;
namespace MonitoringApp.Services.SignalRConsumerService
{
    public interface ISignalRConsumerService
    {
        public Task Connect(string signalRUrl);
    }
}
