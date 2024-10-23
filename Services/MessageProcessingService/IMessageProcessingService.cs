using MongoDB.Driver;
using MonitoringApp.Services.ServerStatisticsCollectionService;
namespace MonitoringApp.Services.MessageProcessingService
{
    public interface IMessageProcessingService
    {
        public void ProcessMessage(ServerStatistics statistics);
        public void DetectAnomalies(ServerStatistics current, ServerStatistics previous, double memoryThreshold, double cpuThreshold);
        public void DetectHighUsageAlert(ServerStatistics current, double memoryUsageThresholdPercentage, double cpuUsageThresholdPercentage);
    
    }
}
