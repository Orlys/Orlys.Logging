
namespace Orlys.Logging.Dev
{
    using Orlys.Diagnostics;
    using Orlys.Logging;
    using System;
    using System.Runtime.CompilerServices;

    class Program
    { 
        static void Main(string[] args)
        {
            // Log.Sink(LogLevel.Error | LogLevel.Critical , "{0}: {1}", 12, 33);

            var x = new Program();

            var k = x.arg1;
            x.arg1 = null;

            var v = x[2];
        }

        public string arg1
        {
            get
            {
                return null;
            }
            set
            {
                var x = Perplexed.Locate().In;
                Console.WriteLine(x.Is);
                Console.WriteLine(x.Set);
            }
        }
        public string this[int i]
        {
            get
            {
                return null;
            }
            set
            {
                var x = Perplexed.Locate().In;
                Console.WriteLine(x.Set);
                Console.WriteLine(x.Is);
            }
        }

    }
}

namespace Orlys.Diagnostics
{
    using Ryuko.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    [Flags]
    public enum Is
    {
        Static = 0x10,
        Async = 0x20,
        Indexer = 0x40,

    }
    public class In
    {
        private const BindingFlags BindingFilter = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        private static readonly Type voidType = typeof(void);
        private const string
            add_prefix = "add_",
            remove_prefix = "remove_",
            get_prefix = "get_",
            set_prefix = "set_",
            indexer_name = "Item",
            implicit_full = "op_Implicit",
            explicit_full = "op_Explicit",
            right_shift_full = "op_RightShift",
            left_shift_full = "op_LeftShift";


        public Is? Is { get; private set; }

        public In(IMethodSignature m, bool isAsync)
        {
            var @is = (Is)0;
            if (isAsync) @is |= Diagnostics.Is.Async;
            if (m.Body.IsStatic) @is |= Diagnostics.Is.Static;
            if (this.Constructor = ConstructorCore(m)) goto InitCompleted;
            if (!(m.Body is MethodInfo)) goto InitCompleted;
            if (this.Deconstructor = DeconstructorCore(m)) goto InitCompleted;
            if (this.Finalize = FinalizeCore(m)) goto InitCompleted;
            if (this.Lambda = LambdaCore(m)) goto InitCompleted;
            if (this.Dispose = DisposeCore(m)) goto InitCompleted;
            if (this.Get = GetCore(m, ref @is)) goto InitCompleted;
            if (this.Set = SetCore(m, ref @is)) goto InitCompleted;
            if (this.Implicit = OperationCastCore(m, implicit_full)) goto InitCompleted;
            if (this.Explicit = OperationCastCore(m, explicit_full)) goto InitCompleted;
            if (this.Add = EventCore(m, add_prefix)) goto InitCompleted;
            if (this.Remove = EventCore(m, remove_prefix)) goto InitCompleted;
            if (this.LeftShift = OperationShiftCore(m, left_shift_full)) goto InitCompleted;
            if (this.RightShift = OperationShiftCore(m, right_shift_full)) goto InitCompleted;

            #region Operator

            #endregion

            InitCompleted:
            if (@is != 0) this.Is = @is;
        }
        private static bool OperationShiftCore(IMethodSignature m, string fullname)
        {
            return
                m.Body.IsStatic &&
                m.Parameters.Count == 2 &&
                m.Parameters[0].type == m.DeclaringType &&
                m.Parameters[1].type == typeof(int) &&
                string.Equals(m.Name, fullname);
        }
        private static bool OperationCastCore(IMethodSignature m, string fullname)
        {
            var nameEquals = string.Equals(fullname, m.Name);
            var isStatic = m.Body.IsStatic;
            var parameterMatched = m.Parameters.Count == 1;
            var hasDeclaringType = m.Parameters[0].type == m.DeclaringType || m.Return == m.DeclaringType;
            return nameEquals && isStatic && parameterMatched && hasDeclaringType;
        }
        private static bool EventCore(IMethodSignature m, string prefix)
        {
            var prefixMatch = m.Name.AsSpan(0, prefix.Length).SequenceEqual(prefix);
            var name = m.Name.AsSpan(prefix.Length).ToString();
            var hasEvent = m.DeclaringType.GetEvent(name, BindingFilter) is EventInfo _;
            return prefixMatch && hasEvent;
        }
        private static bool DisposeCore(IMethodSignature m)
        {
            var isDisposable = typeof(IDisposable).IsAssignableFrom(m.Body.DeclaringType);
            var nameEquals = string.Equals(nameof(IDisposable.Dispose), m.Name);
            return isDisposable && nameEquals;
        }
        private static bool LambdaCore(IMethodSignature m)
        {
            var compilerGeneratedLabel = Attribute.IsDefined(m.DeclaringType, typeof(CompilerGeneratedAttribute));
            var serializableLabel = Attribute.IsDefined(m.DeclaringType, typeof(SerializableAttribute));
            var nestPrivateCondition = m.DeclaringType.IsNestedPrivate;
            var nameCondition = m.DeclaringType.Name.StartsWith("<>");
            return compilerGeneratedLabel && serializableLabel && nestPrivateCondition && nameCondition;
        }
        private static bool FinalizeCore(IMethodSignature m)
        {
            var nameEquals = string.Equals(nameof(Finalize), m.Name);
            var returnTypeMatched = m.Return == voidType;
            var parametersMatched = m.Parameters.Count == 0;
            return !m.Body.IsStatic && nameEquals && returnTypeMatched && parametersMatched;
        }
        private static bool ConstructorCore(IMethodSignature m)
        {
            return m.Body is ConstructorInfo;
        }
        private static bool DeconstructorCore(IMethodSignature m)

        {
            var nameEquals = string.Equals("Deconstruct", m.Name);
            var paramsLenMatched = m.Parameters.Count >= 2; // ValueTuple just supported the generic type arguments greater than 2.
            var allParamsAreOut = m.Parameters.All(p => p.parameter.IsOut);
            return !m.Body.IsStatic && nameEquals && paramsLenMatched && allParamsAreOut;
        }
        private static bool GetCore(IMethodSignature m, ref Is @is)
        {
            var prefixMatch = m.Name.AsSpan(0, get_prefix.Length).SequenceEqual(get_prefix);
            var name = m.Name.AsSpan(get_prefix.Length).ToString();
            if (prefixMatch && m.DeclaringType.GetProperty(name, BindingFilter) is PropertyInfo prop)
            {
                if (string.Equals(name, indexer_name) &&
                    m.Parameters.Count > 0 &&
                    m.Return != voidType &&
                    prop.GetGetMethod(true) is MethodInfo getter &&
                    ReferenceEquals(m.Body as MethodInfo, getter))
                {

                    @is |= Diagnostics.Is.Indexer;
                    return true;
                }

                if (m.Parameters.Count == 0 && m.Return != voidType)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool SetCore(IMethodSignature m, ref Is @is)
        {
            var prefixMatch = m.Name.AsSpan(0, set_prefix.Length).SequenceEqual(set_prefix);
            var name = m.Name.AsSpan(set_prefix.Length).ToString();
            if (prefixMatch && m.DeclaringType.GetProperty(name, BindingFilter) is PropertyInfo prop)
            {
                if (string.Equals(name, indexer_name) &&
                    m.Parameters.Count > 1 &&
                    m.Return == voidType &&
                    prop.GetSetMethod(true) is MethodInfo setter &&
                    ReferenceEquals(m.Body as MethodInfo, setter))
                {
                    @is |= Diagnostics.Is.Indexer;
                    return true;
                }

                if (m.Parameters.Count == 1 && m.Return == voidType)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Constructor { get; }
        public bool Deconstructor { get; }
        public bool Lambda { get; }
        public bool Finalize { get; }
        public bool Dispose { get; }

        /// <summary>
        /// Indicates the <see langword="add"/> method.
        /// </summary>
        public bool Add { get; }
        /// <summary>
        /// Indicates the <see langword="remove"/> method.
        /// </summary>
        public bool Remove { get; }
        /// <summary>
        /// Indicates the <see langword="get"/> method.
        /// </summary>
        public bool Get { get; }
        /// <summary>
        /// Indicates the <see langword="set"/> method.
        /// </summary>
        public bool Set { get; }

        /// <summary>
        /// Indicates the <see langword="explicit operator"/> method.
        /// </summary>
        public bool Explicit { get; }
        /// <summary>
        /// Indicates the <see langword="implicit operator"/> method.
        /// </summary>
        public bool Implicit { get; }

        /// <summary>
        /// Indicates the <see langword="operator &lt;&lt;"/> method.
        /// </summary>
        public bool LeftShift { get; }
        /// <summary>
        /// Indicates the <see langword="operator &gt;&gt;"/> method.
        /// </summary>
        public bool RightShift { get; }
    }


    /*
    public readonly static In X = new In(nameof(X), m => 
    {
        if (m.Body is MethodInfo pm)
        {

            else if (pm.IsStatic &&
                param.Length == 2 &&
                param.Contains(pm.DeclaringType))
            {
                if (string.Equals(pm.Name, "op_Addition", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.Addition;
                }
                else if (string.Equals(pm.Name, "op_Subtraction", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.Subtraction;
                }
                else if (string.Equals(pm.Name, "op_Multiply", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.Multiply;
                }
                else if (string.Equals(pm.Name, "op_Division", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.Division;
                }
                else if (string.Equals(pm.Name, "op_Modulus", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.Modulus;
                }
                else if (string.Equals(pm.Name, "op_BitwiseOr", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.BitwiseOr;
                }
                else if (string.Equals(pm.Name, "op_BitwiseAnd", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.BitwiseAnd;
                }
                else if (string.Equals(pm.Name, "op_ExclusiveOr", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.ExclusiveOr;
                }
                else if (string.Equals(pm.Name, "op_Equality", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.Equality;
                }
                else if (string.Equals(pm.Name, "op_Inequality", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.Inequality;
                }
                else if (string.Equals(pm.Name, "op_GreaterThan", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.GreaterThan;
                }
                else if (string.Equals(pm.Name, "op_LessThan", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.LessThan;
                }
            }
            else if (pm.IsStatic &&
                param.Length == 1 &&
                param[0].Equals(pm.DeclaringType))
            {
                if (string.Equals(pm.Name, "op_True", StringComparison.OrdinalIgnoreCase) && pm.ReturnType == typeof(bool))
                {
                    this.Attributes |= MethodFlags.True;
                }
                else if (string.Equals(pm.Name, "op_False", StringComparison.OrdinalIgnoreCase) && pm.ReturnType == typeof(bool))
                {
                    this.Attributes |= MethodFlags.False;
                }
                else if (string.Equals(pm.Name, "op_OnesComplement", StringComparison.OrdinalIgnoreCase))
                {
                    this.Attributes |= MethodFlags.OnesComplement;
                }
                else if (string.Equals(pm.Name, "op_Increment", StringComparison.OrdinalIgnoreCase) && pm.ReturnType.IsAssignableFrom(pm.DeclaringType))
                {
                    this.Attributes |= MethodFlags.Increment;
                }
                else if (string.Equals(pm.Name, "op_Decrement", StringComparison.OrdinalIgnoreCase) && pm.ReturnType.IsAssignableFrom(pm.DeclaringType))
                {
                    this.Attributes |= MethodFlags.Decrement;
                }
            }


        }
    });
    */
    public sealed class Perplexed : IMethodSignature, ILocatable
    {
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


        private Perplexed(StackFrame frame, bool isAsync)
        {
            this.StackFrame = frame;
            this.Body = frame.GetMethod();

            this.Name = this.Body.Name;
            this.Return = (this.Body as MethodInfo)?.ReturnType ?? null;
            this.Parameters = new List<(Type type, ParameterInfo parameter)>();
            this.DeclaringType = this.Body.DeclaringType;

            foreach (var parameter in this.Body.GetParameters())
                ((List<(Type type, ParameterInfo parameter)>)this.Parameters).Add((parameter.ParameterType, parameter));

            this.In = new In(this, isAsync);
        }

        public MethodBase Body { get; }

        public string Name { get; }

        public IReadOnlyList<(Type type, ParameterInfo parameter)> Parameters { get; }

        public Type Return { get; }

        public Type DeclaringType { get; }

        public In In { get; }
    }


}