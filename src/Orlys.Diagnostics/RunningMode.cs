
namespace Orlys.Diagnostics
{
    using System.Reflection;
    using System.Linq;
    using Unsafe = System.Security.UnverifiableCodeAttribute;
    using Debuggable = System.Diagnostics.DebuggableAttribute;
    using System;

    public static class RunningMode
    {
        static RunningMode()
        {
            var exe = Assembly.GetEntryAssembly();
            IsDebug = exe.GetCustomAttribute<Debuggable>() is Debuggable debug &&
                                  debug.IsJITOptimizerDisabled &&
                                  debug.IsJITTrackingEnabled;
            IsUnsafe = exe.Modules.Any(m => m.GetCustomAttribute<Unsafe>() is Unsafe);
             
        }

        public static bool IsDebug { get; }

        public static bool IsUnsafe { get; } 
    }
}
