

namespace Orlys.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public interface ISlimSignatureInfo
    {
        string Name { get; }
        Type Return { get; }
        IReadOnlyList<(Type type, ParameterInfo parameter)> Parameters { get; }
        MethodBase Body { get; }
        Type Declaring { get; }
        StackFrame StackFrame { get; }
    }

    public interface ISignatureInfo : ISlimSignatureInfo, ILocatable
    { 
    }
}