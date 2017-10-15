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

        public static void WriteTo(this SecsMessage msg, TextWriter writer, int indent = 4)
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

        public static void Write(TextWriter writer, Item item, int indent = 4)
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
                    throw new ArgumentOutOfRangeException(nameof(item.Format), item.Format, "invalid SecsFormat value");
            }
            writer.WriteLine('>');
        }

        public static async Task WriteAsync(TextWriter writer, Item item, int indent = 4)
        {
            var indentStr = new string(' ', indent);
            await writer.WriteAsync(indentStr);
            await writer.WriteAsync($"<{item.Format.ToSml()} [{item.Count}] ");
            await WriteItemAcyn();
            await writer.WriteLineAsync('>');

            Task WriteItemAcyn()
            {
                switch (item.Format)
                {
                    case SecsFormat.List: return WriteListAsnc(writer, item, indent, indentStr);
                    case SecsFormat.ASCII:
                    case SecsFormat.JIS8: return writer.WriteAsync($"'{item.GetString()}'");
                    case SecsFormat.Binary: return writer.WriteAsync(item.GetValues<byte>().ToHexString());
                    case SecsFormat.F4: return writer.WriteAsync(string.Join(" ", item.GetValues<float>()));
                    case SecsFormat.F8: return writer.WriteAsync(string.Join(" ", item.GetValues<double>()));
                    case SecsFormat.I1: return writer.WriteAsync(string.Join(" ", item.GetValues<sbyte>()));
                    case SecsFormat.I2: return writer.WriteAsync(string.Join(" ", item.GetValues<short>()));
                    case SecsFormat.I4: return writer.WriteAsync(string.Join(" ", item.GetValues<int>()));
                    case SecsFormat.I8: return writer.WriteAsync(string.Join(" ", item.GetValues<long>()));
                    case SecsFormat.U1: return writer.WriteAsync(string.Join(" ", item.GetValues<byte>()));
                    case SecsFormat.U2: return writer.WriteAsync(string.Join(" ", item.GetValues<ushort>()));
                    case SecsFormat.U4: return writer.WriteAsync(string.Join(" ", item.GetValues<uint>()));
                    case SecsFormat.U8: return writer.WriteAsync(string.Join(" ", item.GetValues<ulong>()));
                    case SecsFormat.Boolean: return writer.WriteAsync(string.Join(" ", item.GetValues<bool>()));
                    default: throw new ArgumentOutOfRangeException($"{nameof(item)}.{nameof(item.Format)}", item.Format, "Invalid enum value");
                }

                async Task WriteListAsnc(TextWriter w, Item secsItem, int d, string dStr)
                {
                    await w.WriteLineAsync();
                    var items = secsItem.Items;
                    for (int i = 0, count = items.Count; i < count; i++)
                        await WriteAsync(writer, items[i], d << 1);
                    await writer.WriteAsync(dStr);
                }
            }
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
                    throw new ArgumentOutOfRangeException(nameof(format), (int)format, "Invalid enum value");
            }
        }

        public static IEnumerable<SecsMessage> ToSecsMessages(this TextReader reader)
        {
            while (reader.Peek() != -1)
            {
                yield return reader.ToSecsMessage();
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
            var stack = new Stack<List<Item>>();
            Item rootItem = null;

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

            while ((line = await sr.ReadLineAsync()) != null && ParseItem(line, stack, ref rootItem))
            {
            }

            return new SecsMessage(s, f, replyExpected, name, rootItem);
        }

        public static SecsMessage ToSecsMessage(this TextReader sr)
        {
            var line = sr.ReadLine();

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
            while ((line = sr.ReadLine()) != null && ParseItem(line, stack, ref rootItem))
            {
            }

            return new SecsMessage(s, f, replyExpected, name, rootItem);
        }

        private static bool ParseItem(string line, Stack<List<Item>> stack, ref Item rootSecsItem)
        {
            line = line.TrimStart();

            if (line[0] == '.')
                return false;

            if (line[0] == '>')
            {
                var itemList = stack.Pop();
                var item = itemList.Count > 0 ? L(itemList) : L();
                if (stack.Count > 0)
                    stack.Peek()
                         .Add(item);
                else
                    rootSecsItem = item;
                return true;
            }

            // <format[count] smlValue

            int indexItemL = line.IndexOf('<') + 1;
            Debug.Assert(indexItemL != 0);
            int indexSizeL = line.IndexOf('[', indexItemL);
            Debug.Assert(indexSizeL != -1);
            string format = line.Substring(indexItemL, indexSizeL - indexItemL).Trim();

            if (format == "L")
            {
                stack.Push(new List<Item>());
            }
            else
            {
                int indexSizeR = line.IndexOf(']', indexSizeL);
                Debug.Assert(indexSizeR != -1);
                int indexItemR = line.LastIndexOf('>');
                Debug.Assert(indexItemR != -1);
                string valueStr = line.Substring(indexSizeR + 1, indexItemR - indexSizeR - 1);
                var item = Create(format, valueStr);
                if (stack.Count > 0)
                    stack.Peek()
                         .Add(item);
                else
                    rootSecsItem = item;
            }

            return true;
        }

        private static byte HexByteParser(string str) => str.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            ? Convert.ToByte(str, 16)
            : byte.Parse(str);

        private static readonly (Func<Item>, Func<byte[], Item>, Func<string, byte>)
            BinaryParser = (B, B, HexByteParser);
        private static readonly (Func<Item>, Func<sbyte[], Item>, Func<string, sbyte>)
            I1Parser = (I1, I1, sbyte.Parse);
        private static readonly (Func<Item>, Func<short[], Item>, Func<string, short>)
            I2Parser = (I2, I2, short.Parse);
        private static readonly (Func<Item>, Func<int[], Item>, Func<string, int>)
            I4Parser = (I4, I4, int.Parse);
        private static readonly (Func<Item>, Func<long[], Item>, Func<string, long>)
            I8Parser = (I8, I8, long.Parse);
        private static readonly (Func<Item>, Func<byte[], Item>, Func<string, byte>)
            U1Parser = (U1, U1, byte.Parse);
        private static readonly (Func<Item>, Func<ushort[], Item>, Func<string, ushort>)
            U2Parser = (U2, U2, ushort.Parse);
        private static readonly (Func<Item>, Func<uint[], Item>, Func<string, uint>)
            U4Parser = (U4, U4, uint.Parse);
        private static readonly (Func<Item>, Func<ulong[], Item>, Func<string, ulong>)
            U8Parser = (U8, U8, ulong.Parse);
        private static readonly (Func<Item>, Func<float[], Item>, Func<string, float>)
            F4Parser = (F4, F4, float.Parse);
        private static readonly (Func<Item>, Func<double[], Item>, Func<string, double>)
            F8Parser = (F8, F8, double.Parse);
        private static readonly (Func<Item>, Func<bool[], Item>, Func<string, bool>)
            BoolParser = (Boolean, Boolean, bool.Parse);
        private static readonly (Func<Item>, Func<string, Item>)
            AParser = (A, A);
        private static readonly (Func<Item>, Func<string, Item>)
            JParser = (J, J);

        private static readonly char[] Separator = { ' ' };

        public static Item Create(this string format, string smlValue)
        {
            switch (format)
            {
                case "A": return ParseStringItem(smlValue, AParser);
                case "JIS8":
                case "J": return ParseStringItem(smlValue, JParser);
                case "Bool":
                case "Boolean": return ParseValueItem(smlValue, BoolParser);
                case "Binary":
                case "B": return ParseValueItem(smlValue, BinaryParser);
                case "I1": return ParseValueItem(smlValue, I1Parser);
                case "I2": return ParseValueItem(smlValue, I2Parser);
                case "I4": return ParseValueItem(smlValue, I4Parser);
                case "I8": return ParseValueItem(smlValue, I8Parser);
                case "U1": return ParseValueItem(smlValue, U1Parser);
                case "U2": return ParseValueItem(smlValue, U2Parser);
                case "U4": return ParseValueItem(smlValue, U4Parser);
                case "U8": return ParseValueItem(smlValue, U8Parser);
                case "F4": return ParseValueItem(smlValue, F4Parser);
                case "F8": return ParseValueItem(smlValue, F8Parser);
                case "L": throw new SecsException("Please use Item.L(...) to create list item.");
                default: throw new SecsException("Unknown SML format :" + format);
            }

            Item ParseValueItem<T>(string str, (Func<Item> emptyCreator, Func<T[], Item> creator, Func<string, T> converter) parser)
            {
                var valueStrs = str.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
                return (valueStrs.Length == 0)
                    ? parser.emptyCreator()
                    : parser.creator(valueStrs.Select(parser.converter).ToArray());
            }

            Item ParseStringItem(string str, (Func<Item> emptyCreator, Func<string, Item> creator) parser)
            {
                str = str.TrimStart(' ', '\'', '"').TrimEnd(' ', '\'', '"');
                return string.IsNullOrEmpty(str)
                    ? parser.emptyCreator()
                    : parser.creator(str);
            }
        }

        public static Item Create(this SecsFormat format, string smlValue) =>
            Create(format.ToSml(), smlValue);
    }
}
