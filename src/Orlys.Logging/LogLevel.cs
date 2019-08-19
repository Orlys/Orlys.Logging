
namespace Orlys.Logging
{
    using System;

    [Flags]
    public enum LogLevel
    {
        Info = 0x10000,
        Debug = 0x20000,
        Trace = 0x40000,
        Warning = 0x80000,
        Error = 0x100000,
        Critical = 0x200000, 
    }
}