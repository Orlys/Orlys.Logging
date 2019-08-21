
namespace Orlys.Logging.Internal
{
    using Orlys.Diagnostics;
    using Orlys.Logging; 
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class InternalLogService : LogServiceBase
    {
        protected override LogLevel Level => LogLevel.Debug;

        protected internal override void OnSink(IReadOnlyList<LogLevel> levels, ISignatureInfo signature, string message)
        { 
            foreach (var level in levels)
            {
                Console.WriteLine($"[{signature},{level}] {message}");
            }
        }
    }
} 