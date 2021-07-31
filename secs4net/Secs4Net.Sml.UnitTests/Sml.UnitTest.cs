using FluentAssertions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Secs4Net.Item;

namespace Secs4Net.Sml.UnitTests
{
    public class SmlUnitTest
    {
        private readonly SecsMessage message = new(s: 1, f: 2, replyExpected: false)
        {
            Name = "Test",
            SecsItem =
                L(
                    L(),
                    U1(122, 34),
                    U2(34531, 23123),
                    U4(2123513, 52451141),
                    F4(23123.21323f, 2324.221f), //net472 has precision issue with Single.ToString()
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
        public void SecsMessage_Can_Serialize_And_Deserialize()
        {
            var sml = message.ToSml();
            var deserialized = sml.ToSecsMessage();

            deserialized.Should().BeEquivalentTo(message);
        }

        [Fact]
        public async Task Multiple_SecsMessage_Can_Serialize_And_Deserialize_From_Stream()
        {
            var messages = Enumerable.Repeat(message, 5).ToList();

            var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            foreach (var m in messages)
            {
                m.WriteSmlTo(writer);
            }
            writer.Flush();

            memoryStream.Position = 0;
            

            using var reader = new StreamReader(memoryStream);
            var i = 0;
            await foreach (var m in reader.ToSecsMessages())
            {
                m.Should().BeEquivalentTo(messages[i]);
                i++;
            }

            i.Should().Be(messages.Count);
        }
    }
}
