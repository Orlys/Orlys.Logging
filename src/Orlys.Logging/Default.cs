namespace Orlys.Logging
{
    using Orlys.Logging.Internal;
    using System.Collections.Generic;

    public static class Default
    {
        public static IEqualityComparer<LogServiceBase> EqualityComparer { get; } = new InternalLogServiceEqualityComparer();
        public static LogServiceBase LogService { get; } = new InternalLogService(); 
    }
}