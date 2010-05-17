using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        #region SML Factory
        public static SecsMessage ToSecsMessage(this string sml) {
            using (var rd = new StringReader(sml))
                return rd.ToSecsMessage();
        }

        public static SecsMessage ToSecsMessage(this TextReader sr) {
            string line = sr.ReadLine();
            #region Parse FirstValue Line
            int i = line.IndexOf(':');

            var name = line.Substring(0, i);

            i = line.IndexOf("'S", i + 1) + 2;
            int j = line.IndexOf('F', i);
            var s = byte.Parse(line.Substring(i, j - i));

            i = line.IndexOf('\'', j);
            var f = byte.Parse(line.Substring(j + 1, i - (j + 1)));

            var replyExpected = line.IndexOf('W', i) != -1;
            #endregion
            Item rootItem = null;
            var stack = new Stack<List<Item>>();
            while ((line = sr.ReadLine()) != null) {
                line = line.TrimStart();
                if (line[0] == '>') {
                    var itemList = stack.Pop();
                    var item = itemList.Count > 0 ? Item.L(itemList) : Item.L();
                    if (stack.Count > 0)
                        stack.Peek().Add(item);
                    else
                        rootItem = item;
                    continue;
                }
                if (line[0] == '.') break;

                #region <format[count] smlValue
                int index_Item_L = line.IndexOf('<') + 1; //Debug.Assert(index_Item_L != 0);
                int index_Size_L = line.IndexOf('[', index_Item_L); //Debug.Assert(index_Size_L != -1);
                string format = line.Substring(index_Item_L, index_Size_L - index_Item_L).Trim();


                if (format == "L") {
                    stack.Push(new List<Item>());
                    continue;
                } else {
                    int index_Size_R = line.IndexOf(']', index_Size_L); //Debug.Assert(index_Size_R != -1);
                    int index_Item_R = line.LastIndexOf('>'); //Debug.Assert(index_Item_R != -1);
                    string valueStr = line.Substring(index_Size_R + 1, index_Item_R - index_Size_R - 1);
                    var item = Create(format, valueStr);
                    if (stack.Count > 0)
                        stack.Peek().Add(item);
                    else
                        rootItem = item;
                }
                #endregion
            }

            return new SecsMessage(s, f, name, replyExpected, rootItem);
        }

        static readonly IDictionary<string, Func<string, Item>> SmlItemParsers = new Dictionary<string, Func<string, Item>>(14, StringComparer.OrdinalIgnoreCase) {
			{ "A"       ,CreateSmlDecoder(Item.A,Item.A)						        },
            { "J"       ,CreateSmlDecoder(Item.J,Item.J)						        },
			{ "BOOLEAN" ,CreateSmlDecoder<bool>(Item.Boolean,Item.Boolean, bool.Parse)	},
			{ "B"       ,CreateSmlDecoder<byte>(Item.B,Item.B, HexStringToByte)         },
			{ "I1"      ,CreateSmlDecoder<sbyte>(Item.I1,Item.I1, sbyte.Parse)		    },
			{ "I2"      ,CreateSmlDecoder<short>(Item.I2,Item.I2, short.Parse)		    },
			{ "I4"      ,CreateSmlDecoder<int>(Item.I4,Item.I4, int.Parse)		    	},
			{ "I8"      ,CreateSmlDecoder<long>(Item.I8,Item.I8, long.Parse)		    },
			{ "U1"      ,CreateSmlDecoder<byte>(Item.U1,Item.U1, byte.Parse)	    	},
			{ "U2"      ,CreateSmlDecoder<ushort>(Item.U2,Item.U2, ushort.Parse)	    },
			{ "U4"      ,CreateSmlDecoder<uint>(Item.U4,Item.U4, uint.Parse)    		},
			{ "U8"      ,CreateSmlDecoder<ulong>(Item.U8,Item.U8, ulong.Parse)	        },
			{ "F4"      ,CreateSmlDecoder<float>(Item.F4,Item.F4, float.Parse)	        },
			{ "F8"      ,CreateSmlDecoder<double>(Item.F8,Item.F8, double.Parse)    	}
		};

        static byte HexStringToByte(string str) {
            return byte.Parse(str, NumberStyles.HexNumber);
        }

        static Func<string, Item> CreateSmlDecoder(Func<string, Item> itemCreator, Func<Item> emptyCreator) {
            var cache = new ConcurrentDictionary<string, Item>();
            return valueStr => {
                return cache.GetOrAdd(valueStr, str => {
                    str = str.TrimStart(' ', '\'', '"').TrimEnd(' ', '\'', '"');
                    return (string.IsNullOrEmpty(str)) ?
                        emptyCreator() :
                        itemCreator(str);
                });
            };
        }

        static Func<string, Item> CreateSmlDecoder<T>(Func<T[], Item> creator, Func<Item> emptyCreator, Converter<string, T> converter) where T : struct {
            var cache = new ConcurrentDictionary<string, Item>();
            return valueStr => {
                return cache.GetOrAdd(valueStr, str => {
                    var valueStrs = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return (valueStrs.Length == 0) ?
                        emptyCreator() :
                        creator(Array.ConvertAll(valueStrs, converter));
                });
            };
        }

        public static Item Create(this string format, string smlValue) {
            return SmlItemParsers[format](smlValue);
        }
        public static Item Create(this SecsFormat format, string smlValue) {
            return Create(format.ToSML(), smlValue);
        }
        #endregion
    }
}