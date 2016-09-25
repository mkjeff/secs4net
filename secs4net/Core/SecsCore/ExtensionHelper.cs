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


        public static bool IsMatch(this Item src, Item target)
        {
            if (src.Format != target.Format) return false;
            if (target.Count == 0) return true;
            if (src.Count != target.Count) return false;

            switch (target.Format)
            {
                case SecsFormat.List:
                    return src.Items.Zip(target.Items, (a, b) => a.IsMatch(b)).All(match => match);
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    return (string)src.Values == (string)target.Values;
                default:
                    //return memcmp(Unsafe.As<byte[]>(_values), Unsafe.As<byte[]>(target._values), Buffer.ByteLength((Array)_values)) == 0;
                    return UnsafeCompare((Array)src.Values, (Array)target.Values);
            }
        }

        //[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        //static extern int memcmp(byte[] b1, byte[] b2, long count);

        /// <summary>
        /// http://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net/8808245#8808245
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        static unsafe bool UnsafeCompare(Array a1, Array a2)
        {
            int length = Buffer.ByteLength(a2);
            fixed (byte* p1 = Unsafe.As<byte[]>(a1), p2 = Unsafe.As<byte[]>(a2))
            {
                byte* x1 = p1, x2 = p2;
                int l = length;
                for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                    if (*((long*)x1) != *((long*)x2)) return false;
                if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
                if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
                if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
                return true;
            }
        }

        #region Bytes To Item Value
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
#pragma warning disable CS0618 // Type or member is obsolete
                case SecsFormat.JIS8:
                    return J(JIS8Encoding.GetString(data, index, length));
#pragma warning restore CS0618 // Type or member is obsolete
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
        #endregion

        internal static void Reverse(this byte[] bytes, int begin, int end, int offSet)
        {
            if (offSet > 1)
                for (int i = begin; i < end; i += offSet)
                    Array.Reverse(bytes, i, offSet);
        }

        /// <summary>
        /// Encode Item header + value (initial array only)
        /// </summary>
        /// <param name="valueCount">Item value bytes length</param>
        /// <param name="headerlength">return header bytes length</param>
        /// <param name="format"></param>
        /// <returns>header bytes + initial bytes of value </returns>
        internal static byte[] EncodeItem(this SecsFormat format, int valueCount, out int headerlength)
        {
            byte[] lengthBytes = BitConverter.GetBytes(valueCount);
            int dataLength = format == SecsFormat.List ? 0 : valueCount;

            if (valueCount <= 0xff)
            {//	1 byte
                headerlength = 2;
                var result = new byte[dataLength + 2];
                result[0] = (byte)((byte)format | 1);
                result[1] = lengthBytes[0];
                return result;
            }
            if (valueCount <= 0xffff)
            {//	2 byte
                headerlength = 3;
                var result = new byte[dataLength + 3];
                result[0] = (byte)((byte)format | 2);
                result[1] = lengthBytes[1];
                result[2] = lengthBytes[0];
                return result;
            }
            if (valueCount <= 0xffffff)
            {//	3 byte
                headerlength = 4;
                var result = new byte[dataLength + 4];
                result[0] = (byte)((byte)format | 3);
                result[1] = lengthBytes[2];
                result[2] = lengthBytes[1];
                result[3] = lengthBytes[0];
                return result;
            }
            throw new ArgumentOutOfRangeException(nameof(valueCount), valueCount, $"Item data length({valueCount}) is overflow");
        }

        /// <summary>
        /// Encode item to raw data buffer
        /// </summary>
        /// <param name="item"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal static uint Encode(this Item item, List<ArraySegment<byte>> buffer)
        {
            var bytes = item.RawData.Value;
            uint length = unchecked((uint)bytes.Length);
            buffer.Add(new ArraySegment<byte>(bytes));
            if (item.Format == SecsFormat.List)
                foreach (var subItem in item.Items)
                    length += subItem.Encode(buffer);
            return length;
        }
    }
}