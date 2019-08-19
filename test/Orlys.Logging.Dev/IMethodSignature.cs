

namespace Orlys.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public interface IMethodSignature
    {
        string Name { get; }
        Type Return { get; }
        IReadOnlyList<(Type type, ParameterInfo parameter)> Parameters { get; }
        MethodBase Body { get; }
        Type DeclaringType { get; }
        StackFrame StackFrame { get; }
    }
}