namespace Orlys.Logging.Dev
{
    using Orlys.Diagnostics; 
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    internal class Program
    {
        static Program()
        {
            Log.SetupLogService();
        }
        private static void Main(string[] args)
        {
            Log.Sink(LogLevel.Debug | LogLevel.Critical, "{0}: {1}", 12, 34);
        }
    }
}
