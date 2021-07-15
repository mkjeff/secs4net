using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    public static class SecsExtension
    {
        [SkipLocalsInit]
        public static string ToHexString(this IReadOnlyList<byte> value)
        {
            if (value.Count == 0)
            {
                return string.Empty;
            }

            int length = value.Count * 3;
            Span<char> chs = stackalloc char[length];
            for (int ci = 0, i = 0; ci < length; ci += 3)
            {
                byte num = value[i++];
                chs[ci] = GetHexValue(num / 0x10);
                chs[ci + 1] = GetHexValue(num % 0x10);
                chs[ci + 2] = ' ';
            }
            return new string(chs.Slice(0, length - 1));

            static char GetHexValue(int i) => (i < 10) ? (char)(i + 0x30) : (char)(i - 10 + 0x41);
        }


        public static bool IsMatch(this SecsMessage src, SecsMessage target)
        {
            return src.S == target.S
                && src.F == target.F
                && (target.SecsItem == null || src.SecsItem?.IsMatch(target.SecsItem) == true);
        }

        internal static void Reverse(this byte[] bytes, int begin, int end, int offSet)
        {
            if (offSet <= 1)
            {
                return;
            }

            for (var i = begin; i < end; i += offSet)
            {
                Array.Reverse(bytes, i, offSet);
            }
        }

        internal static void Reverse(this Span<byte> bytes, int offSet)
        {
            if (offSet <= 1)
            {
                return;
            }

            for (var i = 0; i < bytes.Length; i += offSet)
            {
                bytes.Slice(i, offSet).Reverse();
            }
        }
    }
}
