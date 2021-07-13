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
            {
                return string.Empty;
            }

            using var sw = new StringWriter();
            msg.WriteTo(sw);
            return sw.ToString();
        }

        public static void WriteTo(this SecsMessage msg, TextWriter writer, int indent = 4)
        {
            if (msg is null)
            {
                return;
            }

            writer.WriteLine(msg.ToString());

            msg.SecsItem?.Write(writer, indent);

            writer.Write('.');
        }

        public static async Task WriteToAsync(this SecsMessage msg, TextWriter writer, int indent = 4)
        {
            if (msg is null)
            {
                return;
            }

            await writer.WriteLineAsync(msg.ToString());
            if (msg.SecsItem != null)
            {
                await WriteAsync(writer, msg.SecsItem, indent);
            }

            await writer.WriteAsync('.');
        }

        public static void Write(this Item item, TextWriter writer, int indent = 4)
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
                        items[i].Write(writer, indent << 1);
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
            await writer.WriteAsync(indentStr).ConfigureAwait(false);
            await writer.WriteAsync($"<{item.Format.ToSml()} [{item.Count}] ").ConfigureAwait(false);
            await WriteItemAcyn(writer, item, indent, indentStr);
            await writer.WriteLineAsync('>').ConfigureAwait(false);

            static Task WriteItemAcyn(TextWriter writer, Item item, int indent, string indentStr) => item.Format switch
            {
                SecsFormat.List => WriteListAsnc(writer, item, indent, indentStr),
                SecsFormat.ASCII or SecsFormat.JIS8 => writer.WriteAsync($"'{item.GetString()}'"),
                SecsFormat.Binary => writer.WriteAsync(item.GetValues<byte>().ToHexString()),
                SecsFormat.F4 => writer.WriteAsync(string.Join(" ", item.GetValues<float>())),
                SecsFormat.F8 => writer.WriteAsync(string.Join(" ", item.GetValues<double>())),
                SecsFormat.I1 => writer.WriteAsync(string.Join(" ", item.GetValues<sbyte>())),
                SecsFormat.I2 => writer.WriteAsync(string.Join(" ", item.GetValues<short>())),
                SecsFormat.I4 => writer.WriteAsync(string.Join(" ", item.GetValues<int>())),
                SecsFormat.I8 => writer.WriteAsync(string.Join(" ", item.GetValues<long>())),
                SecsFormat.U1 => writer.WriteAsync(string.Join(" ", item.GetValues<byte>())),
                SecsFormat.U2 => writer.WriteAsync(string.Join(" ", item.GetValues<ushort>())),
                SecsFormat.U4 => writer.WriteAsync(string.Join(" ", item.GetValues<uint>())),
                SecsFormat.U8 => writer.WriteAsync(string.Join(" ", item.GetValues<ulong>())),
                SecsFormat.Boolean => writer.WriteAsync(string.Join(" ", item.GetValues<bool>())),
                _ => throw new ArgumentOutOfRangeException($"{nameof(item)}.{nameof(item.Format)}", item.Format, "Invalid enum value"),
            };

            static async Task WriteListAsnc(TextWriter writer, Item secsItem, int d, string dStr)
            {
                await writer.WriteLineAsync().ConfigureAwait(false);
                var items = secsItem.Items;
                for (int i = 0, count = items.Count; i < count; i++)
                    await WriteAsync(writer, items[i], d << 1).ConfigureAwait(false);
                await writer.WriteAsync(dStr).ConfigureAwait(false);
            }
        }

        public static string ToSml(this SecsFormat format)
            => format switch
            {
                SecsFormat.List => "L",
                SecsFormat.Binary => "B",
                SecsFormat.Boolean => "Boolean",
                SecsFormat.ASCII => "A",
                SecsFormat.JIS8 => "J",
                SecsFormat.I8 => "I8",
                SecsFormat.I1 => "I1",
                SecsFormat.I2 => "I2",
                SecsFormat.I4 => "I4",
                SecsFormat.F8 => "F8",
                SecsFormat.F4 => "F4",
                SecsFormat.U8 => "U8",
                SecsFormat.U1 => "U1",
                SecsFormat.U2 => "U2",
                SecsFormat.U4 => "U4",
                _ => throw new ArgumentOutOfRangeException(nameof(format), (int)format, "Invalid enum value"),
            };

        public static IEnumerable<SecsMessage> ToSecsMessages(this TextReader reader)
        {
            while (reader.Peek() != -1)
            {
                yield return reader.ToSecsMessage();
            }
        }

        public static SecsMessage ToSecsMessage(this string str)
        {
            using var sr = new StringReader(str);
            return sr.ToSecsMessage();
        }

        public static async Task<SecsMessage> ToSecsMessageAsync(this TextReader sr)
        {
            var line = await sr.ReadLineAsync();
            var stack = new Stack<List<Item>>();
            Item? rootItem = null;

            #region Parse First Line
            Debug.Assert(line != null);
            int i = line.IndexOf(':');
            var name = line.Substring(0, i);

            i = line.IndexOf("'S", i + 1, StringComparison.Ordinal) + 2;
            int j = line.IndexOf('F', i);
            var s = byte.Parse(line.AsSpan().Slice(i, j - i));

            i = line.IndexOf('\'', j);
            var f = byte.Parse(line.AsSpan().Slice(j + 1, i - (j + 1)));

            var replyExpected = line.IndexOf('W', i) != -1;

            #endregion

            while ((line = await sr.ReadLineAsync()) != null && ParseItem(line, stack, ref rootItem))
            {
            }

            return new SecsMessage(s, f, replyExpected)
            {
                Name = name,
                SecsItem = rootItem,
            };
        }

        public static SecsMessage ToSecsMessage(this TextReader sr)
        {
            var line = sr.ReadLine();
            Debug.Assert(line != null);
            #region Parse First Line

            int i = line.IndexOf(':');

            var name = line.Substring(0, i);

            i = line.IndexOf("'S", i + 1, StringComparison.Ordinal) + 2;
            int j = line.IndexOf('F', i);
            var s = byte.Parse(line.AsSpan()[i..j]);

            i = line.IndexOf('\'', j);
            var f = byte.Parse(line.AsSpan()[(j + 1)..i]);

            var replyExpected = line.IndexOf('W', i) != -1;

            #endregion

            Item? rootItem = null;
            var stack = new Stack<List<Item>>();
            while ((line = sr.ReadLine()) != null && ParseItem(line, stack, ref rootItem))
            {
            }

            return new SecsMessage(s, f, replyExpected)
            {
                Name = name,
                SecsItem = rootItem,
            };
        }

        private static bool ParseItem(ReadOnlySpan<char> line, Stack<List<Item>> stack, ref Item? rootSecsItem)
        {
            line = line.TrimStart();

            if (line[0] == '.')
            {
                return false;
            }

            if (line[0] == '>')
            {
                var itemList = stack.Pop();
                var item = itemList.Count > 0 ? L(itemList) : L();
                if (stack.Count > 0)
                {
                    stack.Peek().Add(item);
                }
                else
                {
                    rootSecsItem = item;
                }

                return true;
            }

            // <format[count] smlValue

            int indexItemL = line.IndexOf('<') + 1;
            Debug.Assert(indexItemL != 0);
            int indexSizeL = line[indexItemL..].IndexOf('[') + indexItemL;
            Debug.Assert(indexSizeL != -1);
            var format = line[indexItemL..indexSizeL].Trim();

            if (format.Equals("L", StringComparison.Ordinal))
            {
                stack.Push(new List<Item>());
            }
            else
            {
                int indexSizeR = line[indexSizeL..].IndexOf(']') + indexSizeL;
                Debug.Assert(indexSizeR != -1);
                int indexItemR = line.LastIndexOf('>');
                Debug.Assert(indexItemR != -1);
                var valueStr = line.Slice(indexSizeR + 1, indexItemR - indexSizeR - 1);
                var item = Create(format.ToString(), valueStr);
                if (stack.Count > 0)
                {
                    stack.Peek().Add(item);
                }
                else
                {
                    rootSecsItem = item;
                }
            }

            return true;
        }

        private static byte HexByteParser(ReadOnlySpan<char> str) => str.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            ? byte.Parse(str, System.Globalization.NumberStyles.HexNumber)
            : byte.Parse(str);

        private static readonly (Func<Item>, Func<byte[], Item>, SpanFunc<char, byte>) BinaryParser = (B, B, HexByteParser);
        private static readonly (Func<Item>, Func<sbyte[], Item>, SpanFunc<char, sbyte>) I1Parser = (I1, I1, static span => sbyte.Parse(span));
        private static readonly (Func<Item>, Func<short[], Item>, SpanFunc<char, short>) I2Parser = (I2, I2, static span => short.Parse(span));
        private static readonly (Func<Item>, Func<int[], Item>, SpanFunc<char, int>) I4Parser = (I4, I4, static span => int.Parse(span));
        private static readonly (Func<Item>, Func<long[], Item>, SpanFunc<char, long>) I8Parser = (I8, I8, static span => long.Parse(span));
        private static readonly (Func<Item>, Func<byte[], Item>, SpanFunc<char, byte>) U1Parser = (U1, U1, static span => byte.Parse(span));
        private static readonly (Func<Item>, Func<ushort[], Item>, SpanFunc<char, ushort>) U2Parser = (U2, U2, static span => ushort.Parse(span));
        private static readonly (Func<Item>, Func<uint[], Item>, SpanFunc<char, uint>) U4Parser = (U4, U4, static span => uint.Parse(span));
        private static readonly (Func<Item>, Func<ulong[], Item>, SpanFunc<char, ulong>) U8Parser = (U8, U8, static span => ulong.Parse(span));
        private static readonly (Func<Item>, Func<float[], Item>, SpanFunc<char, float>) F4Parser = (F4, F4, static span => float.Parse(span));
        private static readonly (Func<Item>, Func<double[], Item>, SpanFunc<char, double>) F8Parser = (F8, F8, static span => double.Parse(span));
        private static readonly (Func<Item>, Func<bool[], Item>, SpanFunc<char, bool>) BoolParser = (Boolean, Boolean, bool.Parse);
        private static readonly (Func<Item>, Func<string, Item>) AParser = (A, A);
        private static readonly (Func<Item>, Func<string, Item>) JParser = (J, J);

        private static readonly char[] Separator = { ' ' };
        private static readonly char[] trimElement = new char[] { ' ', '\'', '"' };

        public static Item Create(this string format, ReadOnlySpan<char> smlValue)
        {
            return format switch
            {
                "A" => ParseStringItem(smlValue, AParser),
                "JIS8" or "J" => ParseStringItem(smlValue, JParser),
                "Bool" or "Boolean" => ParseValueItem(smlValue, BoolParser),
                "Binary" or "B" => ParseValueItem(smlValue, BinaryParser),
                "I1" => ParseValueItem(smlValue, I1Parser),
                "I2" => ParseValueItem(smlValue, I2Parser),
                "I4" => ParseValueItem(smlValue, I4Parser),
                "I8" => ParseValueItem(smlValue, I8Parser),
                "U1" => ParseValueItem(smlValue, U1Parser),
                "U2" => ParseValueItem(smlValue, U2Parser),
                "U4" => ParseValueItem(smlValue, U4Parser),
                "U8" => ParseValueItem(smlValue, U8Parser),
                "F4" => ParseValueItem(smlValue, F4Parser),
                "F8" => ParseValueItem(smlValue, F8Parser),
                "L" => throw new SecsException("Please use Item.L(...) to create list item."),
                _ => throw new SecsException("Unknown SML format :" + format),
            };

            static Item ParseValueItem<T>(ReadOnlySpan<char> str, (Func<Item> emptyCreator, Func<T[], Item> creator, SpanFunc<char, T> converter) parser)
            {
                var valueStrs = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return valueStrs.IsEmpty()
                    ? parser.emptyCreator()
                    : parser.creator(valueStrs.Select(parser.converter).ToArray());
            }

            static Item ParseStringItem(ReadOnlySpan<char> str, (Func<Item> emptyCreator, Func<string, Item> creator) parser)
            {
                str = str.TrimStart(trimElement).TrimEnd(trimElement);
                return str.IsEmpty
                    ? parser.emptyCreator()
                    : parser.creator(str.ToString());
            }
        }

        public static Item Create(this SecsFormat format, string smlValue) =>
            Create(format.ToSml(), smlValue);
    }
}