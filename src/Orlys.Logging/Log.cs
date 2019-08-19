namespace Orlys.Logging
{
    using Orlys.Logging;
    using System;

    public static class Log
    {
        static Log()
        {
            SetupLogService();
        } 
         
        private static LogServiceAggregator s_services;

        public static void SetupLogService(LogServiceAggregator services = null)
        {
            s_services = services ?? Default.LogService;
        }

        public static void Sink(LogLevel level, string format, params object[] args)
        {
            var preplexed = Ryuko.Diagnostics.Perplexed.Locate(1);
            foreach (var service in s_services.Lookup())
            { 
                if (service.IsLevelMatched(level, out var list))
                {
                    service.SinkCore(list, preplexed, format, args);
                } 
            }
        }
    }
}