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

            jwtr.WritePropertyName(nameof(msg.S));
            jwtr.WriteNumberValue(msg.S);

            jwtr.WritePropertyName(nameof(msg.F));
            jwtr.WriteNumberValue(msg.S);

            jwtr.WritePropertyName(nameof(msg.ReplyExpected));
            jwtr.WriteBooleanValue(msg.ReplyExpected);

            jwtr.WritePropertyName(nameof(msg.Name));
            jwtr.WriteStringValue(msg.Name);

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
                    WriteValue<byte>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.Boolean:
                    WriteValue<bool>(writer, item, static (writer, i) => writer.WriteBooleanValue(i));
                    break;
                case SecsFormat.I8:
                    WriteValue<long>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.I1:
                    WriteValue<sbyte>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.I2:
                    WriteValue<short>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.I4:
                    WriteValue<int>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.F4:
                    WriteValue<float>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.F8:
                    WriteValue<double>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U8:
                    WriteValue<ulong>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U1:
                    WriteValue<byte>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U2:
                    WriteValue<ushort>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
                case SecsFormat.U4:
                    WriteValue<uint>(writer, item, static (writer, i) => writer.WriteNumberValue(i));
                    break;
            }
            writer.WriteEndObject();

            static void WriteValue<T>(Utf8JsonWriter writer, Item item, Action<Utf8JsonWriter, T> write) where T : unmanaged
            {
                writer.WriteStartArray();
                var values = item.GetValues<T>();
                for (var i =0; i< values.Length; i++)
                {
                    write(writer, values[i]);
                }
                writer.WriteEndArray();
            }
        }
    }
}
