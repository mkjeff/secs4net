using Microsoft.Toolkit.HighPerformance;
using Secs4Net.Extensions;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Secs4Net.Json
{
    public static class JsonWriter
    {
        public static string ToJson(this Item item, JsonWriterOptions options = default)
        {
            using var mem = new MemoryStream();
            using var jwtr = new Utf8JsonWriter(mem, options);
            item.WriteTo(jwtr);
            jwtr.Flush();
            return Encoding.UTF8.GetString(mem.ToArray());
        }

        public static void WriteTo(this Item item, Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(item.Format.GetName());

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
                    WriteArrayValue<byte>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.Boolean:
                    WriteArrayValue<bool>(writer, item, static (writer, value) => writer.WriteBooleanValue(value));
                    break;
                case SecsFormat.I8:
                    WriteArrayValue<long>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.I1:
                    WriteArrayValue<sbyte>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.I2:
                    WriteArrayValue<short>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.I4:
                    WriteArrayValue<int>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.F4:
                    WriteArrayValue<float>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.F8:
                    WriteArrayValue<double>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.U8:
                    WriteArrayValue<ulong>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.U1:
                    WriteArrayValue<byte>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.U2:
                    WriteArrayValue<ushort>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
                case SecsFormat.U4:
                    WriteArrayValue<uint>(writer, item, static (writer, value) => writer.WriteNumberValue(value));
                    break;
            }
            writer.WriteEndObject();

            static void WriteArrayValue<T>(Utf8JsonWriter writer, Item item, Action<Utf8JsonWriter, T> write) where T : unmanaged
            {
                writer.WriteStartArray();
                var values = item.GetReadOnlyMemory<T>().Span;
                for (var i = 0; i < values.Length; i++)
                {
                    write(writer, values.DangerousGetReferenceAt(i));
                }
                writer.WriteEndArray();
            }
        }
    }
}
