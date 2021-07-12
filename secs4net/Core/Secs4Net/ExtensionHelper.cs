using System;

namespace Secs4Net
{
    public static class SecsExtension
    {
        public static string GetName(this SecsFormat format)
            => format switch
            {
                SecsFormat.List => nameof(SecsFormat.List),
                SecsFormat.ASCII => nameof(SecsFormat.ASCII),
                SecsFormat.JIS8 => nameof(SecsFormat.JIS8),
                SecsFormat.Boolean => nameof(SecsFormat.Boolean),
                SecsFormat.Binary => nameof(SecsFormat.Binary),
                SecsFormat.U1 => nameof(SecsFormat.U1),
                SecsFormat.U2 => nameof(SecsFormat.U2),
                SecsFormat.U4 => nameof(SecsFormat.U4),
                SecsFormat.U8 => nameof(SecsFormat.U8),
                SecsFormat.I1 => nameof(SecsFormat.I1),
                SecsFormat.I2 => nameof(SecsFormat.I2),
                SecsFormat.I4 => nameof(SecsFormat.I4),
                SecsFormat.I8 => nameof(SecsFormat.I8),
                SecsFormat.F4 => nameof(SecsFormat.F4),
                SecsFormat.F8 => nameof(SecsFormat.F8),
                _ => throw new ArgumentOutOfRangeException(nameof(format), (int)format, @"Invalid enum value"),
            };

        public static string ToHexString(this byte[] value)
        {
            if (value.Length == 0)
            {
                return string.Empty;
            }

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

            static char GetHexValue(int i) => (i < 10) ? (char)(i + 0x30) : (char)((i - 10) + 0x41);
        }


        public static bool IsMatch(this SecsMessage src, SecsMessage target)
        {
            return src.S == target.S && src.F == target.F &&
                   (target.SecsItem == null || src.SecsItem.IsMatch(target.SecsItem));
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
    }
}