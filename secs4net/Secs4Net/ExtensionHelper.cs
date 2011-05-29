using System;
using System.Text;
using System.Collections.Generic;

namespace Secs4Net {
    static class SecsExtension {
        #region Bytes To Item Value
        internal static Item BytesDecode(this SecsFormat format) {
            switch (format) {
                case SecsFormat.ASCII: return Item.A();
                case SecsFormat.JIS8: return Item.J();
                case SecsFormat.Boolean: return Item.Boolean();
                case SecsFormat.Binary: return Item.B();
                case SecsFormat.U1: return Item.U1();
                case SecsFormat.U2: return Item.U2();
                case SecsFormat.U4: return Item.U4();
                case SecsFormat.U8: return Item.U8();
                case SecsFormat.I1: return Item.I1();
                case SecsFormat.I2: return Item.I2();
                case SecsFormat.I4: return Item.I4();
                case SecsFormat.I8: return Item.I8();
                case SecsFormat.F4: return Item.F4();
                case SecsFormat.F8: return Item.F8();
            }
            throw new ArgumentException("Invalid format:" + format, "format");
        }

        internal static Item BytesDecode(this SecsFormat format, byte[] bytes, int index, int length) {
            switch (format) {
                case SecsFormat.ASCII: return Item.A(Encoding.ASCII.GetString(bytes, index, length));
                case SecsFormat.JIS8: return Item.J(Item.JIS8Encoding.GetString(bytes, index, length));
                case SecsFormat.Boolean: return Item.Boolean(Decode<bool>(sizeof(bool), bytes, index, length));
                case SecsFormat.Binary: return Item.B(Decode<byte>(sizeof(byte), bytes, index, length));
                case SecsFormat.U1: return Item.U1(Decode<byte>(sizeof(byte), bytes, index, length));
                case SecsFormat.U2: return Item.U2(Decode<ushort>(sizeof(ushort), bytes, index, length));
                case SecsFormat.U4: return Item.U4(Decode<uint>(sizeof(uint), bytes, index, length));
                case SecsFormat.U8: return Item.U8(Decode<ulong>(sizeof(ulong), bytes, index, length));
                case SecsFormat.I1: return Item.I1(Decode<sbyte>(sizeof(sbyte), bytes, index, length));
                case SecsFormat.I2: return Item.I2(Decode<short>(sizeof(short), bytes, index, length));
                case SecsFormat.I4: return Item.I4(Decode<int>(sizeof(int), bytes, index, length));
                case SecsFormat.I8: return Item.I8(Decode<long>(sizeof(long), bytes, index, length));
                case SecsFormat.F4: return Item.F4(Decode<float>(sizeof(float), bytes, index, length));
                case SecsFormat.F8: return Item.F8(Decode<double>(sizeof(double), bytes, index, length));
            }
            throw new ArgumentException("Invalid format:" + format, "format");
        }

        static T[] Decode<T>(int elmSize, byte[] bytes, int index, int length) where T : struct {
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
                byte num = value[i++];
                chs[ci] = GetHexValue(num / 0x10);
                chs[ci + 1] = GetHexValue(num % 0x10);
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
            int dataLength = format == SecsFormat.List ? 0 : valueCount;

            if (valueCount <= 0xff) {//	1 byte
                headerlength = 2;
                var result = new byte[dataLength + 2];
                result[0] = (byte)((byte)format | 1);
                result[1] = lengthBytes[0];
                return result;
            }
            if (valueCount <= 0xffff) {//	2 byte
                headerlength = 3;
                var result = new byte[dataLength + 3];
                result[0] = (byte)((byte)format | 2);
                result[1] = lengthBytes[1];
                result[2] = lengthBytes[0];
                return result;
            }
            if (valueCount <= 0xffffff) {//	3 byte
                headerlength = 4;
                var result = new byte[dataLength + 4];
                result[0] = (byte)((byte)format | 3);
                result[1] = lengthBytes[2];
                result[2] = lengthBytes[1];
                result[3] = lengthBytes[0];
                return result;
            }
            throw new ArgumentOutOfRangeException("byteLength", valueCount, String.Format("Item data length({0}) is overflow", valueCount));
        }

        /// <summary>
        /// Encode item to raw data buffer
        /// </summary>
        /// <param name="item"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal static uint Encode(this Item item, List<RawData> buffer) {
            uint length = (uint)item.RawData.Count;
            buffer.Add(item.RawData);
            if (item.Format == SecsFormat.List)
                for (int i = 0,cnt = item.Items.Count; i < cnt; i++)
                    length += item.Items[i].Encode(buffer);
            return length;
        }
    }
}