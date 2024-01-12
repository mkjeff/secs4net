using CommunityToolkit.HighPerformance;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Secs4Net.Sml;

public static class SmlReader
{
    public static async IAsyncEnumerable<SecsMessage> ToSecsMessages(this TextReader reader)
    {
        var stack = new Stack<List<Item>>();
        while (reader.Peek() != -1)
        {
            yield return await reader.ToSecsMessageAsync(stack).ConfigureAwait(false);
            stack.Clear();
        }
    }

    public static SecsMessage ToSecsMessage(this string str)
    {
        using var sr = new StringReader(str);
        return sr.ToSecsMessage();
    }

    public static Task<SecsMessage> ToSecsMessageAsync(this TextReader sr)
    {
        return sr.ToSecsMessageAsync(new Stack<List<Item>>());
    }

    private static async Task<SecsMessage> ToSecsMessageAsync(this TextReader sr, Stack<List<Item>> stack)
    {
        var line = await sr.ReadLineAsync().ConfigureAwait(false);
#if NET
        var (name, s, f, replyExpected) = ParseFirstLine(line);
#else
        var (name, s, f, replyExpected) = ParseFirstLine(line.AsSpan());
#endif

        Item? rootItem = null;

#if NET
        while ((line = await sr.ReadLineAsync().ConfigureAwait(false)) != null && ParseItem(line, stack, ref rootItem)) { }
#else
        while ((line = await sr.ReadLineAsync().ConfigureAwait(false)) != null && ParseItem(line.AsSpan(), stack, ref rootItem)) { }
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

            var name = i > 0 ? line[..i].ToString() : string.Empty;
            line = line[name.Length..];
#if NET
            i = line.IndexOf("'S", StringComparison.Ordinal) + 2;
#else
            i = line.IndexOf("'S".AsSpan(), StringComparison.Ordinal) + 2;
#endif

            int j = line.IndexOf('F');

#if NET
            var s = byte.Parse(line[i..j], provider: CultureInfo.InvariantCulture);
#else
            var s = byte.Parse(line[i..j].ToString(), CultureInfo.InvariantCulture);
#endif

            line = line[(j + 1)..];
            i = line.IndexOf('\'');

#if NET
            var f = byte.Parse(line[0..i], provider: CultureInfo.InvariantCulture);
#else
            var f = byte.Parse(line[0..i].ToString(), CultureInfo.InvariantCulture);
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

        var name = i > 0 ? line[..i].ToString() : string.Empty;

        line = line[name.Length..];

#if NET
        i = line.IndexOf("'S", StringComparison.Ordinal) + 2;
#else
        i = line.IndexOf("'S".AsSpan(), StringComparison.Ordinal) + 2;
#endif

        int j = line.IndexOf('F');

#if NET
        var s = byte.Parse(line[i..j], provider: CultureInfo.InvariantCulture);
#else
        var s = byte.Parse(line[i..j].ToString(), CultureInfo.InvariantCulture);
#endif

        line = line[(j + 1)..];
        i = line.IndexOf('\'');

#if NET
        var f = byte.Parse(line[0..i], provider: CultureInfo.InvariantCulture);
#else
        var f = byte.Parse(line[0..i].ToString(), CultureInfo.InvariantCulture);
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

        if (line.DangerousGetReference() is '.')
        {
            return false;
        }

        if (line.DangerousGetReference() is '>')
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


        int indexSizeR = line[indexSizeL..].IndexOf(']') + indexSizeL;
        Debug.Assert(indexSizeR != -1);

#if NET
        int? size = int.TryParse(line[(indexSizeL + 1)..indexSizeR], out var s) ? s : null;
#else
        int? size = int.TryParse(line[(indexSizeL + 1)..indexSizeR].ToString(), out var s) ? s : null;
#endif

        if (format.DangerousGetReferenceAt(0) == 'L')
        {
            stack.Push(new List<Item>(size ?? 0));
        }
        else
        {
            int indexItemR = line.LastIndexOf('>');
            Debug.Assert(indexItemR != -1);

            var valueStr = line.Slice(indexSizeR + 1, indexItemR - indexSizeR - 1);
            var item = Create(ParseFormat(format), valueStr, size);
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
        ? byte.Parse(str[2..], NumberStyles.HexNumber, provider: CultureInfo.InvariantCulture)
        : byte.Parse(str, provider: CultureInfo.InvariantCulture);
#else
        => str.StartsWith("0x".AsSpan(), StringComparison.OrdinalIgnoreCase)
        ? byte.Parse(str[2..].ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture)
        : byte.Parse(str.ToString(), CultureInfo.InvariantCulture);
#endif

    private static readonly (Func<Item>, Func<byte[], Item>, SpanParser<byte>) BinaryParser = (B, B, HexByteParser);
#if NET
    private static readonly (Func<Item>, Func<sbyte[], Item>, SpanParser<sbyte>) I1Parser = (I1, I1, static span => sbyte.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<short[], Item>, SpanParser<short>) I2Parser = (I2, I2, static span => short.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<int[], Item>, SpanParser<int>) I4Parser = (I4, I4, static span => int.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<long[], Item>, SpanParser<long>) I8Parser = (I8, I8, static span => long.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<byte[], Item>, SpanParser<byte>) U1Parser = (U1, U1, static span => byte.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<ushort[], Item>, SpanParser<ushort>) U2Parser = (U2, U2, static span => ushort.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<uint[], Item>, SpanParser<uint>) U4Parser = (U4, U4, static span => uint.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<ulong[], Item>, SpanParser<ulong>) U8Parser = (U8, U8, static span => ulong.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<float[], Item>, SpanParser<float>) F4Parser = (F4, F4, static span => float.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<double[], Item>, SpanParser<double>) F8Parser = (F8, F8, static span => double.Parse(span, provider: CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<bool[], Item>, SpanParser<bool>) BoolParser = (Boolean, Boolean, bool.Parse);
#else
    private static readonly (Func<Item>, Func<sbyte[], Item>, SpanParser<sbyte>) I1Parser = (I1, I1, static span => sbyte.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<short[], Item>, SpanParser<short>) I2Parser = (I2, I2, static span => short.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<int[], Item>, SpanParser<int>) I4Parser = (I4, I4, static span => int.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<long[], Item>, SpanParser<long>) I8Parser = (I8, I8, static span => long.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<byte[], Item>, SpanParser<byte>) U1Parser = (U1, U1, static span => byte.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<ushort[], Item>, SpanParser<ushort>) U2Parser = (U2, U2, static span => ushort.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<uint[], Item>, SpanParser<uint>) U4Parser = (U4, U4, static span => uint.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<ulong[], Item>, SpanParser<ulong>) U8Parser = (U8, U8, static span => ulong.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<float[], Item>, SpanParser<float>) F4Parser = (F4, F4, static span => float.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<double[], Item>, SpanParser<double>) F8Parser = (F8, F8, static span => double.Parse(span.ToString(), CultureInfo.InvariantCulture));
    private static readonly (Func<Item>, Func<bool[], Item>, SpanParser<bool>) BoolParser = (Boolean, Boolean, static span => bool.Parse(span.ToString()));
#endif
    private static readonly (Func<Item>, Func<string, Item>) AParser = (A, A);
    private static readonly (Func<Item>, Func<string, Item>) JParser = (J, J);

    private static readonly char[] trimElement = [' ', '\'', '"'];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static SecsFormat ParseFormat(ReadOnlySpan<char> format)
    {
        return format.ToString() switch
        {
            "A" => SecsFormat.ASCII,
            "JIS8" or "J" => SecsFormat.JIS8,
            "Bool" or "Boolean" => SecsFormat.Boolean,
            "Binary" or "B" => SecsFormat.Binary,
            "I1" => SecsFormat.I1,
            "I2" => SecsFormat.I2,
            "I4" => SecsFormat.I4,
            "I8" => SecsFormat.I8,
            "U1" => SecsFormat.U1,
            "U2" => SecsFormat.U2,
            "U4" => SecsFormat.U4,
            "U8" => SecsFormat.U8,
            "F4" => SecsFormat.F4,
            "F8" => SecsFormat.F8,
            "L" => SecsFormat.List,
            _ => ThrowHelper(format),
        };

#if NET
        [DoesNotReturn]
        static SecsFormat ThrowHelper(ReadOnlySpan<char> format) => throw new SecsException($"Unknown SML format: {format}");
#else
        [DoesNotReturn]
        static SecsFormat ThrowHelper(ReadOnlySpan<char> format) => throw new SecsException($"Unknown SML format: " + format.ToString());
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Item Create(this SecsFormat format, ReadOnlySpan<char> smlValue, int? size = null)
    {
        return format switch
        {
            SecsFormat.ASCII => ParseStringItem(smlValue, AParser),
            SecsFormat.JIS8 => ParseStringItem(smlValue, JParser),
            SecsFormat.Boolean => ParseArrayItem(smlValue, BoolParser, size),
            SecsFormat.Binary => ParseArrayItem(smlValue, BinaryParser, size),
            SecsFormat.I1 => ParseArrayItem(smlValue, I1Parser, size),
            SecsFormat.I2 => ParseArrayItem(smlValue, I2Parser, size),
            SecsFormat.I4 => ParseArrayItem(smlValue, I4Parser, size),
            SecsFormat.I8 => ParseArrayItem(smlValue, I8Parser, size),
            SecsFormat.U1 => ParseArrayItem(smlValue, U1Parser, size),
            SecsFormat.U2 => ParseArrayItem(smlValue, U2Parser, size),
            SecsFormat.U4 => ParseArrayItem(smlValue, U4Parser, size),
            SecsFormat.U8 => ParseArrayItem(smlValue, U8Parser, size),
            SecsFormat.F4 => ParseArrayItem(smlValue, F4Parser, size),
            SecsFormat.F8 => ParseArrayItem(smlValue, F8Parser, size),
            SecsFormat.List => ThrowHelper("Please use Item.L(...) to create list item."),
            _ => ThrowHelper("Unknown SML format: " + format),
        };

#if NET8_0
        //static Item ParseMemoryItem<T>(ReadOnlySpan<char> str, (Func<Item> emptyCreator, Func<T[], Item> creator) parser, int? size)
        //    where T : unmanaged

        //    , ISpanParsable<T>
        //{
        //    var s = SearchValues.Create(" ");
        //    str.IndexOf()
        //    var range = new Range[09];
        //    str.Split(range, ' ');
        //    var valueStrs = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        //    return valueStrs.IsEmpty()
        //        ? parser.emptyCreator()
        //        : parser.creator(valueStrs.ToArray(parser.converter, size));
        //}
#endif

        static Item ParseArrayItem<T>(ReadOnlySpan<char> str, (Func<Item> emptyCreator, Func<T[], Item> creator, SpanParser<T> converter) parser, int? size) where T : unmanaged
        {
            var valueStrs = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return valueStrs.IsEmpty()
                ? parser.emptyCreator()
                : parser.creator(valueStrs.ToArray(parser.converter, size));
        }

        static Item ParseStringItem(ReadOnlySpan<char> str, (Func<Item> emptyCreator, Func<string, Item> creator) parser)
        {
            str = str.TrimStart(trimElement).TrimEnd(trimElement);
            return str.IsEmpty
                ? parser.emptyCreator()
                : parser.creator(str.ToString());
        }

        [DoesNotReturn]
        static Item ThrowHelper(string message) => throw new SecsException(message);
    }

    public static Item Create(this SecsFormat format, string smlValue)
        => Create(format, smlValue.AsSpan());
}
