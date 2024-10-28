using System.Configuration;
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

        // Constructor with configuration loaded from AppSettings
         public MessageProcessingService(IAlertService alertService, string databaseName)
        {
            _rabbitMqHost = ConfigurationManager.AppSettings["RABBITMQ_HOST"] ?? "localhost";
            _mongoDbHost = ConfigurationManager.AppSettings["MONGODB_HOST"] ?? "localhost";
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));

            var client = new MongoClient(_mongoDbHost);
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
