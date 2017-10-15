using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Secs4Net.Item;

namespace Secs4Net.Json
{
    public static class JsonExtension
    {
        public static string ToJson(this SecsMessage msg)
        {
            using (var sw = new StringWriter())
            {
                msg.WriteTo(sw);
                return sw.ToString();
            }
        }

        public static void WriteTo(this SecsMessage msg, TextWriter writer, Formatting formatting = Formatting.Indented)
            => msg.WriteTo(new JsonTextWriter(writer)
            {
                QuoteName = false,
                QuoteChar = '\'',
                Formatting = Formatting.Indented
            });

        public static Task WriteToAsync(this SecsMessage msg, TextWriter writer, Formatting formatting = Formatting.Indented)
            => msg.WriteToAsync(new JsonTextWriter(writer)
            {
                QuoteName = false,
                QuoteChar = '\'',
                Formatting = Formatting.Indented
            });

        public static void WriteTo(this SecsMessage msg, JsonTextWriter jwtr)
        {
            jwtr.WriteStartObject();

            jwtr.WritePropertyName(nameof(msg.S));
            jwtr.WriteValue(msg.S);

            jwtr.WritePropertyName(nameof(msg.F));
            jwtr.WriteValue(msg.S);

            jwtr.WritePropertyName(nameof(msg.ReplyExpected));
            jwtr.WriteValue(msg.ReplyExpected);

            jwtr.WritePropertyName(nameof(msg.Name));
            jwtr.WriteValue(msg.Name);

            if (msg.SecsItem != null)
            {
                jwtr.WritePropertyName(nameof(msg.SecsItem));
                msg.SecsItem.WriteTo(jwtr);
            }

            jwtr.WriteEndObject();
        }

        public static async Task WriteToAsync(this SecsMessage msg, JsonTextWriter jwtr)
        {
            await jwtr.WriteStartObjectAsync();

            await jwtr.WritePropertyNameAsync(nameof(msg.S));
            await jwtr.WriteValueAsync(msg.S);

            await jwtr.WritePropertyNameAsync(nameof(msg.F));
            await jwtr.WriteValueAsync(msg.S);

            await jwtr.WritePropertyNameAsync(nameof(msg.ReplyExpected));
            await jwtr.WriteValueAsync(msg.ReplyExpected);

            await jwtr.WritePropertyNameAsync(nameof(msg.Name));
            await jwtr.WriteValueAsync(msg.Name);

            if (msg.SecsItem != null)
            {
                await jwtr.WritePropertyNameAsync(nameof(msg.SecsItem));
                await msg.SecsItem.WriteToAsync(jwtr);
            }

            await jwtr.WriteEndObjectAsync();
        }

        public static void WriteTo(this Item item, JsonTextWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(item.Format.GetName());

            switch (item.Format)
            {
                case SecsFormat.List:
                    writer.WriteStartArray();
                    foreach (var subitem in item.Items)
                        subitem.WriteTo(writer);
                    writer.WriteEndArray();
                    break;
                case SecsFormat.ASCII:
                case SecsFormat.JIS8:
                    writer.WriteValue(item.GetString());
                    break;
                case SecsFormat.Binary:
                    WriteValue<byte>(writer, item);
                    break;
                case SecsFormat.Boolean:
                    WriteValue<bool>(writer, item);
                    break;
                case SecsFormat.I8:
                    WriteValue<long>(writer, item);
                    break;
                case SecsFormat.I1:
                    WriteValue<sbyte>(writer, item);
                    break;
                case SecsFormat.I2:
                    WriteValue<short>(writer, item);
                    break;
                case SecsFormat.I4:
                    WriteValue<int>(writer, item);
                    break;
                case SecsFormat.F4:
                    WriteValue<float>(writer, item);
                    break;
                case SecsFormat.F8:
                    WriteValue<double>(writer, item);
                    break;
                case SecsFormat.U8:
                    WriteValue<ulong>(writer, item);
                    break;
                case SecsFormat.U1:
                    WriteValue<byte>(writer, item);
                    break;
                case SecsFormat.U2:
                    WriteValue<ushort>(writer, item);
                    break;
                case SecsFormat.U4:
                    WriteValue<uint>(writer, item);
                    break;
            }
            writer.WriteEndObject();

            void WriteValue<T>(JsonTextWriter w, Item i) where T : struct
            {
                w.WriteStartArray();
                foreach (var v in i.GetValues<T>())
                    w.WriteValue(v);
                w.WriteEndArray();
            }
        }

        public static async Task WriteToAsync(this Item item, JsonTextWriter writer)
        {
            await writer.WriteStartObjectAsync();

            await writer.WritePropertyNameAsync(item.Format.GetName());

            await (item.Format == SecsFormat.List ? WriteListAsync(writer, item) : WriteItemValueAsync(writer, item));

            await writer.WriteEndObjectAsync();

            async Task WriteListAsync(JsonTextWriter w, Item i)
            {
                await w.WriteStartArrayAsync();
                foreach (var subitem in i.Items)
                    await subitem.WriteToAsync(w);
                await w.WriteEndArrayAsync();
            }

            Task WriteItemValueAsync(JsonTextWriter w, Item i)
            {
                switch (i.Format)
                {
                    case SecsFormat.ASCII:
                    case SecsFormat.JIS8: return w.WriteValueAsync(i.GetString());
                    case SecsFormat.Binary: return WriteValueAsync<byte>(w, i);
                    case SecsFormat.Boolean: return WriteValueAsync<bool>(w, i);
                    case SecsFormat.I8: return WriteValueAsync<long>(w, i);
                    case SecsFormat.I1: return WriteValueAsync<sbyte>(w, i);
                    case SecsFormat.I2: return WriteValueAsync<short>(w, i);
                    case SecsFormat.I4: return WriteValueAsync<int>(w, i);
                    case SecsFormat.F4: return WriteValueAsync<float>(w, i);
                    case SecsFormat.F8: return WriteValueAsync<double>(w, i);
                    case SecsFormat.U8: return WriteValueAsync<ulong>(w, i);
                    case SecsFormat.U1: return WriteValueAsync<byte>(w, i);
                    case SecsFormat.U2: return WriteValueAsync<ushort>(w, i);
                    case SecsFormat.U4: return WriteValueAsync<uint>(w, i);
                    default: throw new ArgumentOutOfRangeException($"Invalid SecsItem Format: {i.Format}");
                }
            }

            async Task WriteValueAsync<T>(JsonWriter w, Item i)
                where T : struct
            {
                await w.WriteStartArrayAsync();

                foreach (var v in i.GetValues<T>())
                    await w.WriteValueAsync(v);

                await w.WriteEndArrayAsync();
            }
        }

        public static SecsMessage ToSecsMessage(this string jsonString)
            => JObject.Parse(jsonString).ToSecsMessage();

        public static SecsMessage ToSecsMessage(this JObject json)
        {
            var s = json.Value<byte>(nameof(SecsMessage.S));
            var f = json.Value<byte>(nameof(SecsMessage.F));
            var r = json.Value<bool>(nameof(SecsMessage.ReplyExpected));
            var name = json.Value<string>(nameof(SecsMessage.Name));
            var root = json.Value<JObject>(nameof(SecsMessage.SecsItem));
            return (root is null)
                ? new SecsMessage(s, f, r, name)
                : new SecsMessage(s, f, r, name, root.ToItem());
        }

        public static SecsMessage[] ToSecsMessages(this TextReader reader) => JArray.Load(new JsonTextReader(reader))
                                                                                    .Values<JObject>()
                                                                                    .Select(ToSecsMessage)
                                                                                    .ToArray();

        // Define other methods and classes here
        public static Item ToItem(this JObject jobject)
        {
            var json = (JProperty)jobject.First;

            switch (json.Name)
            {
                case nameof(SecsFormat.List): return L(json.Value.Value<JArray>().Values<JObject>().Select(ToItem));
                case nameof(SecsFormat.ASCII): return A(json.Value.Value<string>());
                case nameof(SecsFormat.JIS8): return J(json.Value.Value<string>());
                case nameof(SecsFormat.Binary): return B(json.Value.Values<byte>());
                case nameof(SecsFormat.Boolean): return Boolean(json.Value.Values<bool>());
                case nameof(SecsFormat.F4): return F4(json.Value.Values<float>());
                case nameof(SecsFormat.F8): return F8(json.Value.Values<double>());
                case nameof(SecsFormat.I1): return I1(json.Value.Values<sbyte>());
                case nameof(SecsFormat.I2): return I2(json.Value.Values<short>());
                case nameof(SecsFormat.I4): return I4(json.Value.Values<int>());
                case nameof(SecsFormat.I8): return I8(json.Value.Values<long>());
                case nameof(SecsFormat.U1): return U1(json.Value.Values<byte>());
                case nameof(SecsFormat.U2): return U2(json.Value.Values<ushort>());
                case nameof(SecsFormat.U4): return U4(json.Value.Values<uint>());
                case nameof(SecsFormat.U8): return U8(json.Value.Values<ulong>());
                default: throw new ArgumentOutOfRangeException($"Unknown item format: {json.Name}");
            }
        }
    }
}