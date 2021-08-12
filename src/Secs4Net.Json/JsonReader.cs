using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static Secs4Net.Item;

namespace Secs4Net.Json;

public static class JsonReader
{
    public static SecsMessage ToSecsMessage(this string jsonString)
        => JsonDocument.Parse(jsonString).ToSecsMessage();

    public static SecsMessage ToSecsMessage(this JsonDocument jsonDocument)
    {
        var json = jsonDocument.RootElement;
        var s = json.GetProperty(nameof(SecsMessage.S)).GetByte();
        var f = json.GetProperty(nameof(SecsMessage.F)).GetByte();
        var replyExpected = json.GetProperty(nameof(SecsMessage.ReplyExpected)).GetBoolean();
        var name = json.GetProperty(nameof(SecsMessage.Name)).GetString();
        var item = json.TryGetProperty(nameof(SecsMessage.SecsItem), out var root)
            ? root.ToItem()
            : null;

        return new SecsMessage(s, f, replyExpected)
        {
            Name = name,
            SecsItem = item,
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Item ToItem(this JsonElement jobject)
    {
        var json = jobject.EnumerateObject().First();
        return json.Name switch
        {
            nameof(SecsFormat.List) => L(json.Value.EnumerateArray().Select(static a => a.ToItem())),
            nameof(SecsFormat.ASCII) => A(json.Value.GetString()),
            nameof(SecsFormat.JIS8) => J(json.Value.GetString()),
            nameof(SecsFormat.Binary) => B(json.Value.EnumerateArray().Select(static a => a.GetByte())),
            nameof(SecsFormat.Boolean) => Boolean(json.Value.EnumerateArray().Select(static a => a.GetBoolean())),
            nameof(SecsFormat.F4) => F4(json.Value.EnumerateArray().Select(static a => a.GetSingle())),
            nameof(SecsFormat.F8) => F8(json.Value.EnumerateArray().Select(static a => a.GetDouble())),
            nameof(SecsFormat.I1) => I1(json.Value.EnumerateArray().Select(static a => a.GetSByte())),
            nameof(SecsFormat.I2) => I2(json.Value.EnumerateArray().Select(static a => a.GetInt16())),
            nameof(SecsFormat.I4) => I4(json.Value.EnumerateArray().Select(static a => a.GetInt32())),
            nameof(SecsFormat.I8) => I8(json.Value.EnumerateArray().Select(static a => a.GetInt64())),
            nameof(SecsFormat.U1) => U1(json.Value.EnumerateArray().Select(static a => a.GetByte())),
            nameof(SecsFormat.U2) => U2(json.Value.EnumerateArray().Select(static a => a.GetUInt16())),
            nameof(SecsFormat.U4) => U4(json.Value.EnumerateArray().Select(static a => a.GetUInt32())),
            nameof(SecsFormat.U8) => U8(json.Value.EnumerateArray().Select(static a => a.GetUInt64())),
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
