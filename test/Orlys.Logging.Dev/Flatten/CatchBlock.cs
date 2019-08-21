namespace Orlys.Flatten.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal sealed class CatchBlock : ICatch
    {
        public void Go()
        {
            try
            {
                this.TryBlock.Try();
            }
            catch (Exception e)
            {
                foreach (var @catch in this._catches)
                {
                    if (@catch.Invoke(e))
                        break;
                }
            }
        }

        private interface ICatchBagGlue
        {
            bool Invoke(Exception e);
        }

        private sealed class CatchBag<TException> : ICatchBagGlue where TException : Exception
        {
            private readonly When<TException> _when;
            private readonly Catch<TException> _catch;

            internal CatchBag(When<TException> when, Catch<TException> @catch)
            {
                this._when = when;
                this._catch = @catch;
            }

            public bool Invoke(Exception e)
            {
                if (e is TException ex
                    && (this._when?.Invoke(ex) ?? true))
                {
                    this._catch.Invoke(ex);
                    return true;
                }
                return false;
            }
        }

        private readonly List<ICatchBagGlue> _catches;
        public ITry TryBlock { get; }

        internal static CatchBlock Create(ITry @try)
        {
            if (@try == null)
                throw new ArgumentNullException(nameof(@try));
            return new CatchBlock(@try);
        }

        private CatchBlock(ITry @try)
        {
            this._catches = new List<ICatchBagGlue>();
            this.TryBlock = @try;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch Catch<TException>(Catch<TException> @catch) where TException : Exception
        {
            return Catch(@catch, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception
        {
            this._catches.Add(new CatchBag<TException>(when, @catch));
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch Catch(Catch<Exception> @catch, When<Exception> when)
        {
            this._catches.Add(new CatchBag<Exception>(when, @catch));
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch Catch(Catch<Exception> @catch)
        {
            return Catch(@catch, null);
        }
    }

    internal sealed class CatchBlock<T> : ICatch<T>
    {
        public T Go()
        {
            try
            {
                return this.TryBlock.Try();
            }
            catch (Exception e)
            {
                var def = this.TryBlock.Default;
                foreach (var @catch in this._catches)
                {
                    if (@catch.Invoke(e, ref def))
                        break;
                }
                return def;
            }
        }

        private interface ICatchBagGlue
        {
            bool Invoke(Exception e, ref T @default);
        }

        private sealed class CatchBag<TException> : ICatchBagGlue where TException : Exception
        {
            private readonly When<TException> _when;
            private readonly Catch<TException> _catch;
            private readonly T _default;

            internal CatchBag(When<TException> when, Catch<TException> @catch, T @default)
            {
                this._when = when;
                this._catch = @catch;
                this._default = @default;
            }

            public bool Invoke(Exception e, ref T @default)
            {
                if (e is TException ex
                    && (this._when?.Invoke(ex) ?? true))
                {
                    this._catch.Invoke(ex);
                    @default = this._default;
                    return true;
                }

                return false;
            }
        }

        private readonly List<ICatchBagGlue> _catches;
        public ITry<T> TryBlock { get; }

        internal static ICatch<T> Create(TryBlock<T> @try)
        {
            if (@try == null)
                throw new ArgumentNullException(nameof(@try));
            return new CatchBlock<T>(@try);
        }

        private CatchBlock(TryBlock<T> @try)
        {
            this._catches = new List<ICatchBagGlue>();
            this.TryBlock = @try;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch<TException>(Catch<TException> @catch) where TException : Exception
        {
            return Catch(@catch, null, this.TryBlock.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch<TException>(Catch<TException> @catch, T @default) where TException : Exception
        {
            return Catch(@catch, null, @default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception
        {
            this._catches.Add(new CatchBag<TException>(when, @catch, this.TryBlock.Default));
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when, T @default) where TException : Exception
        {
            this._catches.Add(new CatchBag<TException>(when, @catch, @default));
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch(Catch<Exception> @catch)
        {
            return Catch(@catch, null, this.TryBlock.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch(Catch<Exception> @catch, T @default)
        {
            return Catch(@catch, null, @default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when)
        {
            this._catches.Add(new CatchBag<Exception>(when, @catch, this.TryBlock.Default));
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when, T @default)
        {
            this._catches.Add(new CatchBag<Exception>(when, @catch, @default));
            return this;
        }
    }
}