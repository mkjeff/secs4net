using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Secs4Net.Sml
{
    public static class SmlWriter
    {
        public static int SmlIndent = 4;

        public static string ToSml(this SecsMessage msg)
        {
            using var sw = new StringWriter();
            msg.WriteSmlTo(sw);
            return sw.ToString();
        }

        public static void WriteSmlTo(this SecsMessage msg, TextWriter writer, int indent = 4)
        {
            if (msg.Name is not null)
            {
                writer.Write(msg.Name);
                writer.Write(':');
            }
            writer.Write("'S");
            writer.Write(msg.S);
            writer.Write('F');
            writer.Write(msg.F);
            writer.Write('\'');
            if (msg.ReplyExpected)
            {
                writer.Write('W');
            }
            writer.WriteLine();

            if (msg.SecsItem is not null)
            {
                writer.Write(msg.SecsItem, indent);
            }

            writer.WriteLine('.');
        }

        public static async Task WriteSmlToAsync(this SecsMessage msg, TextWriter writer, int indent = 4)
        {
            if (msg.Name is not null)
            {
                writer.Write(msg.Name);
                writer.Write(':');
            }
            writer.Write("'S");
            writer.Write(msg.S);
            writer.Write('F');
            writer.Write(msg.F);
            writer.Write('\'');
            if (msg.ReplyExpected)
            {
                writer.Write('W');
            }
            writer.WriteLine();

            if (msg.SecsItem is not null)
            {
                await writer.WriteAsync(msg.SecsItem, indent).ConfigureAwait(false);
            }

            await writer.WriteLineAsync('.').ConfigureAwait(false);
        }

        public static void Write(this TextWriter writer, Item item, int indent = 4)
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
                    foreach (var a in item)
                    {
                        writer.Write(a, indent + SmlIndent);
                    }
                    writer.Write(indentStr);
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    writer.Write('\'');
                    writer.Write(item.GetString());
                    writer.Write('\'');
                    break;
                case SecsFormat.Binary:
                    WriteHexArray(writer, item.GetReadOnlyMemory<byte>());
                    break;
                case SecsFormat.F4:
                    writer.WriteArray(item.GetReadOnlyMemory<float>());
                    break;
                case SecsFormat.F8:
                    writer.WriteArray(item.GetReadOnlyMemory<double>());
                    break;
                case SecsFormat.I1:
                    writer.WriteArray(item.GetReadOnlyMemory<sbyte>());
                    break;
                case SecsFormat.I2:
                    writer.WriteArray(item.GetReadOnlyMemory<short>());
                    break;
                case SecsFormat.I4:
                    writer.WriteArray(item.GetReadOnlyMemory<int>());
                    break;
                case SecsFormat.I8:
                    writer.WriteArray(item.GetReadOnlyMemory<long>());
                    break;
                case SecsFormat.U1:
                    writer.WriteArray(item.GetReadOnlyMemory<byte>());
                    break;
                case SecsFormat.U2:
                    writer.WriteArray(item.GetReadOnlyMemory<ushort>());
                    break;
                case SecsFormat.U4:
                    writer.WriteArray(item.GetReadOnlyMemory<uint>());
                    break;
                case SecsFormat.U8:
                    writer.WriteArray(item.GetReadOnlyMemory<ulong>());
                    break;
                case SecsFormat.Boolean:
                    writer.WriteArray(item.GetReadOnlyMemory<bool>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item.Format), item.Format, "invalid SecsFormat value");
            }
            writer.WriteLine('>');
        }

        public static async Task WriteAsync(this TextWriter writer, Item item, int indent = 4)
        {
            var indentStr = new string(' ', indent);
            await writer.WriteAsync(indentStr).ConfigureAwait(false);
            await writer.WriteAsync($"<{item.Format.ToSml()} [{item.Count}] ").ConfigureAwait(false);
            switch (item.Format)
            {
                case SecsFormat.List:
                    await WriteListAsnc(writer, item, indent, indentStr).ConfigureAwait(false);
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    writer.Write('\'');
                    writer.Write(item.GetString());
                    writer.Write('\'');
                    break;
                case SecsFormat.Binary:
                    writer.WriteHexArray(item.GetReadOnlyMemory<byte>());
                    break;
                case SecsFormat.F4:
                    writer.WriteArray(item.GetReadOnlyMemory<float>());
                    break;
                case SecsFormat.F8:
                    writer.WriteArray(item.GetReadOnlyMemory<double>());
                    break;
                case SecsFormat.I1:
                    writer.WriteArray(item.GetReadOnlyMemory<sbyte>());
                    break;
                case SecsFormat.I2:
                    writer.WriteArray(item.GetReadOnlyMemory<short>());
                    break;
                case SecsFormat.I4:
                    writer.WriteArray(item.GetReadOnlyMemory<int>());
                    break;
                case SecsFormat.I8:
                    writer.WriteArray(item.GetReadOnlyMemory<long>());
                    break;
                case SecsFormat.U1:
                    writer.WriteArray(item.GetReadOnlyMemory<byte>());
                    break;
                case SecsFormat.U2:
                    writer.WriteArray(item.GetReadOnlyMemory<ushort>());
                    break;
                case SecsFormat.U4:
                    writer.WriteArray(item.GetReadOnlyMemory<uint>());
                    break;
                case SecsFormat.U8:
                    writer.WriteArray(item.GetReadOnlyMemory<ulong>());
                    break;
                case SecsFormat.Boolean:
                    writer.WriteArray(item.GetReadOnlyMemory<bool>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item.Format), item.Format, "invalid SecsFormat value");
            }

            await writer.WriteLineAsync('>').ConfigureAwait(false);

            static async Task WriteListAsnc(TextWriter writer, Item item, int indent, string indentStr)
            {
                await writer.WriteLineAsync().ConfigureAwait(false);
                foreach (var a in item)
                {
                    await WriteAsync(writer, a, indent + SmlIndent).ConfigureAwait(false);
                }

                await writer.WriteAsync(indentStr).ConfigureAwait(false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteArray<T>(this TextWriter writer, ReadOnlyMemory<T> memory)
#if NET6_0           
            where T : unmanaged, ISpanFormattable
#else
            where T: unmanaged
#endif
        {
            if (memory.IsEmpty)
            {
                return;
            }

            var array = memory.Span;
            ref var rStart = ref array.DangerousGetReferenceAt(0);
            ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                WriteValue(writer, rStart);
                writer.Write(' ');
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
            WriteValue(writer, rStart);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void WriteValue(TextWriter writer, T value)
#if NET6_0
                => writer.WriteSpanFormattableValue(value);
#else
                => writer.Write(value.ToString());
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteArray(this TextWriter writer, ReadOnlyMemory<float> memory)
        {
            if (memory.IsEmpty)
            {
                return;
            }

            var array = memory.Span;
            ref var rStart = ref array.DangerousGetReferenceAt(0);
            ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                WriteValue(writer, rStart);
                writer.Write(' ');
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
            WriteValue(writer, rStart);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void WriteValue(TextWriter writer, float value)
#if NET6_0
                => writer.WriteSpanFormattableValue(value);
#else
                => writer.Write(value.ToString("G9", CultureInfo.InvariantCulture));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteArray(this TextWriter writer, ReadOnlyMemory<bool> memory)
        {
            if (memory.IsEmpty)
            {
                return;
            }

            var array = memory.Span;
            ref var rStart = ref array.DangerousGetReferenceAt(0);
            ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                writer.Write(rStart.ToString());
                writer.Write(' ');
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
            writer.Write(rStart.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteHexArray(this TextWriter writer, ReadOnlyMemory<byte> memory)
        {
            if (memory.IsEmpty)
            {
                return;
            }

            var array = memory.Span;
            ref var rStart = ref array.DangerousGetReferenceAt(0);
            ref var rEnd = ref array.DangerousGetReferenceAt(array.Length - 1);
            while (Unsafe.IsAddressLessThan(ref rStart, ref rEnd))
            {
                AppendHexChars(writer, rStart);
                writer.Write(' ');
                rStart = ref Unsafe.Add(ref rStart, 1);
            }
            AppendHexChars(writer, rStart);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void AppendHexChars(TextWriter sb, byte num)
            {
                sb.Write("0x");
                var hex1 = Math.DivRem(num, 0x10, out var hex0);
                sb.Write(GetHexChar(hex1));
                sb.Write(GetHexChar(hex0));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static char GetHexChar(int i) => (i < 10) ? (char)(i + 0x30) : (char)(i - 10 + 0x41);
        }

#if NET6_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteSpanFormattableValue<T>(this TextWriter writer, T value) where T : unmanaged, ISpanFormattable
        {
            using var spanOwner = SpanOwner<char>.Allocate(128);
            if (value.TryFormat(spanOwner.Span, out var writtenCount, default, CultureInfo.InvariantCulture))
            {
                writer.Write(spanOwner.Span.Slice(0, writtenCount));
            }
            else
            {
                writer.Write(value.ToString());
            }
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToSml(this SecsFormat format)
        {
            return format switch
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
                _ => ThrowHelper(format),
            };

            [DoesNotReturn]
            static string ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), (int)format, "Invalid enum value");
        }
    }
}
