namespace Orlys.Logging
{
    using System;

    public sealed class LogServiceExeception : Exception
    {
        internal LogServiceExeception(string summary, string detail) : base(summary + "\r\n" + detail)
        {
            this.Summary = summary;
            this.Detail = detail;
        }

        public string Summary { get; }
        public string Detail { get; }
    }


}