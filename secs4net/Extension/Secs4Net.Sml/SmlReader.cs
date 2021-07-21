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
            var (name, s, f, replyExpected) = ParseFirstLine(line);
            
            var stack = new Stack<List<Item>>();
            Item? rootItem = null;
            while ((line = await sr.ReadLineAsync()) != null && ParseItem(line, stack, ref rootItem))
            {
            }

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
                i = line.IndexOf("'S", StringComparison.Ordinal) + 2;
                int j = line.IndexOf('F');
                var s = byte.Parse(line[i..j]);
                line = line[(j + 1)..];
                i = line.IndexOf('\'');
                var f = byte.Parse(line[0..i]);
                var replyExpected = line[i..].IndexOf('W') != -1;
                return (name, s, f, replyExpected);
            }
        }

        public static SecsMessage ToSecsMessage(this TextReader sr)
        {
            ReadOnlySpan<char> line = sr.ReadLine();
            // Parse First Line
            int i = line.IndexOf(':');

            var name = i > 0 ? line.Slice(0, i).ToString() : string.Empty;

            line = line[name.Length..];
            i = line.IndexOf("'S", StringComparison.Ordinal) + 2;
            int j = line.IndexOf('F');
            var s = byte.Parse(line[i..j]);

            line = line[(j + 1)..];
            i = line.IndexOf('\'');
            var f = byte.Parse(line[0..i]);

            var replyExpected = line[i..].IndexOf('W') != -1;

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

        private static byte HexByteParser(ReadOnlySpan<char> str)
            => str.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
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

        public static Item Create(this SecsFormat format, string smlValue)
            => Create(format.ToSml(), smlValue);
    }
}
