namespace Orlys.Diagnostics
{

    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed partial class In
    {
        internal In(ISlimSignatureInfo m, bool isAsync)
        { 
            var @is = (Is)0;
            if (isAsync)
                @is |= Diagnostics.Is.Async;
            if (m.Body.IsStatic)
                @is |= Diagnostics.Is.Static;

            if (this.Constructor = ConstructorCore(m)) goto InitCompleted;
            if (!(m.Body is MethodInfo)) goto InitCompleted;

            if (this.Deconstructor = DeconstructorCore(m)) goto InitCompleted;
            if (this.Finalize = FinalizeCore(m)) goto InitCompleted;
            if (this.Lambda = LambdaCore(m)) goto InitCompleted;
            if (this.Dispose = DisposeCore(m)) goto InitCompleted;
            if (this.Get = GetCore(m, ref @is)) goto InitCompleted;
            if (this.Set = SetCore(m, ref @is)) goto InitCompleted;
            if (this.Add = EventCore(m, add_)) goto InitCompleted;
            if (this.Remove = EventCore(m, remove_)) goto InitCompleted;

            if (this.Implicit = OperationCastCore(m, implicit_full)) goto InitCompleted;
            if (this.Explicit = OperationCastCore(m, explicit_full)) goto InitCompleted;
            if (this.LeftShift = OperationShiftCore(m, left_shift_full)) goto InitCompleted;
            if (this.RightShift = OperationShiftCore(m, right_shift_full)) goto InitCompleted;

            // Binary operation
            if (this.Addition = OperationBinaryCore(m, addition_full)) goto InitCompleted;
            if (this.Subtraction = OperationBinaryCore(m, subtraction_full)) goto InitCompleted;
            if (this.Multiply = OperationBinaryCore(m, multiply_full)) goto InitCompleted;
            if (this.Division = OperationBinaryCore(m, division_full)) goto InitCompleted;
            if (this.Modulus = OperationBinaryCore(m, modulus_full)) goto InitCompleted;
            if (this.BitwiseOr = OperationBinaryCore(m, bitwiseOr_full)) goto InitCompleted;
            if (this.BitwiseAnd = OperationBinaryCore(m, bitwiseAnd_full)) goto InitCompleted;
            if (this.ExclusiveOr = OperationBinaryCore(m, exclusiveOr_full)) goto InitCompleted;
            if (this.Equality = OperationBinaryCore(m, equality_full)) goto InitCompleted;
            if (this.Inequality = OperationBinaryCore(m, inequality_full)) goto InitCompleted;
            if (this.GreaterThan = OperationBinaryCore(m, greaterThan_full)) goto InitCompleted;
            if (this.LessThan = OperationBinaryCore(m, lessThan_full)) goto InitCompleted;

            // Unary operation
            if (this.True = OperationUnaryCore(m, true_full)) goto InitCompleted;
            if (this.False = OperationUnaryCore(m, false_full)) goto InitCompleted;
            if (this.OnesComplement = OperationUnaryCore(m, onesComplement_full)) goto InitCompleted;
            if (this.Increment = OperationUnaryCore(m, increment_full)) goto InitCompleted;
            if (this.Decrement = OperationUnaryCore(m, decrement_full)) goto InitCompleted;

            InitCompleted:
            if (@is != 0) this.Is = @is;
        }

        public Is? Is { get; private set; }

        private static bool ConstructorCore(ISlimSignatureInfo m)
        {
            return 
                m.Body is ConstructorInfo;
        }

        private static bool DeconstructorCore(ISlimSignatureInfo m)
        {
            var nameEquals = string.Equals(deconstructor_full, m.Name);
            var paramsLenMatched = m.Parameters.Count >= 2; // ValueTuple just supported the generic type arguments greater than 2.
            var allParamsAreOut = m.Parameters.All(p => p.parameter.IsOut);
            return !m.Body.IsStatic && nameEquals && paramsLenMatched && allParamsAreOut;
        }

        private static bool DisposeCore(ISlimSignatureInfo m)
        {
            var isDisposable = typeof(IDisposable).IsAssignableFrom(m.Body.DeclaringType);
            var nameEquals = string.Equals(nameof(IDisposable.Dispose), m.Name);
            return isDisposable && nameEquals;
        }

        private static bool EventCore(ISlimSignatureInfo m, string prefix)
        {
            var prefixMatch = m.Name.AsSpan(0, prefix.Length).SequenceEqual(prefix);
            var name = m.Name.AsSpan(prefix.Length).ToString();
            var hasEvent = m.Declaring.GetEvent(name, BindingFilter) is EventInfo;
            return prefixMatch && hasEvent;
        }

        private static bool FinalizeCore(ISlimSignatureInfo m)
        {
            var nameEquals = string.Equals(nameof(Finalize), m.Name);
            var returnTypeMatched = m.Return == voidType;
            var parametersMatched = m.Parameters.Count == 0;
            return !m.Body.IsStatic && nameEquals && returnTypeMatched && parametersMatched;
        }

        private static bool GetCore(ISlimSignatureInfo m, ref Is @is)
        {
            var prefixMatch = m.Name.AsSpan(0, get_.Length).SequenceEqual(get_);
            var name = m.Name.AsSpan(get_.Length).ToString();
            if (prefixMatch && m.Declaring.GetProperty(name, BindingFilter) is PropertyInfo prop)
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

        private static bool LambdaCore(ISlimSignatureInfo m)
        {
            return
                Attribute.IsDefined(m.Declaring, typeof(CompilerGeneratedAttribute)) &&
                Attribute.IsDefined(m.Declaring, typeof(SerializableAttribute)) &&
                m.Declaring.IsNestedPrivate &&
                m.Declaring.Name.StartsWith(lambdaStartWithTrait);
        }

        private static bool OperationBinaryCore(ISlimSignatureInfo m, string fullname)
        {
            return string.Equals(m.Name, fullname) &&
                m.Parameters.Count == 2 &&
                m.Parameters.Any(p => p.type == m.Declaring);
        }

        private static bool OperationCastCore(ISlimSignatureInfo m, string fullname)
        {
            return 
                string.Equals(fullname, m.Name) &&
                m.Body.IsStatic &&
                m.Parameters.Count == 1 &&
                m.Parameters[0].type == m.Declaring || m.Return == m.Declaring; 
        }

        private static bool OperationShiftCore(ISlimSignatureInfo m, string fullname)
        {
            return
                string.Equals(m.Name, fullname) &&
                m.Body.IsStatic &&
                m.Parameters.Count == 2 &&
                m.Parameters[0].type == m.Declaring &&
                m.Parameters[1].type == typeof(int);
        }

        private static bool OperationUnaryCore(ISlimSignatureInfo m, string fullname)
        {
            return
                string.Equals(m.Name, fullname) &&
                m.Body.IsStatic &&
                m.Parameters.Count == 1 &&
                m.Parameters[0].type == m.Declaring;
        }
        private static bool SetCore(ISlimSignatureInfo m, ref Is @is)
        {
            var prefixMatch = m.Name.AsSpan(0, set_.Length).SequenceEqual(set_);
            var name = m.Name.AsSpan(set_.Length).ToString();
            if (prefixMatch && m.Declaring.GetProperty(name, BindingFilter) is PropertyInfo prop)
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
    }
     
}
