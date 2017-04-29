using System;
using System.Text;
using System.Runtime.CompilerServices;
using static Secs4Net.Item;

namespace Secs4Net
{
    public static class SecsExtension
    {
        public static string ToHexString(this byte[] value)
        {
            if (value.Length == 0) return string.Empty;
            int length = value.Length * 3;
            char[] chs = new char[length];
            for (int ci = 0, i = 0; ci < length; ci += 3)
            {
                byte num = value[i++];
                chs[ci] = GetHexValue(num / 0x10);
                chs[ci + 1] = GetHexValue(num % 0x10);
                chs[ci + 2] = ' ';
            }
            return new string(chs, 0, length - 1);

            char GetHexValue(int i) => (i < 10) ? (char)(i + 0x30) : (char)((i - 10) + 0x41);
        }


        public static bool IsMatch(this SecsMessage src, SecsMessage target)
        {
            return src.S == target.S && src.F == target.F &&
                   (target.SecsItem == null || src.SecsItem.IsMatch(target.SecsItem));
        }

        internal static void Reverse(this byte[] bytes, int begin, int end, int offSet)
        {
            if (offSet > 1)
                for (int i = begin; i < end; i += offSet)
                    Array.Reverse(bytes, i, offSet);
        }
    }
}