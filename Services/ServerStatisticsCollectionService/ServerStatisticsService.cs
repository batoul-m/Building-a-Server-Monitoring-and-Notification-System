using System.Diagnostics;
using System.Configuration;
namespace MonitoringApp.Services.ServerStatisticsCollectionService
{
    public class ServerStatisticsService : IServerStatisticsService
    {
        private readonly int _samplingIntervalSeconds;
        private readonly string _serverIdentifier;

        // Constructor with configuration loaded from AppSettings
        public ServerStatisticsService()
        {
            _samplingIntervalSeconds = int.Parse(ConfigurationManager.AppSettings["SAMPLING_INTERVAL"] ?? "60");
            _serverIdentifier = ConfigurationManager.AppSettings["SERVER_IDENTIFIER"] ?? "default_server";
        }
        
        public ServerStatisticsService(int samplingIntervalSeconds, string serverIdentifier)
        {
            _samplingIntervalSeconds = samplingIntervalSeconds;
            _serverIdentifier = serverIdentifier;
        }

        public ServerStatistics CollectStatistics()
        {
            var memoryUsage = Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024);
            var cpuUsage = new PerformanceCounter("Processor","% Processot Time","_Total").NextValue();
            return new ServerStatistics
            {
                MemoryUsage = memoryUsage,
                AvailableMemory = new PerformanceCounter("Memory","Available MBytes").NextValue(),
                CpuUsage = cpuUsage,
                Timestamp = DateTime.Now
            };
        }
    }
}
