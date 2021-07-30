using Microsoft.Toolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Secs4Net.Sml
{
    public static class SmlReader
    {
        public static async IAsyncEnumerable<SecsMessage> ToSecsMessages(this TextReader reader)
        {
            while (reader.Peek() != -1)
            {
                yield return await reader.ToSecsMessageAsync();
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
#if NET
            var (name, s, f, replyExpected) = ParseFirstLine(line);
#else
            var (name, s, f, replyExpected) = ParseFirstLine(line.AsSpan());
#endif

            var stack = new Stack<List<Item>>();
            Item? rootItem = null;

#if NET
            while ((line = await sr.ReadLineAsync()) != null && ParseItem(line, stack, ref rootItem)) { }
#else
            while ((line = await sr.ReadLineAsync()) != null && ParseItem(line.AsSpan(), stack, ref rootItem)) { }
#endif

            return new SecsMessage(s, f, replyExpected)
            {
                Name = name,
                SecsItem = rootItem,
            };

            static (string name, byte s, byte f, bool replyExpected) ParseFirstLine(ReadOnlySpan<char> line)
            {
                // Parse First Line
                int i = line.IndexOf(':');

                var name = i > 0 ? line.Slice(0, i).ToString() : string.Empty;
                line = line[name.Length..];
#if NET
                i = line.IndexOf("'S", StringComparison.Ordinal) + 2;
#else
                i = line.IndexOf("'S".AsSpan(), StringComparison.Ordinal) + 2;
#endif

                int j = line.IndexOf('F');

#if NET
                var s = byte.Parse(line[i..j]);
#else
                var s = byte.Parse(line[i..j].ToString());
#endif

                line = line[(j + 1)..];
                i = line.IndexOf('\'');

#if NET
                var f = byte.Parse(line[0..i]);
#else
                var f = byte.Parse(line[0..i].ToString());
#endif

                var replyExpected = line[i..].IndexOf('W') != -1;
                return (name, s, f, replyExpected);
            }
        }

        public static SecsMessage ToSecsMessage(this TextReader sr)
        {
#if NET
            ReadOnlySpan<char> line = sr.ReadLine();
#else
            ReadOnlySpan<char> line = sr.ReadLine().AsSpan();
#endif
            // Parse First Line
            int i = line.IndexOf(':');

            var name = i > 0 ? line.Slice(0, i).ToString() : string.Empty;

            line = line[name.Length..];

#if NET
            i = line.IndexOf("'S", StringComparison.Ordinal) + 2;
#else
            i = line.IndexOf("'S".AsSpan(), StringComparison.Ordinal) + 2;
#endif

            int j = line.IndexOf('F');

#if NET
            var s = byte.Parse(line[i..j]);
#else
            var s = byte.Parse(line[i..j].ToString());
#endif

            line = line[(j + 1)..];
            i = line.IndexOf('\'');

#if NET
            var f = byte.Parse(line[0..i]);
#else
            var f = byte.Parse(line[0..i].ToString());
#endif

            var replyExpected = line[i..].IndexOf('W') != -1;

            Item? rootItem = null;
            var stack = new Stack<List<Item>>();

#if NET
            while ((line = sr.ReadLine()) != null && ParseItem(line, stack, ref rootItem)) { }
#else
            while ((line = sr.ReadLine().AsSpan()) != null && ParseItem(line, stack, ref rootItem)) { }
#endif

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

            if (format[0] == 'L')
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

        private static byte HexByteParser(ReadOnlySpan<char> str)
#if NET
            => str.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            ? byte.Parse(str, System.Globalization.NumberStyles.HexNumber)
            : byte.Parse(str);
#else
            => str.StartsWith("0x".AsSpan(), StringComparison.OrdinalIgnoreCase)
            ? byte.Parse(str.ToString(), System.Globalization.NumberStyles.HexNumber)
            : byte.Parse(str.ToString());
#endif

        private static readonly (Func<Item>, Func<byte[], Item>, SpanParser<byte>) BinaryParser = (B, B, HexByteParser);
#if NET
        private static readonly (Func<Item>, Func<sbyte[], Item>, SpanParser<sbyte>) I1Parser = (I1, I1, static span => sbyte.Parse(span));
        private static readonly (Func<Item>, Func<short[], Item>, SpanParser<short>) I2Parser = (I2, I2, static span => short.Parse(span));
        private static readonly (Func<Item>, Func<int[], Item>, SpanParser<int>) I4Parser = (I4, I4, static span => int.Parse(span));
        private static readonly (Func<Item>, Func<long[], Item>, SpanParser<long>) I8Parser = (I8, I8, static span => long.Parse(span));
        private static readonly (Func<Item>, Func<byte[], Item>, SpanParser<byte>) U1Parser = (U1, U1, static span => byte.Parse(span));
        private static readonly (Func<Item>, Func<ushort[], Item>, SpanParser<ushort>) U2Parser = (U2, U2, static span => ushort.Parse(span));
        private static readonly (Func<Item>, Func<uint[], Item>, SpanParser<uint>) U4Parser = (U4, U4, static span => uint.Parse(span));
        private static readonly (Func<Item>, Func<ulong[], Item>, SpanParser<ulong>) U8Parser = (U8, U8, static span => ulong.Parse(span));
        private static readonly (Func<Item>, Func<float[], Item>, SpanParser<float>) F4Parser = (F4, F4, static span => float.Parse(span));
        private static readonly (Func<Item>, Func<double[], Item>, SpanParser<double>) F8Parser = (F8, F8, static span => double.Parse(span));
        private static readonly (Func<Item>, Func<bool[], Item>, SpanParser<bool>) BoolParser = (Boolean, Boolean, bool.Parse);
#else
        private static readonly (Func<Item>, Func<sbyte[], Item>, SpanParser<sbyte>) I1Parser = (I1, I1, static span => sbyte.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<short[], Item>, SpanParser<short>) I2Parser = (I2, I2, static span => short.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<int[], Item>, SpanParser<int>) I4Parser = (I4, I4, static span => int.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<long[], Item>, SpanParser<long>) I8Parser = (I8, I8, static span => long.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<byte[], Item>, SpanParser<byte>) U1Parser = (U1, U1, static span => byte.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<ushort[], Item>, SpanParser<ushort>) U2Parser = (U2, U2, static span => ushort.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<uint[], Item>, SpanParser<uint>) U4Parser = (U4, U4, static span => uint.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<ulong[], Item>, SpanParser<ulong>) U8Parser = (U8, U8, static span => ulong.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<float[], Item>, SpanParser<float>) F4Parser = (F4, F4, static span => float.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<double[], Item>, SpanParser<double>) F8Parser = (F8, F8, static span => double.Parse(span.ToString()));
        private static readonly (Func<Item>, Func<bool[], Item>, SpanParser<bool>) BoolParser = (Boolean, Boolean, static span => bool.Parse(span.ToString()));
#endif
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

            static Item ParseValueItem<T>(ReadOnlySpan<char> str, (Func<Item> emptyCreator, Func<T[], Item> creator, SpanParser<T> converter) parser)
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

        public static Item Create(this SecsFormat format, string smlValue)
#if NET
            => Create(format.ToSml(), smlValue);
#else
            => Create(format.ToSml(), smlValue.AsSpan());
#endif
    }
}
