#if !NET_CORE

namespace Orlys.Diagnostics
{
    using System.Text;

    internal static class FakeSpanBridge
    {
        public static StringBuilder AsSpan(this string str, int startIndex, int length)
        {
            var sb = new StringBuilder();
            if (str.Length < length)
                return sb.Append(str);

            for (int i = startIndex; i < length; i++)
            {
                sb.Append(str[i]);
            }
            return sb;
        }

        public static StringBuilder AsSpan(this string str, int startIndex)
        {
            var sb = new StringBuilder();

            for (int i = startIndex; i < str.Length; i++)
            {
                sb.Append(str[i]);
            }
            return sb;
        }

        public static bool SequenceEqual(this StringBuilder sb, string str)
        {
            if (sb.Length != str.Length)
                return false;
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] != str[i])
                    return false;
            }
            return true;
        }
    }
}
#endif
