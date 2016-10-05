using System;
using System.IO;
using System.Linq;
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

        public static void WriteTo(this SecsMessage msg, TextWriter writer)
        {
            using (var jwtr = new JsonTextWriter(writer))
                msg.WriteTo(jwtr);
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

        public static void WriteTo(this Item item, JsonTextWriter writer)
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
                writer.WritePropertyName(nameof(item.Values));

                if (item.Format == SecsFormat.ASCII || item.Format == SecsFormat.JIS8)
                {
                    writer.WriteValue(item.GetValue<string>());
                }
                else
                {
                    writer.WriteStartArray();
                    foreach (var value in item.Values)
                        writer.WriteValue(value);
                    writer.WriteEndArray();
                }
            }
            writer.WriteEndObject();
        }

        public static SecsMessage ToSecsMessage(this string jsonString)
            => JObject.Parse(jsonString).ToSecsMessage();

        public static SecsMessage ToSecsMessage(this JObject json)
        {
            var msg = default(SecsMessage);
            var s = json.Value<byte>(nameof(msg.S));
            var f = json.Value<byte>(nameof(msg.F));
            var r = json.Value<bool>(nameof(msg.ReplyExpected));
            var name = json.Value<string>(nameof(msg.Name));
            var root = json.Value<JObject>(nameof(msg.SecsItem));
            return (root == null)
                ? new SecsMessage(s, f, r, name)
                : new SecsMessage(s, f, r, name, root.ToItem());
        }

        public static SecsMessage[] ToSecsMessages(this TextReader reader)
        {
            var arr = JToken.ReadFrom(new JsonTextReader(reader));
            var query =
                from a in arr.Value<JArray>().Values<JObject>()
                select a.ToSecsMessage();
            return query.ToArray();
        }

        // Define other methods and classes here
        public static Item ToItem(this JObject json)
        {
            var item = default(Item);
            var format = json.Value<JValue>(nameof(item.Format)).ToObject<SecsFormat>();
            if (format == SecsFormat.List)
            {
                return L(from a in json.Value<JArray>(nameof(item.Items)).Values<JObject>()
                         select a.ToItem());
            }

            if (format == SecsFormat.ASCII || format == SecsFormat.JIS8)
            {
                var str = json.Value<string>(nameof(item.Values));
                return format == SecsFormat.ASCII ? A(str) : J(str);
            }

            var values = json.Value<JArray>(nameof(item.Values));
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
