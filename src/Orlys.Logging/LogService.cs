namespace Orlys.Logging
{
    using Orlys.Diagnostics; 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class LogServiceBase  
    {
        public virtual Guid TypeId { get; }
        
        public LogServiceBase()
        {
            this.TypeId = this.GetType().GUID; 
        }

        internal bool IsLevelMatched(LogLevel level, out IReadOnlyList<LogLevel> exploded)
        {
            exploded = new List<LogLevel>();
            foreach (LogLevel r in Enum.GetValues(typeof(LogLevel)))
            { 
                if (level.HasFlag(r)) 
                { 
                    ((List<LogLevel>)(exploded)).Add(r);
                }
            }
            
            return exploded.Count > 0;
        }

        protected abstract LogLevel Level { get; }
         
        #region Implicit Convention

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static implicit operator LogServiceAggregator(LogServiceBase service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return LogServiceAggregator.Create(service, Default.EqualityComparer);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LogServiceAggregator operator +(LogServiceBase first, LogServiceBase second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }
             
            return LogServiceAggregator.Create(first, Default.EqualityComparer) + second;
        }
        #endregion

        internal protected abstract void OnSink(IReadOnlyList<LogLevel> levels, ISignatureInfo signature, string format, params object[] args);
    }
}
   