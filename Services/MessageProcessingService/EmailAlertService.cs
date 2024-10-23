namespace MonitoringApp.Services.MessageProcessingService
{
    public class EmailAlertService : IAlertService
    {
        public void SendAlert(string message)
        {
            Console.WriteLine($"Email Alert: {message}");
        }
    }
}