using Microsoft.AspNetCore.SignalR.Client;
namespace MonitoringApp.Services.SignalRConsumerService
{
    public class SignalRConsumerService : ISignalRConsumerService
    {
        private HubConnection _connection;
        private string _signalRUrl;
        // Use Environment variable
        public SignalRConsumerService()
        {
            _signalRUrl = Environment.GetEnvironmentVariable("SIGNALR_URL") ?? "http://localhost:5000";
        }

        public async Task Connect(string signalRUrl)
        {
            _signalRUrl = signalRUrl;
            _connection = new HubConnectionBuilder()
                .WithUrl(signalRUrl)
                .Build();

            _connection.On<string>("ReceiveMessage", (message) =>
            {
                Console.WriteLine("Received event: " + message);
            });

            await _connection.StartAsync();
        }
        public async Task Disconnect()
        {
            await _connection.StopAsync();  
            Console.WriteLine("Disconnected from SignalR hub.");
        }
    }
}
