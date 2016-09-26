using System;
using System.Text;
using System.Collections.Generic;
using static Secs4Net.Item;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Runtime.InteropServices;

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
        }

        static char GetHexValue(int i) => (i < 10) ? (char)(i + 0x30) : (char)((i - 10) + 0x41);

        public static bool IsMatch(this SecsMessage src, SecsMessage target)
        {
            if (src.S != target.S) return false;
            if (src.F != target.F) return false;
            if (target.SecsItem == null) return true;
            return src.SecsItem.IsMatch(target.SecsItem);
        }

        internal static Item BytesDecode(this SecsFormat format)
        {
            switch (format)
            {
                case SecsFormat.ASCII: return A();
                case SecsFormat.JIS8: return J();
                case SecsFormat.Boolean: return Boolean();
                case SecsFormat.Binary: return B();
                case SecsFormat.U1: return U1();
                case SecsFormat.U2: return U2();
                case SecsFormat.U4: return U4();
                case SecsFormat.U8: return U8();
                case SecsFormat.I1: return I1();
                case SecsFormat.I2: return I2();
                case SecsFormat.I4: return I4();
                case SecsFormat.I8: return I8();
                case SecsFormat.F4: return F4();
                case SecsFormat.F8: return F8();
            }
            throw new ArgumentException(@"Invalid format:" + format, nameof(format));
        }

        internal static Item BytesDecode(this SecsFormat format, byte[] data, ref int index, ref int length)
        {
            switch (format)
            {
                case SecsFormat.ASCII:
                    return A(Encoding.ASCII.GetString(data, index, length));
                case SecsFormat.JIS8:
                    return J(JIS8Encoding.GetString(data, index, length));
                case SecsFormat.Boolean:
                    return Boolean(Decode<bool>(data, ref index, ref length));
                case SecsFormat.Binary:
                    return B(Decode<byte>(data, ref index, ref length));
                case SecsFormat.U1:
                    return U1(Decode<byte>(data, ref index, ref length));
                case SecsFormat.U2:
                    return U2(Decode<ushort>(data, ref index, ref length));
                case SecsFormat.U4:
                    return U4(Decode<uint>(data, ref index, ref length));
                case SecsFormat.U8:
                    return U8(Decode<ulong>(data, ref index, ref length));
                case SecsFormat.I1:
                    return I1(Decode<sbyte>(data, ref index, ref length));
                case SecsFormat.I2:
                    return I2(Decode<short>(data, ref index, ref length));
                case SecsFormat.I4:
                    return I4(Decode<int>(data, ref index, ref length));
                case SecsFormat.I8:
                    return I8(Decode<long>(data, ref index, ref length));
                case SecsFormat.F4:
                    return F4(Decode<float>(data, ref index, ref length));
                case SecsFormat.F8:
                    return F8(Decode<double>(data, ref index, ref length));
            }
            throw new ArgumentException(@"Invalid format", nameof(format));
        }

        static T[] Decode<T>(byte[] data, ref int index, ref int length) where T : struct
        {
            int elmSize = Unsafe.SizeOf<T>();
            data.Reverse(index, index + length, elmSize);
            var values = new T[length / elmSize];
            Buffer.BlockCopy(data, index, values, 0, length);
            return values;
        }

        internal static void Reverse(this byte[] bytes, int begin, int end, int offSet)
        {
            if (offSet > 1)
                for (int i = begin; i < end; i += offSet)
                    Array.Reverse(bytes, i, offSet);
        }
    }
}