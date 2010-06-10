using System;
using System.Collections.Generic;
using System.Text;

namespace Secs4Net {
    public static class SecsExtension {
        public static string ToSML(this SecsFormat format) {
            return SecsFormatStrMap[format];
        }

        internal static void CheckNull(this object arg, string argName) {
            if (arg == null) throw new ArgumentNullException(argName);
        }

        static readonly IDictionary<SecsFormat, string> SecsFormatStrMap = new Dictionary<SecsFormat, string>(15) {
            { SecsFormat.List   ,   "L"         },
			{ SecsFormat.ASCII  ,	"A"         },
			{ SecsFormat.JIS8   ,	"J"			},
			{ SecsFormat.Boolean,	"Boolean"	},
			{ SecsFormat.Binary ,	"B"			},
			{ SecsFormat.I1     ,	"I1"		},
			{ SecsFormat.I2     ,	"I2"		},
			{ SecsFormat.I4     ,	"I4"		},
			{ SecsFormat.I8     ,	"I8"		},
			{ SecsFormat.U1     ,	"U1"		},
			{ SecsFormat.U2     ,	"U2"		},
			{ SecsFormat.U4     ,	"U4"		},
			{ SecsFormat.U8     ,	"U8"		},
			{ SecsFormat.F4     ,	"F4"		},
			{ SecsFormat.F8     ,	"F8"		}
		};

        #region Bytes To Item
        internal static readonly IDictionary<SecsFormat, Func<byte[], int, int, Item>> BytesDecoders = new Dictionary<SecsFormat, Func<byte[], int, int, Item>>(14){
            { SecsFormat.ASCII  ,	ToDecoder(Item.A, Item.A, Encoding.ASCII)		            },
            { SecsFormat.JIS8   ,   ToDecoder(Item.J, Item.J, Item.JIS8Encoding)                },
        	{ SecsFormat.Binary ,	ToDecoder<byte>(Item.B, Item.B, sizeof(byte))               },
            { SecsFormat.U1     ,	ToDecoder<byte>(Item.U1, Item.U1, sizeof(byte))             },
        	{ SecsFormat.U2     ,	ToDecoder<ushort>(Item.U2, Item.U2, sizeof(ushort))         },
        	{ SecsFormat.U4     ,	ToDecoder<uint>(Item.U4, Item.U4, sizeof(uint))	            },
        	{ SecsFormat.U8     ,	ToDecoder<ulong>(Item.U8, Item.U8, sizeof(ulong))	        },
        	{ SecsFormat.I1     ,	ToDecoder<sbyte>(Item.I1, Item.I1, sizeof(sbyte))	        },
        	{ SecsFormat.I2     ,	ToDecoder<short>(Item.I2, Item.I2, sizeof(short))	        },
        	{ SecsFormat.I4     ,	ToDecoder<int>(Item.I4, Item.I4, sizeof(int))	            },
        	{ SecsFormat.I8     ,	ToDecoder<long>(Item.I8, Item.I8, sizeof(long))	            },
        	{ SecsFormat.F4     ,	ToDecoder<float>(Item.F4, Item.F4, sizeof(float))	        },
        	{ SecsFormat.F8     ,	ToDecoder<double>(Item.F8, Item.F8, sizeof(double))	        },
            { SecsFormat.Boolean,	ToDecoder<bool>(Item.Boolean, Item.Boolean, sizeof(bool))   }
        };

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
                for (int i = begin; i < end; i += offSet)
                    Array.Reverse(bytes, i, offSet);
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