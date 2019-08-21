namespace Orlys.Logging
{
    using Orlys.Logging.Internal;
    using System.Collections.Generic;

    public static class Default
    {
        public static IEqualityComparer<LogServiceBase> EqualityComparer { get; } = new InternalLogServiceEqualityComparer();
        public static LogServiceBase LogService { get; } = new InternalLogService();

        public const LogLevel AllLevel = LogLevel.Info | LogLevel.Debug | LogLevel.Trace | LogLevel.Warning | LogLevel.Error | LogLevel.Critical;
    }
}