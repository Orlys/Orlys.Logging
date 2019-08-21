namespace Orlys.Flatten.Internal
{
    using System;

    internal sealed class TryBlock : ITry
    {
        public Try Try { get; }

        internal static TryBlock Create(Try @try)
        {
            if (@try == null)
                throw new ArgumentNullException(nameof(@try));
            return new TryBlock(@try);
        }

        private TryBlock(Try @try)
        {
            this.Try = @try;
        }

        /// <summary>
        /// <see langword="catch"/> 區塊，<see langword="when"/> 條件
        /// </summary>
        /// <returns></returns>
        public ICatch Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception
        {
            var cb = CatchBlock.Create(this);
            cb.Catch(@catch, when);
            return cb;
        }

        /// <summary>
        /// <see langword="catch"/> 區塊
        /// </summary>
        /// <returns></returns>
        public ICatch Catch<TException>(Catch<TException> @catch) where TException : Exception
        {
            var cb = CatchBlock.Create(this);
            cb.Catch(@catch);
            return cb;
        }

        /// <summary>
        /// <see langword="catch"/> 區塊為空
        /// </summary>
        /// <returns></returns>
        public INonExceptionCatch Catch()
        {
            return NonExceptionCatchBlock.Create(this);
        }

        public ICatch Catch(Catch<Exception> @catch)
        {
            var cb = CatchBlock.Create(this);
            cb.Catch(@catch);
            return cb;
        }

        public ICatch Catch(Catch<Exception> @catch, When<Exception> when)
        {
            var cb = CatchBlock.Create(this);
            cb.Catch(@catch, when);
            return cb;
        }
    }

    internal sealed class TryBlock<T> : ITry<T>
    {
        public Try<T> Try { get; }

        public T Default { get; private set; }

        internal static TryBlock<T> Create(Try<T> @try, T @default)
        {
            return new TryBlock<T>(@try, @default);
        }

        private TryBlock(Try<T> @try, T @default)
        {
            this.Try = @try;
            this.Default = @default;
        }

        public INonExceptionCatch<T> Catch()
        {
            return NonExceptionCatchBlock<T>.Create(this);
        }

        /// <summary>
        /// <see langword="catch"/> 區塊，<see langword="when"/> 條件
        /// </summary>
        /// <returns></returns>
        public ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when, T @default) where TException : Exception
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, when, @default);
            return cb;
        }

        public ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, when, this.Default);
            return cb;
        }

        /// <summary>
        /// <see langword="catch"/> 區塊
        /// </summary>
        /// <returns></returns>
        public ICatch<T> Catch<TException>(Catch<TException> @catch, T @default) where TException : Exception
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, null, @default);
            return cb;
        }

        public ICatch<T> Catch<TException>(Catch<TException> @catch) where TException : Exception
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, null, this.Default);
            return cb;
        }

        public ICatch<T> Catch(Catch<Exception> @catch)
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, null, this.Default);
            return cb;
        }

        public ICatch<T> Catch(Catch<Exception> @catch, T @default)
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, null, @default);
            return cb;
        }

        public ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when)
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, when, this.Default);
            return cb;
        }

        public ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when, T @default)
        {
            var cb = CatchBlock<T>.Create(this);
            cb.Catch(@catch, when, @default);
            return cb;
        }
    }
}