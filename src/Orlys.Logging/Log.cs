namespace Orlys.Logging
{
    using Orlys.Diagnostics;
    using Orlys.Logging;
    using System;
    using System.Diagnostics;
    using System.Reflection;

    public static class Log
    { 
        private static LogServiceAggregator s_services;

        private static bool CheckIsMainStaticConstructor()
        {
            var runningAssembly = Assembly.GetEntryAssembly().EntryPoint;
            var perplexed = Perplexed.Locate(2);
            return 
                perplexed.In.Constructor &&
                perplexed.In.Is.Value == Is.Static &&
                perplexed.Declaring == runningAssembly.DeclaringType;
        }

        /// <summary>
        /// Calls this method in entry point's static constructor to setup all of the log services.
        /// </summary>
        /// <param name="services">The services what you want to use.</param> 
        [DebuggerNonUserCode]
        public static void SetupLogService(LogServiceAggregator services = null)
        {
            if(CheckIsMainStaticConstructor())
            {
                s_services = services ?? Default.LogService;
            }
            else
                throw new LogServiceExeception("Incorrect callsite.", "The method 'SetupLogService' must be called in entry point's static constructor.");
        }

        [DebuggerNonUserCode]
        public static void Sink(LogLevel level, string format, params object[] args)
        {
            if (CheckIsMainStaticConstructor())
            {
                throw new LogServiceExeception("Incorrect callsite.", "The method 'Sink' should not called in entry point's static constructor.");
            }

            if (s_services == null)
                throw new LogServiceExeception("Uninitialized log service.", "The method 'SetupLogService' not called yet.");

            var preplexed = Perplexed.Locate(1);
            foreach (var service in s_services.Lookup())
            { 
                if (service.IsLevelMatched(level, out var list))
                {
                    service.OnSink(list, preplexed, format, args);
                } 
            }
        }
    }

    public sealed class LogServiceExeception : Exception
    {
        internal LogServiceExeception(string summary, string detail) : base(summary + "\r\n" + detail)
        {
            this.Summary = summary;
            this.Detail = detail;
        }

        public string Summary { get; }
        public string Detail { get; }
    }


}