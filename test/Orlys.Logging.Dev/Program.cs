namespace Orlys.Logging.Dev
{
    using Orlys.Diagnostics; 
    using System;
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
            Log.Sink(LogLevel.Error | LogLevel.Critical , "{0}: {1}", 12, 33);
        }
    }
}
