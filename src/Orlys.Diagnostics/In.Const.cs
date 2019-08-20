namespace Orlys.Diagnostics
{
    using System;
    using System.Reflection;

    public sealed partial class In
    {
        private const string
            add_ = nameof(add_),
            remove_ = nameof(remove_),
            get_ = nameof(get_),
            set_ = nameof(set_),
            indexer_name = "Item",
            deconstructor_full = "Deconstruct",
            lambdaStartWithTrait = "<>",
            op_ = nameof(op_),
            implicit_full = op_ + nameof(Implicit),
            explicit_full = op_ + nameof(Explicit),
            right_shift_full = op_ + nameof(RightShift),
            left_shift_full = op_ + nameof(LeftShift),

            addition_full = op_ + nameof(Addition),
            subtraction_full = op_ + nameof(Subtraction),
            multiply_full = op_ + nameof(Multiply),
            division_full = op_ + nameof(Division),
            modulus_full = op_ + nameof(Modulus),
            bitwiseOr_full = op_ + nameof(BitwiseOr),
            bitwiseAnd_full = op_ + nameof(BitwiseAnd),
            exclusiveOr_full = op_ + nameof(ExclusiveOr),
            equality_full = op_ + nameof(Equality),
            inequality_full = op_ + nameof(Inequality),
            greaterThan_full = op_ + nameof(GreaterThan),
            lessThan_full = op_ + nameof(LessThan),
            onesComplement_full = op_ + nameof(OnesComplement),
            true_full = op_ + nameof(True),
            false_full = op_ + nameof(False),
            increment_full = op_ + nameof(Increment),
            decrement_full = op_ + nameof(Decrement);

        private const BindingFlags BindingFilter = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        private static readonly Type voidType = typeof(void);

        /// <summary>
        /// Indicates the <see langword="add"/> method.
        /// </summary>
        public bool Add { get; }

        /// <summary>
        /// Indicates the <see langword="operator +"/> method.
        /// </summary>
        public bool Addition { get; }

        /// <summary>
        /// Indicates the <see langword="operator &amp;"/> method.
        /// </summary>
        public bool BitwiseAnd { get; }

        /// <summary>
        /// Indicates the <see langword="operator |"/> method.
        /// </summary>
        public bool BitwiseOr { get; }

        /// <summary>
        /// Gets a value indicating whether the method is in <see langword=".ctor"/> or the <see langword=".cctor"/> method.
        /// </summary>
        public bool Constructor { get; }

        /// <summary>
        /// Gets a value indicating whether the method is in <see langword="Deconstruct"/> method.
        /// </summary>
        public bool Deconstructor { get; }

        /// <summary>
        /// Indicates the <see langword="operator --"/> method.
        /// </summary>
        public bool Decrement { get; }

        /// <summary>
        /// Gets a value indicating whether the method is in <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        public bool Dispose { get; }

        /// <summary>
        /// Indicates the <see langword="operator /"/> method.
        /// </summary>
        public bool Division { get; }

        /// <summary>
        /// Indicates the <see langword="operator =="/> method.
        /// </summary>
        public bool Equality { get; }

        /// <summary>
        /// Indicates the <see langword="operator !="/> method.
        /// </summary>
        public bool ExclusiveOr { get; }

        /// <summary>
        /// Indicates the <see langword="explicit operator"/> method.
        /// </summary>
        public bool Explicit { get; }

        /// <summary>
        /// Indicates the <see langword="operator false"/> method.
        /// </summary>
        public bool False { get; }

        /// <summary>
        /// Indicates the <see cref="~"/>(aka Finalize) method.
        /// </summary>
        public bool Finalize { get; }

        /// <summary>
        /// Indicates the <see langword="get"/> method.
        /// </summary>
        public bool Get { get; }

        /// <summary>
        /// Indicates the <see langword="operator &gt;"/> method.
        /// </summary>
        public bool GreaterThan { get; }

        /// <summary>
        /// Indicates the <see langword="implicit operator"/> method.
        /// </summary>
        public bool Implicit { get; }

        /// <summary>
        /// Indicates the <see langword="operator ++"/> method.
        /// </summary>
        public bool Increment { get; }

        /// <summary>
        /// Indicates the <see langword="operator &lt;&lt;"/> method.
        /// </summary>
        public bool Inequality { get; }

        /// <summary>
        /// Indicates the <see langword="delegate"/> method.
        /// </summary>
        public bool Lambda { get; }

        /// <summary>
        /// Indicates the <see langword="operator &lt;&lt;"/> method.
        /// </summary>
        public bool LeftShift { get; }

        /// <summary>
        /// Indicates the <see langword="operator &lt;"/> method.
        /// </summary>
        public bool LessThan { get; }

        /// <summary>
        /// Indicates the <see langword="operator %"/> method.
        /// </summary>
        public bool Modulus { get; }

        /// <summary>
        /// Indicates the <see langword="operator *"/> method.
        /// </summary>
        public bool Multiply { get; }

        /// <summary>
        /// Indicates the <see langword="operator ~"/> method.
        /// </summary>
        public bool OnesComplement { get; }

        /// <summary>
        /// Indicates the <see langword="remove"/> method.
        /// </summary>
        public bool Remove { get; }

        /// <summary>
        /// Indicates the <see langword="operator &gt;&gt;"/> method.
        /// </summary>
        public bool RightShift { get; }

        /// <summary>
        /// Indicates the <see langword="set"/> method.
        /// </summary>
        public bool Set { get; }

        /// <summary>
        /// Indicates the <see langword="operator -"/> method.
        /// </summary>
        public bool Subtraction { get; }

        /// <summary>
        /// Indicates the <see langword="operator true"/> method.
        /// </summary>
        public bool True { get; }
    }
}