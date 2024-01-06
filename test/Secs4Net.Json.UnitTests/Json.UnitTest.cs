using FluentAssertions;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Secs4Net.Json.UnitTests;

public class JsonUnitTest
{
    private readonly SecsMessage EmptyMessage = new(s: 1, f: 2, replyExpected: false);
    private readonly SecsMessage message = new(s: 1, f: 2, replyExpected: false)
    {
        Name = "Test",
        SecsItem =
            L(
                L(),
                U1(122, 34),
                U2(34531, 23123),
                U4(2123513, 52451141),
                F4(23123.21323f, 2324.221f),
                A("A string"),
                J("sdsad"),
                F8(231.00002321d, 0.2913212312d),
                L(
                    U1(122, 34),
                    U2(34531, 23123),
                    U4(2123513, 52451141),
                    F4(23123.21323f, 2324.221f),
                    Boolean(true, false, false, true),
                    B(0x1C, 0x01, 0xFF),
                    L(
                        A("A string"),
                        J("sdsad"),
                        Boolean(true, false, false, true),
                        B(0x1C, 0x01, 0xFF)),
                    F8(231.00002321d, 0.2913212312d)))
    };

    [Fact]
    public void SecsMessage_Can_Serialize_And_Deserialize_With_JsonConverter()
    {
        var options = new JsonSerializerOptions
        {
            Converters = {
                new ItemJsonConverter()
            }
        };

        var sml = JsonSerializer.Serialize(message, options);
        var deserialized = JsonSerializer.Deserialize<SecsMessage>(sml, options);

        deserialized.Should().NotBeNull().And.BeEquivalentTo(message);
    }

    [Fact]
    public void Empty_SecsMessage_Can_Serialize_And_Deserialize_With_JsonConverter()
    {
        var options = new JsonSerializerOptions
        {
            Converters = {
                new ItemJsonConverter()
            }
        };

        var sml = JsonSerializer.Serialize(EmptyMessage, options);
        var deserialized = JsonSerializer.Deserialize<SecsMessage>(sml, options);

        deserialized.Should().NotBeNull().And.BeEquivalentTo(EmptyMessage);
        deserialized!.SecsItem.Should().BeNull();
    }

    [Fact]
    public async Task Multiple_SecsMessage_Can_Serialize_And_Deserialize_From_Stream()
    {
        var options = new JsonSerializerOptions
        {
            Converters = {
                new ItemJsonConverter()
            }
        };

        var messages = Enumerable.Repeat(message, 5).ToList();

        var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, messages, options);
        await memoryStream.FlushAsync();
        memoryStream.Position = 0;

        var i = 0;
        await foreach (var m in JsonSerializer.DeserializeAsyncEnumerable<SecsMessage>(memoryStream, options))
        {
            m.Should().BeEquivalentTo(messages[i]);
            i++;
        }

        i.Should().Be(messages.Count);
    }
}
