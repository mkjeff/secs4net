using System;
using System.Collections.Generic;
using System.Text;

namespace Secs4Net {
    static class SecsExtension {
        internal static void CheckNull(this object arg, string argName) {
            if (arg == null) throw new ArgumentNullException(argName);
        }

        #region Bytes To Item

        internal static Item BytesDecode(SecsFormat format, byte[] bytes, int index, int length) {
            switch (format) {
                case SecsFormat.ASCII: return length == 0 ? Item.A() : Item.A(Encoding.ASCII.GetString(bytes, index, length));
                case SecsFormat.JIS8: return length == 0 ? Item.J() : Item.J(Item.JIS8Encoding.GetString(bytes, index, length));
                case SecsFormat.Boolean: return length == 0 ? Item.Boolean() : Item.Boolean(Decoder<bool>(sizeof(bool), bytes, index, length));
                case SecsFormat.Binary: return length == 0 ? Item.B() : Item.B(Decoder<byte>(sizeof(byte), bytes, index, length));
                case SecsFormat.U1: return length == 0 ? Item.U1() : Item.U1(Decoder<byte>(sizeof(byte), bytes, index, length));
                case SecsFormat.U2: return length == 0 ? Item.U2() : Item.U2(Decoder<ushort>(sizeof(ushort), bytes, index, length));
                case SecsFormat.U4: return length == 0 ? Item.U4() : Item.U4(Decoder<uint>(sizeof(uint), bytes, index, length));
                case SecsFormat.U8: return length == 0 ? Item.U8() : Item.U8(Decoder<ulong>(sizeof(ulong), bytes, index, length));
                case SecsFormat.I1: return length == 0 ? Item.I1() : Item.I1(Decoder<sbyte>(sizeof(sbyte), bytes, index, length));
                case SecsFormat.I2: return length == 0 ? Item.I2() : Item.I2(Decoder<short>(sizeof(short), bytes, index, length));
                case SecsFormat.I4: return length == 0 ? Item.I4() : Item.I4(Decoder<int>(sizeof(int), bytes, index, length));
                case SecsFormat.I8: return length == 0 ? Item.I8() : Item.I8(Decoder<long>(sizeof(long), bytes, index, length));
                case SecsFormat.F4: return length == 0 ? Item.F4() : Item.F4(Decoder<float>(sizeof(float), bytes, index, length));
                case SecsFormat.F8: return length == 0 ? Item.F8() : Item.F8(Decoder<double>(sizeof(double), bytes, index, length));
                default:/* case SecsFormat.List*/ throw new ArgumentException("Invalid format:" + format, "format");
            }
        }

        static T[] Decoder<T>(int elmSize, byte[] bytes, int index, int length) where T : struct {
            bytes.Reverse(index, index + length, elmSize);
            var values = new T[length / elmSize];
            Buffer.BlockCopy(bytes, index, values, 0, length);
            return values;
        }
        #endregion

        #region Value To SML
        internal static string ToHexString(this byte[] value) {
            if (value.Length == 0) return string.Empty;
            int length = value.Length * 3;
            char[] chs = new char[length];
            for (int ci = 0, i = 0; ci < length; ci += 3) {
                byte num5 = value[i++];
                chs[ci] = GetHexValue(num5 / 0x10);
                chs[ci + 1] = GetHexValue(num5 % 0x10);
                chs[ci + 2] = ' ';
            }
            return new string(chs, 0, length - 1);
        }

        static char GetHexValue(int i) {
            return (i < 10) ? (char)(i + 0x30) : (char)((i - 10) + 0x41);
        }

        internal static string ToSmlString<T>(this T[] value) where T : struct {
            return value.Length == 1 ? value[0].ToString() : string.Join(" ", Array.ConvertAll(value, v => v.ToString()));
        }
        #endregion

        internal static void Reverse(this byte[] bytes, int begin, int end, int offSet) {
            if (offSet > 1)
                for (int i = begin; i < end; i += offSet)
                    Array.Reverse(bytes, i, offSet);
        }

        /// <summary>
        /// Encode Item header + value (initial array only)
        /// </summary>
        /// <param name="dataLength">Item value bytes length</param>
        /// <param name="headerlength">return header bytes length</param>
        /// <returns>header bytes + initial bytes of value </returns>
        internal static byte[] EncodeItem(this SecsFormat format, int valueCount, out int headerlength) {
            byte[] lengthBytes = BitConverter.GetBytes(valueCount);
            int resultLength = format == SecsFormat.List ? 0 : valueCount;

            if (valueCount <= 0xff) {//	1 byte
                headerlength = 2;
                var result = new byte[resultLength + 2];
                result[0] = (byte)((byte)format | 1);
                result[1] = lengthBytes[0];
                return result;
            }
            if (valueCount <= 0xffff) {//	2 byte
                headerlength = 3;
                var result = new byte[resultLength + 3];
                result[0] = (byte)((byte)format | 2);
                result[1] = lengthBytes[1];
                result[2] = lengthBytes[0];
                return result;
            }
            if (valueCount <= 0xffffff) {//	3 byte
                headerlength = 4;
                var result = new byte[resultLength + 4];
                result[0] = (byte)((byte)format | 3);
                result[1] = lengthBytes[2];
                result[2] = lengthBytes[1];
                result[3] = lengthBytes[0];
                return result;
            }
            throw new ArgumentOutOfRangeException("byteLength", valueCount, String.Format("Item data length({0}) is overflow", valueCount));
        }
    }
}