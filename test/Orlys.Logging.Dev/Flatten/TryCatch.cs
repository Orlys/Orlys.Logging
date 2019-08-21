namespace Orlys.Flatten
{
    using Orlys.Flatten.Internal;

    public static class TryCatch
    {
        public static ITry Try(Try @try)
        {
            return TryBlock.Create(@try);
        }

        public static ITry<T> Try<T>(Try<T> @try, T @default = default)
        {
            return TryBlock<T>.Create(@try, @default);
        }

    }


}
