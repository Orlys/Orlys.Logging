namespace Orlys.Logging
{
    using Orlys.Logging;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// Provides how <see cref="LogServiceBase"/> object aggregated to one instance for collect. 
    /// Using <see cref="LogServiceBase"/> <see langword="+"/> <see cref="LogServiceBase"/> to aggregate all services.
    /// </summary>
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
        
        internal ISet<LogServiceBase> Lookup()
        {
            return m_list;
        }

        public static LogServiceAggregator operator +(LogServiceAggregator aggregator, LogServiceBase service)
        {
            if (aggregator == null)
            {
                throw new ArgumentNullException(nameof(aggregator));
            }
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            aggregator.Append(service);
            return aggregator;
        }
         
    }

}