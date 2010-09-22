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
            switch (format) {
                case SecsFormat.List: return "L";
                case SecsFormat.Binary: return "B";
                case SecsFormat.Boolean: return "Boolean";
                case SecsFormat.ASCII: return "A";
                case SecsFormat.JIS8: return "J";
                case SecsFormat.I8: return "I8";
                case SecsFormat.I1: return "I1";
                case SecsFormat.I2: return "I2";
                case SecsFormat.I4: return "I4";
                case SecsFormat.F8: return "F8";
                case SecsFormat.F4: return "F4";
                case SecsFormat.U8: return "U8";
                case SecsFormat.U1: return "U1";
                case SecsFormat.U2: return "U2";
                default/* SecsFormat.U4 */: return "U4";
            }
        }

        public static SecsMessage ToSecsMessage(this string str) {
            using (var sr = new StringReader(str))
                return sr.ToSecsMessage();
        }

        public static SecsMessage ToSecsMessage(this TextReader sr) {
            string line = sr.ReadLine();
            #region Parse First Line
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

        static readonly Func<string, Item> SmlParser_A = CreateSmlParser(Item.A, Item.A);
        static readonly Func<string, Item> SmlParser_J = CreateSmlParser(Item.J, Item.J);
        static readonly Func<string, Item> SmlParser_Boolean = CreateSmlParser<bool>(Item.Boolean, Item.Boolean, bool.Parse);
        static readonly Func<string, Item> SmlParser_B = CreateSmlParser<byte>(Item.B, Item.B, HexStringToByte);
        static readonly Func<string, Item> SmlParser_I1 = CreateSmlParser<sbyte>(Item.I1, Item.I1, sbyte.Parse);
        static readonly Func<string, Item> SmlParser_I2 = CreateSmlParser<short>(Item.I2, Item.I2, short.Parse);
        static readonly Func<string, Item> SmlParser_I4 = CreateSmlParser<int>(Item.I4, Item.I4, int.Parse);
        static readonly Func<string, Item> SmlParser_I8 = CreateSmlParser<long>(Item.I8, Item.I8, long.Parse);
        static readonly Func<string, Item> SmlParser_U1 = CreateSmlParser<byte>(Item.U1, Item.U1, byte.Parse);
        static readonly Func<string, Item> SmlParser_U2 = CreateSmlParser<ushort>(Item.U2, Item.U2, ushort.Parse);
        static readonly Func<string, Item> SmlParser_U4 = CreateSmlParser<uint>(Item.U4, Item.U4, uint.Parse);
        static readonly Func<string, Item> SmlParser_U8 = CreateSmlParser<ulong>(Item.U8, Item.U8, ulong.Parse);
        static readonly Func<string, Item> SmlParser_F4 = CreateSmlParser<float>(Item.F4, Item.F4, float.Parse);
        static readonly Func<string, Item> SmlParser_F8 = CreateSmlParser<double>(Item.F8, Item.F8, double.Parse);

        static byte HexStringToByte(string str) {
            return byte.Parse(str, NumberStyles.HexNumber);
        }

        static Func<string, Item> CreateSmlParser(Func<string, Item> itemCreator, Func<Item> emptyCreator) {
            return valueStr => {
                valueStr = valueStr.TrimStart(' ', '\'', '"').TrimEnd(' ', '\'', '"');
                return string.IsNullOrEmpty(valueStr) ?
                        emptyCreator() :
                        itemCreator(valueStr);
            };
        }

        static Func<string, Item> CreateSmlParser<T>(Func<T[], Item> creator, Func<Item> emptyCreator, Converter<string, T> converter) where T : struct {
            return valueStr => {
                var valueStrs = valueStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return (valueStrs.Length == 0) ?
                    emptyCreator() :
                    creator(Array.ConvertAll(valueStrs, converter));
            };
        }

        public static Item Create(this string format, string smlValue) {
            switch (format) {
                case "A": return SmlParser_A(smlValue);
                case "JIS8":
                case "J": return SmlParser_J(smlValue);
                case "Bool":
                case "Boolean": return SmlParser_Boolean(smlValue);
                case "Binary":
                case "B": return SmlParser_B(smlValue);
                case "I1": return SmlParser_I1(smlValue);
                case "I2": return SmlParser_I2(smlValue);
                case "I4": return SmlParser_I4(smlValue);
                case "I8": return SmlParser_I8(smlValue);
                case "U1": return SmlParser_U1(smlValue);
                case "U2": return SmlParser_U2(smlValue);
                case "U4": return SmlParser_U4(smlValue);
                case "U8": return SmlParser_U8(smlValue);
                case "F4": return SmlParser_F4(smlValue);
                case "F8": return SmlParser_F8(smlValue);
                case "L": throw new SecsException("Please use Item.L(...) to create list item.");
                default: throw new SecsException("Unknown SML format :" + format);
            }
        }
        public static Item Create(this SecsFormat format, string smlValue) {
            return Create(format.ToSML(), smlValue);
        }
    }
}
