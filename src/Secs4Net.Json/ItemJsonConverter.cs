using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Secs4Net.Json;

public sealed class ItemJsonConverter : JsonConverter<Item>
{
    public override Item? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonElement.TryParseValue(ref reader, out var element)
            ? element.GetValueOrDefault().ToItem()
            : null;
    }

    public override void Write(Utf8JsonWriter writer, Item item, JsonSerializerOptions options)
    {
        item.WriteTo(writer);
    }
}
