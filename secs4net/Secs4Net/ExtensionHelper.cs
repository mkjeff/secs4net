using System;
using System.Collections.Generic;
using System.Text;

namespace Secs4Net {
    public static class SecsExtension {
        internal static void CheckNull(this object arg, string argName) {
            if (arg == null) throw new ArgumentNullException(argName);
        }

        #region Bytes To Item

        static readonly Func<byte[], int, int, Item> decoder_A = ToDecoder(Item.A, Item.A, Encoding.ASCII);
        static readonly Func<byte[], int, int, Item> decoder_J = ToDecoder(Item.J, Item.J, Item.JIS8Encoding);
        static readonly Func<byte[], int, int, Item> decoder_Binary = ToDecoder<byte>(Item.B, Item.B, sizeof(byte));
        static readonly Func<byte[], int, int, Item> decoder_Boolean = ToDecoder<bool>(Item.Boolean, Item.Boolean, sizeof(bool));
        static readonly Func<byte[], int, int, Item> decoder_U1 = ToDecoder<byte>(Item.U1, Item.U1, sizeof(byte));
        static readonly Func<byte[], int, int, Item> decoder_U2 = ToDecoder<ushort>(Item.U2, Item.U2, sizeof(ushort));
        static readonly Func<byte[], int, int, Item> decoder_U4 = ToDecoder<uint>(Item.U4, Item.U4, sizeof(uint));
        static readonly Func<byte[], int, int, Item> decoder_U8 = ToDecoder<ulong>(Item.U8, Item.U8, sizeof(ulong));
        static readonly Func<byte[], int, int, Item> decoder_I1 = ToDecoder<sbyte>(Item.I1, Item.I1, sizeof(sbyte));
        static readonly Func<byte[], int, int, Item> decoder_I2 = ToDecoder<short>(Item.I2, Item.I2, sizeof(short));
        static readonly Func<byte[], int, int, Item> decoder_I4 = ToDecoder<int>(Item.I4, Item.I4, sizeof(int));
        static readonly Func<byte[], int, int, Item> decoder_I8 = ToDecoder<long>(Item.I8, Item.I8, sizeof(long));
        static readonly Func<byte[], int, int, Item> decoder_F4 = ToDecoder<float>(Item.F4, Item.F4, sizeof(float));
        static readonly Func<byte[], int, int, Item> decoder_F8 = ToDecoder<double>(Item.F8, Item.F8, sizeof(double));

        internal static Item BytesDecode(SecsFormat format, byte[] bytes, int index, int length) {
            switch (format) {
                case SecsFormat.ASCII: return decoder_A(bytes, index, length);
                case SecsFormat.JIS8: return decoder_J(bytes, index, length);
                case SecsFormat.Binary: return decoder_Binary(bytes, index, length);
                case SecsFormat.U1: return decoder_U1(bytes, index, length);
                case SecsFormat.U2: return decoder_U2(bytes, index, length);
                case SecsFormat.U4: return decoder_U4(bytes, index, length);
                case SecsFormat.U8: return decoder_U8(bytes, index, length);
                case SecsFormat.I1: return decoder_I1(bytes, index, length);
                case SecsFormat.I2: return decoder_I2(bytes, index, length);
                case SecsFormat.I4: return decoder_I4(bytes, index, length);
                case SecsFormat.I8: return decoder_I8(bytes, index, length);
                case SecsFormat.F4: return decoder_F4(bytes, index, length);
                case SecsFormat.F8: return decoder_F8(bytes, index, length);
                default/*SecsFormat.Boolean*/: return decoder_Boolean(bytes, index, length);
            }
        }

        static Func<byte[], int, int, Item> ToDecoder(Func<string, Item> creator, Func<Item> emptyCreator, Encoding decode) {
            return (bytes, index, length) => length == 0 ? emptyCreator() : creator(decode.GetString(bytes, index, length));
        }

        static Func<byte[], int, int, Item> ToDecoder<T>(Func<T[], Item> ctor, Func<Item> emptyCreator, int elmSize) where T : struct {
            return (bytes, index, length) => {
                if (length == 0) return emptyCreator();
                bytes.Reverse(index, index + length, elmSize);
                var values = new T[length / elmSize];
                Buffer.BlockCopy(bytes, index, values, 0, length);
                return ctor(values);
            };
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
                try {
                    for (int i = begin; i < end; i += offSet)
                        Array.Reverse(bytes, i, offSet);
                } catch (Exception ex) {

                }
        }

        /// <summary>
        /// Encode Item header+value(init only)
        /// </summary>
        /// <param name="dataLength">Item value bytes length</param>
        /// <param name="headerlength">return header bytes length</param>
        /// <returns>header bytes + init value bytes space</returns>
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
            throw new ArgumentOutOfRangeException("byteLength", valueCount, "Item data length(" + valueCount + ") is overflow");
        }
    }
}