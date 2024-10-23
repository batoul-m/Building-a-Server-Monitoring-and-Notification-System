using MongoDB.Driver;
using MonitoringApp.Services.ServerStatisticsCollectionService;
namespace MonitoringApp.Services.MessageProcessingService
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly IMongoCollection<ServerStatistics> _statisticsCollection;
        private readonly string _rabbitMqHost;
        private readonly string _mongoDbHost;
        private readonly IAlertService _alertService;
        // Use Environment Variables
        public MessageProcessingService()
        {
            _rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
            _mongoDbHost = Environment.GetEnvironmentVariable("MONGODB_HOST") ?? "localhost";
        }

        public MessageProcessingService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _statisticsCollection = database.GetCollection<ServerStatistics>("server_statistics");
        }
        public void ProcessMessage(ServerStatistics statistics)
        {
            _statisticsCollection.InsertOne(statistics);
        }
        void IMessageProcessingService.DetectAnomalies(ServerStatistics current, ServerStatistics previous, double memoryThreshold, double cpuThreshold)
        {
            bool memoryAnomaly = current.MemoryUsage > (previous.MemoryUsage * (1 + memoryThreshold));
            bool cpuAnomaly = current.CpuUsage > (previous.CpuUsage * (1 + cpuThreshold));

            if (memoryAnomaly || cpuAnomaly)
            {
                string alertMessage = $"Anomaly detected! Memory: {current.MemoryUsage}, CPU: {current.CpuUsage}";
                _alertService.SendAlert(alertMessage);
            }
        }
        public void DetectHighUsageAlert(ServerStatistics current, double memoryUsageThresholdPercentage, double cpuUsageThresholdPercentage)
        {
            bool memoryHighUsageAlert = (current.MemoryUsage / (current.MemoryUsage + current.AvailableMemory)) > memoryUsageThresholdPercentage;
            bool cpuHighUsageAlert = (current.CpuUsage) > cpuUsageThresholdPercentage;
            if (memoryHighUsageAlert || cpuHighUsageAlert)
            {
                string alertMessage = $"High usage alert! Memory: {current.MemoryUsage}, CPU: {current.CpuUsage}";
                _alertService.SendAlert(alertMessage);
            }
        }
    }
}
