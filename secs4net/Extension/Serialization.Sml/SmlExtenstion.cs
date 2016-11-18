using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Linq;
using static Secs4Net.Item;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Secs4Net.Sml
{
    public static class SmlExtenstion
    {
        private const int SmlIndent = 2;

        public static string ToSml(this SecsMessage msg)
        {
            if (msg == null)
                return null;

            using (var sw = new StringWriter())
            {
                msg.WriteTo(sw);
                return sw.ToString();
            }
        }

        public static void WriteTo(this SecsMessage msg, TextWriter writer)
        {
            if (msg == null)
                return;

            writer.WriteLine(msg.ToString());
            if (msg.SecsItem != null)
                Write(writer, msg.SecsItem, SmlIndent);
            writer.Write('.');
        }

        private static void Write(TextWriter writer, SecsItem item, int indent)
        {
            var indentStr = new string(' ', indent);
            writer.Write(indentStr);
            writer.Write('<');
            writer.Write(item.Format.ToSml());
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
                    writer.Write(item.GetString());
                    writer.Write('\'');
                    break;
                case SecsFormat.Binary:
                    writer.Write(item.GetValues<byte>().ToHexString());
                    break;
                case SecsFormat.F4:
                    writer.Write(string.Join(" ", item.GetValues<float>()));
                    break;
                case SecsFormat.F8:
                    writer.Write(string.Join(" ", item.GetValues<double>()));
                    break;
                case SecsFormat.I1:
                    writer.Write(string.Join(" ", item.GetValues<sbyte>()));
                    break;
                case SecsFormat.I2:
                    writer.Write(string.Join(" ", item.GetValues<short>()));
                    break;
                case SecsFormat.I4:
                    writer.Write(string.Join(" ", item.GetValues<int>()));
                    break;
                case SecsFormat.I8:
                    writer.Write(string.Join(" ", item.GetValues<long>()));
                    break;
                case SecsFormat.U1:
                    writer.Write(string.Join(" ", item.GetValues<byte>()));
                    break;
                case SecsFormat.U2:
                    writer.Write(string.Join(" ", item.GetValues<ushort>()));
                    break;
                case SecsFormat.U4:
                    writer.Write(string.Join(" ", item.GetValues<uint>()));
                    break;
                case SecsFormat.U8:
                    writer.Write(string.Join(" ", item.GetValues<ulong>()));
                    break;
                case SecsFormat.Boolean:
                    writer.Write(string.Join(" ", item.GetValues<bool>()));
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(item) + "." + nameof(item.Format), (int) item.Format,
                        typeof(SecsFormat));
            }
            writer.WriteLine('>');
        }

        public static string ToSml(this SecsFormat format)
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
                default:
                    throw new InvalidEnumArgumentException(nameof(format), (int) format, typeof(SecsFormat));
            }
        }

        public static IEnumerable<SecsMessage> ToSecsMessages(this TextReader reader)
        {
            while (reader.Peek() != -1)
            {
                SecsMessage secsMsg = null;
                try
                {
                    secsMsg = reader.ToSecsMessage();
                }
                catch (Exception ex)
                {
                    throw new Exception("SML parsing error before:\n" + reader.ReadToEnd(), ex);
                }
                if (secsMsg != null)
                    yield return secsMsg;
            }
        }

        public static SecsMessage ToSecsMessage(this string str)
        {
            using (var sr = new StringReader(str))
                return sr.ToSecsMessage();
        }

        public static async Task<SecsMessage> ToSecsMessageAsync(this TextReader sr)
        {
            var line = await sr.ReadLineAsync();
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
                SecsItem rootItem = null;
                var stack = new Stack<List<SecsItem>>();
                while ((line = await sr.ReadLineAsync()) != null)
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
                    int indexItemL = line.IndexOf('<') + 1; //Debug.Assert(index_Item_L != 0);
                    int indexSizeL = line.IndexOf('[', indexItemL); //Debug.Assert(index_Size_L != -1);
                    string format = line.Substring(indexItemL, indexSizeL - indexItemL).Trim();


                    if (format == "L")
                    {
                        stack.Push(new List<SecsItem>());
                    }
                    else
                    {
                        int indexSizeR = line.IndexOf(']', indexSizeL); //Debug.Assert(index_Size_R != -1);
                        int indexItemR = line.LastIndexOf('>'); //Debug.Assert(index_Item_R != -1);
                        string valueStr = line.Substring(indexSizeR + 1, indexItemR - indexSizeR - 1);
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
            var line = sr.ReadLine();
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
                SecsItem rootSecsItem = null;
                var stack = new Stack<List<SecsItem>>();
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
                            rootSecsItem = item;
                        continue;
                    }
                    if (line[0] == '.') break;

                    #region <format[count] smlValue
                    int indexItemL = line.IndexOf('<') + 1; Debug.Assert(indexItemL != 0);
                    int indexSizeL = line.IndexOf('[', indexItemL); Debug.Assert(indexSizeL != -1);
                    string format = line.Substring(indexItemL, indexSizeL - indexItemL).Trim();


                    if (format == "L")
                    {
                        stack.Push(new List<SecsItem>());
                    }
                    else
                    {
                        int indexSizeR = line.IndexOf(']', indexSizeL); Debug.Assert(indexSizeR != -1);
                        int indexItemR = line.LastIndexOf('>'); Debug.Assert(indexItemR != -1);
                        string valueStr = line.Substring(indexSizeR + 1, indexItemR - indexSizeR - 1);
                        var item = Create(format, valueStr);
                        if (stack.Count > 0)
                            stack.Peek().Add(item);
                        else
                            rootSecsItem = item;
                    }
                    #endregion
                }

                return new SecsMessage(s, f, replyExpected, name, rootSecsItem);
            }
            catch (Exception ex)
            {
                throw new FormatException("incorrect SML format", ex);
            }
        }

        private static readonly Func<string, SecsItem> SmlParserA = CreateSmlParser(A, A);
        private static readonly Func<string, SecsItem> SmlParserJ = CreateSmlParser(J, J);
        private static readonly Func<string, SecsItem> SmlParserBoolean = CreateSmlParser(Boolean, Boolean, bool.Parse);
        private static readonly Func<string, SecsItem> SmlParserB = CreateSmlParser(B, B, HexStringToByte);
        private static readonly Func<string, SecsItem> SmlParserI1 = CreateSmlParser(I1, I1, sbyte.Parse);
        private static readonly Func<string, SecsItem> SmlParserI2 = CreateSmlParser(I2, I2, short.Parse);
        private static readonly Func<string, SecsItem> SmlParserI4 = CreateSmlParser(I4, I4, int.Parse);
        private static readonly Func<string, SecsItem> SmlParserI8 = CreateSmlParser(I8, I8, long.Parse);
        private static readonly Func<string, SecsItem> SmlParserU1 = CreateSmlParser(U1, U1, byte.Parse);
        private static readonly Func<string, SecsItem> SmlParserU2 = CreateSmlParser(U2, U2, ushort.Parse);
        private static readonly Func<string, SecsItem> SmlParserU4 = CreateSmlParser(U4, U4, uint.Parse);
        private static readonly Func<string, SecsItem> SmlParserU8 = CreateSmlParser(U8, U8, ulong.Parse);
        private static readonly Func<string, SecsItem> SmlParserF4 = CreateSmlParser(F4, F4, float.Parse);
        private static readonly Func<string, SecsItem> SmlParserF8 = CreateSmlParser(F8, F8, double.Parse);

        private static byte HexStringToByte(string str) => str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToByte(str, 16) : byte.Parse(str);

        private static Func<string, SecsItem> CreateSmlParser(Func<string, SecsItem> itemCreator, Func<SecsItem> emptyCreator)
            => str =>
            {
                str = str.TrimStart(' ', '\'', '"')
                         .TrimEnd(' ', '\'', '"');
                return string.IsNullOrEmpty(str)
                    ? emptyCreator()
                    : itemCreator(str);
            };

        private static Func<string, SecsItem> CreateSmlParser<T>(Func<T[], SecsItem> creator, Func<SecsItem> emptyCreator, Func<string, T> converter) where T : struct
            => str =>
            {
                var valueStrs = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return (valueStrs.Length == 0)
                    ? emptyCreator()
                    : creator(valueStrs.Select(converter).ToArray());
            };

        public static SecsItem Create(this string format, string smlValue)
        {
            switch (format)
            {
                case "A": return SmlParserA(smlValue);
                case "JIS8":
                case "J": return SmlParserJ(smlValue);
                case "Bool":
                case "Boolean": return SmlParserBoolean(smlValue);
                case "Binary":
                case "B": return SmlParserB(smlValue);
                case "I1": return SmlParserI1(smlValue);
                case "I2": return SmlParserI2(smlValue);
                case "I4": return SmlParserI4(smlValue);
                case "I8": return SmlParserI8(smlValue);
                case "U1": return SmlParserU1(smlValue);
                case "U2": return SmlParserU2(smlValue);
                case "U4": return SmlParserU4(smlValue);
                case "U8": return SmlParserU8(smlValue);
                case "F4": return SmlParserF4(smlValue);
                case "F8": return SmlParserF8(smlValue);
                case "L": throw new SecsException("Please use Item.L(...) to create list item.");
                default: throw new SecsException("Unknown SML format :" + format);
            }
        }

        public static SecsItem Create(this SecsFormat format, string smlValue) =>
            Create(format.ToSml(), smlValue);
    }
}
