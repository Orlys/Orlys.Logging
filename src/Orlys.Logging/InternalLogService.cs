
namespace Orlys.Logging.Internal
{
    using Orlys.Logging;
    using Ryuko.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class InternalLogService : LogServiceBase
    {
        protected override LogLevel Level => LogLevel.Debug;
        protected internal override void SinkCore(IReadOnlyList<LogLevel> levels, Perplexed perplexed,  string format, params object[] args)
        { 
            foreach (var level in levels)
            {
                Console.WriteLine($"[{perplexed},{level}] {string.Format(format, args)}");
            }
        }
    }
} 