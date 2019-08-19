namespace Orlys.Logging
{
    using Orlys.Logging;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
      
    public sealed class LogServiceAggregator
    {
        private readonly HashSet<LogServiceBase> m_list;

        internal static LogServiceAggregator Create(LogServiceBase service, IEqualityComparer<LogServiceBase> comparer)
        {
            var aggregator = new LogServiceAggregator(comparer);
            aggregator.Append(service);
            return aggregator;
        }

        private LogServiceAggregator(IEqualityComparer<LogServiceBase> comparer)
        {
            this.m_list = new HashSet<LogServiceBase>(comparer);
        }

        private void Append(LogServiceBase service)
        {
            this.m_list.Add(service);
        } 
        
        public ISet<LogServiceBase> Lookup()
        {
            return m_list;
        }

        public static LogServiceAggregator operator +(LogServiceAggregator aggregator, LogServiceBase service)
        {
            aggregator.Append(service);
            return aggregator;
        }
         
    }
}