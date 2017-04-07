using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Secs4Net.Item;
using System.Threading.Tasks;

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

        public static void WriteTo(this SecsMessage msg, TextWriter writer)
        {
            using (var jwtr = new JsonTextWriter(writer))
                msg.WriteTo(jwtr);
        }

        public static async Task WriteToAsync(this SecsMessage msg, TextWriter writer)
        {
            using (var jwtr = new JsonTextWriter(writer))
                await msg.WriteToAsync(jwtr);
        }

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

            jwtr.Flush();
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

            await jwtr.FlushAsync();
        }

        const string ItemValuesName = "Values";

        public static void WriteTo(this SecsItem item, JsonTextWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(item.Format));
            writer.WriteValue(item.Format.GetName());
            if (item.Format == SecsFormat.List)
            {
                writer.WritePropertyName(nameof(item.Items));
                writer.WriteStartArray();
                foreach (var subitem in item.Items)
                    subitem.WriteTo(writer);
                writer.WriteEndArray();
            }
            else
            {
                writer.WritePropertyName(ItemValuesName);
                switch (item.Format)
                {
                    case SecsFormat.ASCII:
                    case SecsFormat.JIS8:
                        writer.WriteValue(item.GetString());
                        break;
                    case SecsFormat.Binary:
                        WriteValue<byte>();
                        break;
                    case SecsFormat.Boolean:
                        WriteValue<bool>();
                        break;
                    case SecsFormat.I8:
                        WriteValue<long>();
                        break;
                    case SecsFormat.I1:
                        WriteValue<sbyte>();
                        break;
                    case SecsFormat.I2:
                        WriteValue<short>();
                        break;
                    case SecsFormat.I4:
                        WriteValue<int>();
                        break;
                    case SecsFormat.F4:
                        WriteValue<float>();
                        break;
                    case SecsFormat.F8:
                        WriteValue<double>();
                        break;
                    case SecsFormat.U8:
                        WriteValue<ulong>();
                        break;
                    case SecsFormat.U1:
                        WriteValue<byte>();
                        break;
                    case SecsFormat.U2:
                        WriteValue<ushort>();
                        break;
                    case SecsFormat.U4:
                        WriteValue<uint>();
                        break;
                }
            }
            writer.WriteEndObject();

            void WriteValue<T>() where T:struct
            {
                writer.WriteStartArray();

                foreach (var v in item.GetValues<T>())
                    writer.WriteValue(v);
                writer.WriteEndArray();
            }
        }

        public static async Task WriteToAsync(this SecsItem item, JsonTextWriter writer)
        {
            await writer.WriteStartObjectAsync();

            await writer.WritePropertyNameAsync(nameof(item.Format));
            await writer.WriteValueAsync(item.Format.GetName());

            if (item.Format == SecsFormat.List)
            {
                await WriteListAsync();
            }
            else
            {
                await writer.WritePropertyNameAsync(ItemValuesName);
                await WriteItemValueAsync();
            }

            await writer.WriteEndObjectAsync();

            async Task WriteListAsync()
            {
                await writer.WritePropertyNameAsync(nameof(item.Items));
                await writer.WriteStartArrayAsync();
                foreach (var subitem in item.Items)
                    await subitem.WriteToAsync(writer);
                await writer.WriteEndArrayAsync();
            }

            Task WriteItemValueAsync()
            {
                switch (item.Format)
                {
                    case SecsFormat.ASCII:
                    case SecsFormat.JIS8:
                        return writer.WriteValueAsync(item.GetString());
                    case SecsFormat.Binary:
                        return WriteValueAsync<byte>(writer,item);
                    case SecsFormat.Boolean:
                        return WriteValueAsync<bool>(writer, item);
                    case SecsFormat.I8:
                        return WriteValueAsync<long>(writer, item);
                    case SecsFormat.I1:
                        return WriteValueAsync<sbyte>(writer, item);
                    case SecsFormat.I2:
                        return WriteValueAsync<short>(writer, item);
                    case SecsFormat.I4:
                        return WriteValueAsync<int>(writer, item);
                    case SecsFormat.F4:
                        return WriteValueAsync<float>(writer, item);
                    case SecsFormat.F8:
                        return WriteValueAsync<double>(writer, item);
                    case SecsFormat.U8:
                        return WriteValueAsync<ulong>(writer, item);
                    case SecsFormat.U1:
                        return WriteValueAsync<byte>(writer, item);
                    case SecsFormat.U2:
                        return WriteValueAsync<ushort>(writer, item);
                    default:
                        // case SecsFormat.U4:
                        return WriteValueAsync<uint>(writer, item);
                }

                async Task WriteValueAsync<T>(JsonWriter w,SecsItem i)
                    where T : struct
                {
                    await w.WriteStartArrayAsync();

                    foreach (var v in i.GetValues<T>())
                        await w.WriteValueAsync(v);

                    await w.WriteEndArrayAsync();
                }
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

        public static SecsMessage[] ToSecsMessages(this TextReader reader)
        {
            var arr = JArray.Load(new JsonTextReader(reader));
            var query =
                from a in arr.Values<JObject>()
                select a.ToSecsMessage();
            return query.ToArray();
        }

        // Define other methods and classes here
        public static SecsItem ToItem(this JObject json)
        {
            var format = json.Value<JValue>(nameof(SecsItem.Format)).ToObject<SecsFormat>();
            if (format == SecsFormat.List)
            {
                return L(from a in json.Value<JArray>(nameof(SecsItem.Items)).Values<JObject>()
                         select a.ToItem());
            }

            if (format == SecsFormat.ASCII || format == SecsFormat.JIS8)
            {
                var str = json.Value<string>(ItemValuesName);
                return format == SecsFormat.ASCII ? A(str) : J(str);
            }

            var values = json.Value<JArray>(ItemValuesName);
            switch (format)
            {
                case SecsFormat.Binary: return B(values.Values<byte>());
                case SecsFormat.Boolean: return Boolean(values.Values<bool>());
                case SecsFormat.F4: return F4(values.Values<float>());
                case SecsFormat.F8: return F8(values.Values<double>());
                case SecsFormat.I1: return I1(values.Values<sbyte>());
                case SecsFormat.I2: return I2(values.Values<short>());
                case SecsFormat.I4: return I4(values.Values<int>());
                case SecsFormat.I8: return I8(values.Values<long>());
                case SecsFormat.U1: return U1(values.Values<byte>());
                case SecsFormat.U2: return U2(values.Values<ushort>());
                case SecsFormat.U4: return U4(values.Values<uint>());
                case SecsFormat.U8: return U8(values.Values<ulong>());
                default: throw new ArgumentException("unknown exception");
            }
        }
    }
}
