namespace Orlys.Flatten
{
    using System;

    public delegate void Try();
    public delegate T Try<T>();
    public delegate void Catch<TException>(TException exception) where TException : Exception;
    public delegate bool When<TException>(TException exception) where TException : Exception;
}