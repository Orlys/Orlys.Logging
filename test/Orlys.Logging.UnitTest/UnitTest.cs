
namespace Orlys.Logging.UnitTest
{
    using System;
    using Xunit;
    using Orlys.Logging;
    using Orlys.Logging.Internal;
    using System.Diagnostics;

    public class UnitTest
    {
        [Fact]
        public void Test1()
        {
            var log = new InternalLogService();
            var k = log + log + log;

            Log.Setup(log + log);

            Log.Sink(LogLevel.Critical | LogLevel.Error, "xxx", 1258);
        }
    }
}
