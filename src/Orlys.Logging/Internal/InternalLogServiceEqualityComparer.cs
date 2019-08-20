namespace Orlys.Logging.Internal
{ 
    using System.Collections.Generic;

    internal sealed class InternalLogServiceEqualityComparer : IEqualityComparer<LogServiceBase>
    {
        internal InternalLogServiceEqualityComparer()
        {

        }

        public bool Equals(LogServiceBase x, LogServiceBase y)
        {
            if (x == null || y == null)
                return false;

            return x.TypeId.Equals(y.TypeId);
        }
        public int GetHashCode(LogServiceBase obj) => obj.TypeId.GetHashCode();
    }
}