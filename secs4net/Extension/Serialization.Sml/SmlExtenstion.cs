using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Concurrent;
using System.Linq;
using static Secs4Net.Item;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Secs4Net.Sml
{
    public static class SmlExtenstion
    {
        const int SmlIndent = 2;

        public static string ToSML(this SecsMessage msg)
        {
            using (var sw = new StringWriter())
            {
                msg.WriteTo(sw);
                return sw.ToString();
            }
        }

        public static void WriteTo(this SecsMessage msg, TextWriter writer)
        {
            writer.WriteLine(msg.ToString());
            if (msg.SecsItem != null)
                Write(writer, msg.SecsItem, SmlIndent);
            writer.Write('.');
        }

        static void Write(TextWriter writer, Item item, int indent)
        {
            var indentStr = new string(' ', indent);
            writer.Write(indentStr);
            writer.Write('<');
            writer.Write(item.Format.ToSML());
            writer.Write(" [");
            writer.Write(item.Count);
            writer.Write("] ");
            switch (item.Format)
            {
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
                    writer.Write(item.GetValue<string>());
                    writer.Write('\'');
                    break;
                case SecsFormat.Binary:
                    writer.Write(item.GetValue<byte[]>().ToHexString());
                    break;
                case SecsFormat.F4:
                    writer.Write(string.Join(" ", item.Values.Cast<float>()));
                    break;
                case SecsFormat.F8:
                    writer.Write(string.Join(" ", item.Values.Cast<double>()));
                    break;
                case SecsFormat.I1:
                    writer.Write(string.Join(" ", item.Values.Cast<sbyte>()));
                    break;
                case SecsFormat.I2:
                    writer.Write(string.Join(" ", item.Values.Cast<short>()));
                    break;
                case SecsFormat.I4:
                    writer.Write(string.Join(" ", item.Values.Cast<int>()));
                    break;
                case SecsFormat.I8:
                    writer.Write(string.Join(" ", item.Values.Cast<long>()));
                    break;
                case SecsFormat.U1:
                    writer.Write(string.Join(" ", item.Values.Cast<byte>()));
                    break;
                case SecsFormat.U2:
                    writer.Write(string.Join(" ", item.Values.Cast<ushort>()));
                    break;
                case SecsFormat.U4:
                    writer.Write(string.Join(" ", item.Values.Cast<uint>()));
                    break;
                case SecsFormat.U8:
                    writer.Write(string.Join(" ", item.Values.Cast<ulong>()));
                    break;
                case SecsFormat.Boolean:
                    writer.Write(string.Join(" ", item.Values.Cast<bool>()));
                    break;
                default:
                    writer.Write(item);
                    break;
            }
            writer.WriteLine('>');
        }

        public static string ToSML(this SecsFormat format)
        {
            switch (format)
            {
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
                case SecsFormat.U4: return "U4";
                default: throw new ArgumentException("invalid format", nameof(format));
            }
        }

        public static SecsMessage ToSecsMessage(this string str)
        {
            using (var sr = new StringReader(str))
                return sr.ToSecsMessage();
        }

        public static async Task<SecsMessage> ToSecsMessageAsync(this TextReader sr)
        {
            string line = await sr.ReadLineAsync();
            try
            {
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
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    line = line.TrimStart();
                    if (line[0] == '>')
                    {
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


                    if (format == "L")
                    {
                        stack.Push(new List<Item>());
                        continue;
                    }
                    else
                    {
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

                return new SecsMessage(s, f, replyExpected, name, rootItem);
            }
            catch (Exception ex)
            {
                throw new FormatException("incorrect SML format", ex);
            }
        }

        public static SecsMessage ToSecsMessage(this TextReader sr)
        {
            string line = sr.ReadLine();
            try
            {
                #region Parse First Line
                int i = line.IndexOf(':');

                var name = line.Substring(0, i);

                i = line.IndexOf("'S", i + 1, StringComparison.Ordinal) + 2;
                int j = line.IndexOf('F', i);
                var s = byte.Parse(line.Substring(i, j - i));

                i = line.IndexOf('\'', j);
                var f = byte.Parse(line.Substring(j + 1, i - (j + 1)));

                var replyExpected = line.IndexOf('W', i) != -1;
                #endregion
                Item rootItem = null;
                var stack = new Stack<List<Item>>();
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.TrimStart();
                    if (line[0] == '>')
                    {
                        var itemList = stack.Pop();
                        var item = itemList.Count > 0 ? L(itemList) : L();
                        if (stack.Count > 0)
                            stack.Peek().Add(item);
                        else
                            rootItem = item;
                        continue;
                    }
                    if (line[0] == '.') break;

                    #region <format[count] smlValue
                    int index_Item_L = line.IndexOf('<') + 1; Debug.Assert(index_Item_L != 0);
                    int index_Size_L = line.IndexOf('[', index_Item_L); Debug.Assert(index_Size_L != -1);
                    string format = line.Substring(index_Item_L, index_Size_L - index_Item_L).Trim();


                    if (format == "L")
                    {
                        stack.Push(new List<Item>());
                        continue;
                    }
                    else
                    {
                        int index_Size_R = line.IndexOf(']', index_Size_L); Debug.Assert(index_Size_R != -1);
                        int index_Item_R = line.LastIndexOf('>'); Debug.Assert(index_Item_R != -1);
                        string valueStr = line.Substring(index_Size_R + 1, index_Item_R - index_Size_R - 1);
                        var item = Create(format, valueStr);
                        if (stack.Count > 0)
                            stack.Peek().Add(item);
                        else
                            rootItem = item;
                    }
                    #endregion
                }

                return new SecsMessage(s, f, replyExpected, name, rootItem);
            }
            catch (Exception ex)
            {
                throw new FormatException("incorrect SML format", ex);
            }
        }

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

        static string ToSmlString<T>(this T[] value) where T : struct =>
            value.Length == 1 ? value[0].ToString() : string.Join(" ", value);

        static readonly Func<string, Item> SmlParser_A = CreateSmlParser(A, A);
#pragma warning disable CS0618 // Type or member is obsolete
        static readonly Func<string, Item> SmlParser_J = CreateSmlParser(J, J);
#pragma warning restore CS0618 // Type or member is obsolete
        static readonly Func<string, Item> SmlParser_Boolean = CreateSmlParser(Boolean, Boolean, bool.Parse);
        static readonly Func<string, Item> SmlParser_B = CreateSmlParser(B, B, HexStringToByte);
        static readonly Func<string, Item> SmlParser_I1 = CreateSmlParser(I1, I1, sbyte.Parse);
        static readonly Func<string, Item> SmlParser_I2 = CreateSmlParser(I2, I2, short.Parse);
        static readonly Func<string, Item> SmlParser_I4 = CreateSmlParser(I4, I4, int.Parse);
        static readonly Func<string, Item> SmlParser_I8 = CreateSmlParser(I8, I8, long.Parse);
        static readonly Func<string, Item> SmlParser_U1 = CreateSmlParser(U1, U1, byte.Parse);
        static readonly Func<string, Item> SmlParser_U2 = CreateSmlParser(U2, U2, ushort.Parse);
        static readonly Func<string, Item> SmlParser_U4 = CreateSmlParser(U4, U4, uint.Parse);
        static readonly Func<string, Item> SmlParser_U8 = CreateSmlParser(U8, U8, ulong.Parse);
        static readonly Func<string, Item> SmlParser_F4 = CreateSmlParser(F4, F4, float.Parse);
        static readonly Func<string, Item> SmlParser_F8 = CreateSmlParser(F8, F8, double.Parse);
        static readonly ConcurrentDictionary<string, Item> Cache = new ConcurrentDictionary<string, Item>();

        static byte HexStringToByte(string str) => str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToByte(str, 16) : byte.Parse(str);

        static Func<string, Item> CreateSmlParser(Func<string, Item> itemCreator, Func<Item> emptyCreator) => valueStr =>
                 Cache.GetOrAdd(valueStr, str =>
                 {
                     str = str.TrimStart(' ', '\'', '"').TrimEnd(' ', '\'', '"');
                     return string.IsNullOrEmpty(str) ?
                             emptyCreator() :
                             itemCreator(str);
                 });

        static Func<string, Item> CreateSmlParser<T>(Func<T[], Item> creator, Func<Item> emptyCreator, Func<string, T> converter) where T : struct => valueStr =>
                 Cache.GetOrAdd(valueStr, str =>
                 {
                     var valueStrs = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                     return (valueStrs.Length == 0) ?
                         emptyCreator() :
                         creator(valueStrs.Select(converter).ToArray());
                 });

        public static Item Create(this string format, string smlValue)
        {
            switch (format)
            {
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
        public static Item Create(this SecsFormat format, string smlValue) => Create(format.ToSML(), smlValue);
    }
}
