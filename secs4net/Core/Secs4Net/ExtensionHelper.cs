using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    public static class SecsExtension
    {
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
