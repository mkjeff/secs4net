using Microsoft.Toolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Secs4Net.Json
{
    public static class JsonWriter
    {
        public static string ToJson(this SecsMessage msg, JsonWriterOptions options = default)
        {
            using var mem = new MemoryStream();
            msg.WriteTo(mem, options);
            return Encoding.UTF8.GetString(mem.ToArray());
        }

        public static string ToJson(this Item item, JsonWriterOptions options = default)
        {
            using var mem = new MemoryStream();
            using var jwtr = new Utf8JsonWriter(mem, options);
            item.WriteTo(jwtr);
            jwtr.Flush();
            return Encoding.UTF8.GetString(mem.ToArray());
        }


        public static void WriteTo(this IEnumerable<SecsMessage> messages, Stream writer, JsonWriterOptions options)
        {
            using var jwtr = new Utf8JsonWriter(writer, options);
            jwtr.WriteStartArray();
            foreach (var msg in messages)
            {
                msg.WriteTo(jwtr);
            }
            jwtr.WriteEndArray();
            jwtr.Flush();
        }

        public static void WriteTo(this SecsMessage msg, Stream writer, JsonWriterOptions options = default)
            => msg.WriteTo(new Utf8JsonWriter(writer, options));

        public static void WriteTo(this SecsMessage msg, Utf8JsonWriter jwtr)
        {
            jwtr.WriteStartObject();

            jwtr.WriteNumber(nameof(msg.S), msg.S);
            jwtr.WriteNumber(nameof(msg.F), msg.F);
            jwtr.WriteBoolean(nameof(msg.ReplyExpected), msg.ReplyExpected);
            jwtr.WriteString(nameof(msg.Name), msg.Name);

            if (msg.SecsItem != null)
            {
                jwtr.WritePropertyName(nameof(msg.SecsItem));
                msg.SecsItem.WriteTo(jwtr);
            }

            jwtr.WriteEndObject();
            jwtr.Flush();
        }

        public static void WriteTo(this Item item, Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(item.Format.ToString());

            switch (item.Format)
            {
                case SecsFormat.List:
                    writer.WriteStartArray();
                    foreach (var subitem in item)
                    {
                        subitem.WriteTo(writer);
                    }
                    writer.WriteEndArray();
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    writer.WriteStringValue(item.GetString());
                    break;
                case SecsFormat.Binary:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<byte>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.Boolean:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<bool>(), static (writer, i) => writer.WriteBooleanValue(i));
                    break;
                case SecsFormat.I8:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<long>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.I1:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<sbyte>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.I2:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<short>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.I4:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<int>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.F4:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<float>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.F8:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<double>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U8:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<ulong>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U1:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<byte>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U2:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<ushort>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U4:
                    WriteArrayValue(writer, item.GetReadOnlyMemory<uint>(), static (writer, i) => writer.WriteNumberValue(i));
                    break;
            }
            writer.WriteEndObject();

            static void WriteArrayValue<T>(Utf8JsonWriter writer, ReadOnlyMemory<T> memory, Action<Utf8JsonWriter, T> write) where T : unmanaged
            {
                writer.WriteStartArray();
                var values = memory.Span;
                for (var i = 0; i < values.Length; i++)
                {
                    write(writer, values.DangerousGetReferenceAt(i));
                }
                writer.WriteEndArray();
            }
        }
    }
}
