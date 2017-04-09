using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Secs4Net.Sml
{
    public static class SmlExtension
    {
        public static string ToSml(this SecsMessage msg)
        {
            if (msg is null)
                return null;

            using (var sw = new StringWriter())
            {
                msg.WriteTo(sw);
                return sw.ToString();
            }
        }

        public static void WriteTo(this SecsMessage msg, TextWriter writer,int indent = 4)
        {
            if (msg is null)
                return;

            writer.WriteLine(msg.ToString());
            if (msg.SecsItem != null)
                Write(writer, msg.SecsItem, indent);
            writer.Write('.');
        }

        public static async Task WriteToAsync(this SecsMessage msg, TextWriter writer, int indent = 4)
        {
            if (msg is null)
                return;

            await writer.WriteLineAsync(msg.ToString());
            if (msg.SecsItem != null)
                await WriteAsync(writer, msg.SecsItem, indent);
            await writer.WriteAsync('.');
        }

        public static void Write(TextWriter writer, SecsItem item, int indent = 4)
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
                    for (int i = 0, count = items.Count; i < count; i++)
                        Write(writer, items[i], indent << 1);
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
                    throw new ArgumentOutOfRangeException(nameof(item) + "." + nameof(item.Format), item.Format,
                        "Invalid enum value");
            }
            writer.WriteLine('>');
        }

        public static async Task WriteAsync(TextWriter writer, SecsItem item, int indent = 4)
        {
            var indentStr = new string(' ', indent);
            await writer.WriteAsync(indentStr);
            await writer.WriteAsync('<');
            await writer.WriteAsync(item.Format.ToSml());
            await writer.WriteAsync(" [");
            await writer.WriteAsync(item.Count.ToString());
            await writer.WriteAsync("] ");
            switch (item.Format)
            {
                case SecsFormat.List:
                    await writer.WriteLineAsync();
                    var items = item.Items;
                    for (int i = 0, count = items.Count; i < count; i++)
                        await WriteAsync(writer, items[i], indent << 1);
                    await writer.WriteAsync(indentStr);
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    await writer.WriteAsync('\'');
                    await writer.WriteAsync(item.GetString());
                    await writer.WriteAsync('\'');
                    break;
                case SecsFormat.Binary:
                    await writer.WriteAsync(item.GetValues<byte>().ToHexString());
                    break;
                case SecsFormat.F4:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<float>()));
                    break;
                case SecsFormat.F8:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<double>()));
                    break;
                case SecsFormat.I1:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<sbyte>()));
                    break;
                case SecsFormat.I2:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<short>()));
                    break;
                case SecsFormat.I4:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<int>()));
                    break;
                case SecsFormat.I8:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<long>()));
                    break;
                case SecsFormat.U1:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<byte>()));
                    break;
                case SecsFormat.U2:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<ushort>()));
                    break;
                case SecsFormat.U4:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<uint>()));
                    break;
                case SecsFormat.U8:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<ulong>()));
                    break;
                case SecsFormat.Boolean:
                    await writer.WriteAsync(string.Join(" ", item.GetValues<bool>()));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item) + "." + nameof(item.Format), item.Format,
                        "Invalid enum value");
            }
            await writer.WriteLineAsync('>');
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
                    throw new ArgumentOutOfRangeException(nameof(format), (int) format, "Invalid enum value");
            }
        }

        public static IEnumerable<SecsMessage> ToSecsMessages(this TextReader reader)
        {
            while (reader.Peek() != -1)
            {
                SecsMessage secsMsg;
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

                i = line.IndexOf("'S", i + 1, StringComparison.Ordinal) + 2;
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

        private static readonly Func<string, byte> HexStringToByte =
            (string str) => str.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                ? Convert.ToByte(str, 16)
                : byte.Parse(str);

        private static readonly char[] Separator = { ' ' };

        public static SecsItem Create(this string format, string smlValue)
        {
            switch (format)
            {
                case "A": return ParseStringItem(smlValue, A, A);
                case "JIS8":
                case "J": return ParseStringItem(smlValue, J, J);
                case "Bool":
                case "Boolean": return ParseValueItem(smlValue, Boolean, Boolean, bool.Parse);
                case "Binary":
                case "B": return ParseValueItem(smlValue, B, B, HexStringToByte);
                case "I1": return ParseValueItem(smlValue, I1, I1, sbyte.Parse);
                case "I2": return ParseValueItem(smlValue, I2, I2, short.Parse);
                case "I4": return ParseValueItem(smlValue, I4, I4, int.Parse);
                case "I8": return ParseValueItem(smlValue, I8, I8, long.Parse);
                case "U1": return ParseValueItem(smlValue, U1, U1, byte.Parse);
                case "U2": return ParseValueItem(smlValue, U2, U2, ushort.Parse);
                case "U4": return ParseValueItem(smlValue, U4, U4, uint.Parse);
                case "U8": return ParseValueItem(smlValue, U8, U8, ulong.Parse);
                case "F4": return ParseValueItem(smlValue, F4, F4, float.Parse);
                case "F8": return ParseValueItem(smlValue, F8, F8, double.Parse);
                case "L": throw new SecsException("Please use Item.L(...) to create list item.");
                default: throw new SecsException("Unknown SML format :" + format);
            }

            SecsItem ParseValueItem<T>(string str, Func<SecsItem> emptyCreator, Func<T[], SecsItem> creator, Func<string, T> converter)
            {
                var valueStrs = str.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
                return (valueStrs.Length == 0)
                    ? emptyCreator()
                    : creator(valueStrs.Select(converter).ToArray());
            }

            SecsItem ParseStringItem(string str, Func<SecsItem> emptyCreator, Func<string, SecsItem> creator)
            {
                str = str.TrimStart(' ', '\'', '"').TrimEnd(' ', '\'', '"');
                return string.IsNullOrEmpty(str)
                    ? emptyCreator()
                    : creator(str);
            }
        }

        public static SecsItem Create(this SecsFormat format, string smlValue) =>
            Create(format.ToSml(), smlValue);
    }
}
