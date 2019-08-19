namespace Orlys.Diagnostics
{

    using System;

    [Flags]
    public enum Is
    {
        Static = 0x10,
        Async = 0x20,
        Indexer = 0x40,
    }
}
