namespace Orlys.Flatten
{
    using System;

    public interface ICatch : INonExceptionCatch
    {
        ICatch Catch<TException>(Catch<TException> @catch) where TException : Exception;

        ICatch Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception;

        ICatch Catch(Catch<Exception> @catch);

        ICatch Catch(Catch<Exception> @catch, When<Exception> when);
    }

    public interface ICatch<T> : INonExceptionCatch<T>
    {
        ICatch<T> Catch<TException>(Catch<TException> @catch) where TException : Exception;

        ICatch<T> Catch<TException>(Catch<TException> @catch, T @default) where TException : Exception;

        ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception;

        ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when, T @default) where TException : Exception;

        ICatch<T> Catch(Catch<Exception> @catch);

        ICatch<T> Catch(Catch<Exception> @catch, T @default);

        ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when);

        ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when, T @default);
    }
}