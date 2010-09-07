using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace Secs4Net {
    public static class SecsMessageExtenstion {
        const int SmlIndent = 2;

        public static string ToSML(this SecsMessage msg) {
            using (var sw = new StringWriter()) {
                msg.WriteTo(sw);
                return sw.ToString();
            }
        }

        public static void WriteTo(this SecsMessage msg, TextWriter writer) {
            writer.WriteLine(msg.ToString());
            Write(writer, msg.SecsItem, SmlIndent);
            writer.Write('.');
        }

        static void Write(TextWriter writer, Item item, int indent) {
            if (item == null) return;
            var indentStr = new string(' ', indent);
            writer.Write(indentStr);
            writer.Write('<');
            writer.Write(item.Format.ToSML());
            writer.Write(" [");
            writer.Write(item.Count);
            writer.Write("] ");
            switch (item.Format) {
                case SecsFormat.List:
                    writer.WriteLine();
                    var items = item.Items;
                    int count = items.Count;
                    for (int i = 0; i < count; i++)
                        Write(writer, items[i], indent + SmlIndent);
                    writer.Write(indentStr);
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    writer.Write('\'');
                    writer.Write(item.ToString());
                    writer.Write('\'');
                    break;
                default:
                    writer.Write(item.ToString());
                    break;
            }
            writer.WriteLine('>');
        }

        public static string ToSML(this SecsFormat format) {
            return SecsFormatStrMap[format];
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

        public static SecsMessage ToSecsMessage(this string str) {
            try {
                using (var sr = new StringReader(str))
                    return sr.ToSecsMessage();
            } catch (Exception ex) {
                throw new SecsException("Not well known SML format");
            }
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
            return valueStr => {
                valueStr = valueStr.TrimStart(' ', '\'', '"').TrimEnd(' ', '\'', '"');
                return string.IsNullOrEmpty(valueStr) ?
                        emptyCreator() :
                        itemCreator(valueStr);
            };
        }

        static Func<string, Item> CreateSmlDecoder<T>(Func<T[], Item> creator, Func<Item> emptyCreator, Converter<string, T> converter) where T : struct {
            return valueStr => {
                var valueStrs = valueStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return (valueStrs.Length == 0) ?
                    emptyCreator() :
                    creator(Array.ConvertAll(valueStrs, converter));
            };
        }

        public static Item Create(this string format, string smlValue) {
            return SmlItemParsers[format](smlValue);
        }
        public static Item Create(this SecsFormat format, string smlValue) {
            return Create(format.ToSML(), smlValue);
        }
    }
}
