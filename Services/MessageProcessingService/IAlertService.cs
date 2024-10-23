namespace MonitoringApp.Services.MessageProcessingService
{
    public interface IAlertService
    {
        void SendAlert(string message);
    }
}