using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using static Secs4Net.Item;

namespace Secs4Net.Json
{
    public static class JsonExtension
    {
        public static string ToJson(this SecsMessage msg)
        {
            using var mem = new MemoryStream();
            msg.WriteTo(mem);
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

        public static void WriteTo(this SecsMessage msg, Stream writer)
            => msg.WriteTo(writer, new JsonWriterOptions { Indented = true });

        public static void WriteTo(this SecsMessage msg, Stream writer, JsonWriterOptions options)
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
                    foreach (var subitem in item.Items)
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

            static void WriteValue<T>(Utf8JsonWriter w, Item i, Action<Utf8JsonWriter, T> write) where T : unmanaged
            {
                w.WriteStartArray();
                foreach (var v in i.GetValues<T>())
                {
                    write(w, v);
                }
                w.WriteEndArray();
            }
        }

        public static SecsMessage ToSecsMessage(this string jsonString)
            => JsonDocument.Parse(jsonString).ToSecsMessage();

        public static SecsMessage ToSecsMessage(this JsonDocument jsonDocument)
        {
            var json = jsonDocument.RootElement;
            var s = json.GetProperty(nameof(SecsMessage.S)).GetByte();
            var f = json.GetProperty(nameof(SecsMessage.F)).GetByte();
            var r = json.GetProperty(nameof(SecsMessage.ReplyExpected)).GetBoolean();
            var name = json.GetProperty(nameof(SecsMessage.Name)).GetString();
            var item = json.TryGetProperty(nameof(SecsMessage.SecsItem), out var root)
                ? root.ToItem()
                : null;

            return new SecsMessage(s, f, replyExpected: r)
            {
                Name = name,
                SecsItem = item,
            };
        }

        // Define other methods and classes here
        public static Item ToItem(this JsonElement jobject)
        {
            var json = jobject.EnumerateObject().First();
            return json.Name switch
            {
                nameof(SecsFormat.List) => L(json.Value.EnumerateArray().Select(ToItem)),
                nameof(SecsFormat.ASCII) => A(json.Value.GetString()),
                nameof(SecsFormat.JIS8) => J(json.Value.GetString()),
                nameof(SecsFormat.Binary) => B(json.Value.EnumerateArray().Select(a => a.GetByte())),
                nameof(SecsFormat.Boolean) => Boolean(json.Value.EnumerateArray().Select(a => a.GetBoolean())),
                nameof(SecsFormat.F4) => F4(json.Value.EnumerateArray().Select(a => a.GetSingle())),
                nameof(SecsFormat.F8) => F8(json.Value.EnumerateArray().Select(a => a.GetDouble())),
                nameof(SecsFormat.I1) => I1(json.Value.EnumerateArray().Select(a => a.GetSByte())),
                nameof(SecsFormat.I2) => I2(json.Value.EnumerateArray().Select(a => a.GetInt16())),
                nameof(SecsFormat.I4) => I4(json.Value.EnumerateArray().Select(a => a.GetInt32())),
                nameof(SecsFormat.I8) => I8(json.Value.EnumerateArray().Select(a => a.GetInt64())),
                nameof(SecsFormat.U1) => U1(json.Value.EnumerateArray().Select(a => a.GetByte())),
                nameof(SecsFormat.U2) => U2(json.Value.EnumerateArray().Select(a => a.GetUInt16())),
                nameof(SecsFormat.U4) => U4(json.Value.EnumerateArray().Select(a => a.GetUInt32())),
                nameof(SecsFormat.U8) => U8(json.Value.EnumerateArray().Select(a => a.GetUInt64())),
                _ => throw new ArgumentOutOfRangeException($"Unknown item format: {json.Name}"),
            };
        }

        public static List<SecsMessage> ToSecsMessages(this Stream stream)
        {
            using var jsonStreamReader = new Utf8JsonStreamReader(stream, 4096);
            jsonStreamReader.Read(); // move to array start
            jsonStreamReader.Read(); // move to start of the object

            var result = new List<SecsMessage>(capacity: 32);
            while (jsonStreamReader.TokenType != JsonTokenType.EndArray)
            {
                // deserialize object
                var jsonObject = jsonStreamReader.GetJsonDocument();
                result.Add(jsonObject.ToSecsMessage());
                // JsonSerializer.Deserialize ends on last token of the object parsed,
                // move to the first token of next object
                jsonStreamReader.Read();
            }

            return result;
        }
    }
}