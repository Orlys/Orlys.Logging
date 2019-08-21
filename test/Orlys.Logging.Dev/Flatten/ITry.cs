namespace Orlys.Flatten
{
    using System;

    public interface ITry<T>
    {
        Try<T> Try { get; }
        T Default { get; }

        INonExceptionCatch<T> Catch();

        ICatch<T> Catch<TException>(Catch<TException> @catch) where TException : Exception;

        ICatch<T> Catch<TException>(Catch<TException> @catch, T @default) where TException : Exception;

        ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception;

        ICatch<T> Catch<TException>(Catch<TException> @catch, When<TException> when, T @default) where TException : Exception;

        ICatch<T> Catch(Catch<Exception> @catch);

        ICatch<T> Catch(Catch<Exception> @catch, T @default);

        ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when);

        ICatch<T> Catch(Catch<Exception> @catch, When<Exception> when, T @default);
    }

    public interface ITry
    {
        Try Try { get; }

        INonExceptionCatch Catch();

        ICatch Catch<TException>(Catch<TException> @catch) where TException : Exception;

        ICatch Catch<TException>(Catch<TException> @catch, When<TException> when) where TException : Exception;

        ICatch Catch(Catch<Exception> @catch);

        ICatch Catch(Catch<Exception> @catch, When<Exception> when);
    }
}