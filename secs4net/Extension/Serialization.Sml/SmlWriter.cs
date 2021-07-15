using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Secs4Net.Sml
{
    public static class SmlWriter
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
                    {
                        items[i].Write(writer, indent << 1);
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
                    WriteHexArray(writer, item.GetValues<byte>());
                    break;
                case SecsFormat.F4:
                    WriteArray(writer, item.GetValues<float>());
                    break;
                case SecsFormat.F8:
                    WriteArray(writer, item.GetValues<double>());
                    break;
                case SecsFormat.I1:
                    WriteArray(writer, item.GetValues<sbyte>());
                    break;
                case SecsFormat.I2:
                    WriteArray(writer, item.GetValues<short>());
                    break;
                case SecsFormat.I4:
                    WriteArray(writer, item.GetValues<int>());
                    break;
                case SecsFormat.I8:
                    WriteArray(writer, item.GetValues<long>());
                    break;
                case SecsFormat.U1:
                    WriteArray(writer, item.GetValues<byte>());
                    break;
                case SecsFormat.U2:
                    WriteArray(writer, item.GetValues<ushort>());
                    break;
                case SecsFormat.U4:
                    WriteArray(writer, item.GetValues<uint>());
                    break;
                case SecsFormat.U8:
                    WriteArray(writer, item.GetValues<ulong>());
                    break;
                case SecsFormat.Boolean:
                    WriteArray(writer, item.GetValues<bool>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item.Format), item.Format, "invalid SecsFormat value");
            }
            writer.WriteLine('>');

            static void WriteArray<T>(TextWriter writer, T[] array) where T : unmanaged
            {
                for (int i = 0; i < array.Length - 1; i++)
                {
                    writer.Write(array[i].ToString());
                    writer.Write(' ');
                }

                writer.Write(array[^1].ToString());
            }

            static void WriteHexArray(TextWriter writer, byte[] value)
            {
                if (value.Length == 0)
                {
                    return;
                }

                int length = value.Length * 3;
                for (int i = 0; i < value.Length - 1; i++)
                {
                    AppendHexChars(writer, value[i]);
                    writer.Write(' ');
                }

                AppendHexChars(writer, value[^1]);

                static void AppendHexChars(TextWriter sb, byte num)
                {
                    var hex1 = Math.DivRem(num, 0x10, out var hex0);
                    sb.Write(GetHexChar(hex1));
                    sb.Write(GetHexChar(hex0));
                }
            }
        }

        public static async Task WriteAsync(TextWriter writer, Item item, int indent = 4)
        {
            var indentStr = new string(' ', indent);
            await writer.WriteAsync(indentStr).ConfigureAwait(false);
            await writer.WriteAsync($"<{item.Format.ToSml()} [{item.Count}] ").ConfigureAwait(false);
            await WriteItemAcyn(writer, item, indent, indentStr);
            await writer.WriteLineAsync('>').ConfigureAwait(false);

            static Task WriteItemAcyn(TextWriter writer, Item item, int indent, string indentStr)
                => item.Format switch
                {
                    SecsFormat.List => WriteListAsnc(writer, item, indent, indentStr),
                    SecsFormat.ASCII or SecsFormat.JIS8 => writer.WriteAsync($"'{item.GetString()}'"),
                    SecsFormat.Binary => WriteHexArray(writer, item.GetValues<byte>()),
                    SecsFormat.F4 => WriteArrayAsync(writer, item.GetValues<float>()),
                    SecsFormat.F8 => WriteArrayAsync(writer, item.GetValues<double>()),
                    SecsFormat.I1 => WriteArrayAsync(writer, item.GetValues<sbyte>()),
                    SecsFormat.I2 => WriteArrayAsync(writer, item.GetValues<short>()),
                    SecsFormat.I4 => WriteArrayAsync(writer, item.GetValues<int>()),
                    SecsFormat.I8 => WriteArrayAsync(writer, item.GetValues<long>()),
                    SecsFormat.U1 => WriteArrayAsync(writer, item.GetValues<byte>()),
                    SecsFormat.U2 => WriteArrayAsync(writer, item.GetValues<ushort>()),
                    SecsFormat.U4 => WriteArrayAsync(writer, item.GetValues<uint>()),
                    SecsFormat.U8 => WriteArrayAsync(writer, item.GetValues<ulong>()),
                    SecsFormat.Boolean => WriteArrayAsync(writer, item.GetValues<bool>()),
                    _ => throw new ArgumentOutOfRangeException($"{nameof(item)}.{nameof(item.Format)}", item.Format, "Invalid enum value"),
                };

            static async Task WriteListAsnc(TextWriter writer, Item secsItem, int d, string dStr)
            {
                await writer.WriteLineAsync().ConfigureAwait(false);
                var items = secsItem.Items;
                for (int i = 0, count = items.Count; i < count; i++)
                {
                    await WriteAsync(writer, items[i], d << 1).ConfigureAwait(false);
                }

                await writer.WriteAsync(dStr).ConfigureAwait(false);
            }

            static async Task WriteArrayAsync<T>(TextWriter writer, IReadOnlyList<T> array) where T : unmanaged
            {
                for (int i = 0; i < array.Count - 1; i++)
                {
                    T a = array[i];
                    await writer.WriteAsync(array[i].ToString());
                    await writer.WriteAsync(' ');
                }

                await writer.WriteAsync(array[^1].ToString());
            }

            static async Task WriteHexArray(TextWriter writer, byte[] value)
            {
                if (value.Length == 0)
                {
                    return;
                }

                int length = value.Length * 3;
                for (int i = 0; i < value.Length - 1; i++)
                {
                    await AppendHexChars(writer, value[i]);
                    await writer.WriteAsync(' ');
                }

                await AppendHexChars(writer, value[^1]);

                static async Task AppendHexChars(TextWriter sb, byte num)
                {
                    var hex1 = Math.DivRem(num, 0x10, out var hex0);
                    await sb.WriteAsync(GetHexChar(hex1));
                    await sb.WriteAsync(GetHexChar(hex0));
                }
            }
        }

        internal static string ToSml(this SecsFormat format)
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

        private static char GetHexChar(int i) => (i < 10) ? (char)(i + 0x30) : (char)(i - 10 + 0x41);
    }
}
