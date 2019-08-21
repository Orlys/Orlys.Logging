namespace Orlys.Flatten.Internal
{
    using System;

    internal sealed class NonExceptionCatchBlock : INonExceptionCatch
    {
        public ITry TryBlock { get; }

        internal static NonExceptionCatchBlock Create(ITry @try)
        {
            if (@try == null)
                throw new ArgumentNullException(nameof(@try));
            return new NonExceptionCatchBlock(@try);
        }

        private NonExceptionCatchBlock(ITry @try)
        {
            this.TryBlock = @try;
        }

        public void Go()
        {
            try { this.TryBlock.Try(); } catch { }
        }
    }

    internal sealed class NonExceptionCatchBlock<T> : INonExceptionCatch<T>
    {
        public ITry<T> TryBlock { get; }

        internal static NonExceptionCatchBlock<T> Create(ITry<T> @try)
        {
            if (@try == null)
                throw new ArgumentNullException(nameof(@try));
            return new NonExceptionCatchBlock<T>(@try);
        }

        private NonExceptionCatchBlock(ITry<T> @try)
        {
            this.TryBlock = @try;
        }

        public T Go()
        {
            try
            {
                return this.TryBlock.Try();
            }
            catch
            {
                return this.TryBlock.Default;
            }
        }
    }
}