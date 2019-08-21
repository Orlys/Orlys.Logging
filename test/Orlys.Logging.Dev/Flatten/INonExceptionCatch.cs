namespace Orlys.Flatten
{
    public interface INonExceptionCatch<T>
    {
        ITry<T> TryBlock { get; }

        T Go();
    }

    public interface INonExceptionCatch
    {
        ITry TryBlock { get; }

        void Go();
    }
}