namespace Orlys.Diagnostics
{ 

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    public sealed class Perplexed : ISignatureInfo
    {
        private Perplexed(StackFrame frame, bool isAsync)
        {
            this.StackFrame = frame;
            this.Body = frame.GetMethod();

            this.Name = this.Body.Name;
            this.Return = (this.Body as MethodInfo)?.ReturnType ?? null;
            this.Parameters = new List<(Type type, ParameterInfo parameter)>();
            this.Declaring = this.Body.DeclaringType;

            foreach (var parameter in this.Body.GetParameters())
                ((List<(Type type, ParameterInfo parameter)>)this.Parameters).Add((parameter.ParameterType, parameter));

            this.In = new In(this, isAsync);
        }

        public MethodBase Body { get; }
        public Type Declaring { get; }
        public In In { get; }
        public string Name { get; }
        public IReadOnlyList<(Type type, ParameterInfo parameter)> Parameters { get; }
        public Type Return { get; }
        public StackFrame StackFrame { get; }

        /// <summary>
        /// Gets the location of the call to this method.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Perplexed Locate()
        {
            return Locate(1);
        }

        /// <summary>
        /// Gets the location of the call to this method.
        /// </summary>
        /// <param name="offset">The offset of <see cref="System.Diagnostics.StackFrame"></see>, If you call the method within the <see langword="async"/>, they must be started from 2, otherwise 0. </param>
        /// <returns></returns>
        public static Perplexed Locate(sbyte offset)
        {
            var st = new StackTrace(RunningMode.IsDebug);

            var frame = default(StackFrame);
            if (st.FrameCount >= 4 + offset)
            {
                frame = st.GetFrame(3 + offset);
                if (Attribute.IsDefined(frame.GetMethod(), typeof(AsyncStateMachineAttribute)))
                    return new Perplexed(frame, true);
            }

            frame = st.GetFrame(1 + offset);
            return new Perplexed(frame, false);
        }
    }
}
