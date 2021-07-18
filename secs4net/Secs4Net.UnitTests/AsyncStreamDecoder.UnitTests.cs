using FluentAssertions;
using Microsoft.Toolkit.HighPerformance.Buffers;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Secs4Net.Item;

namespace Secs4Net.UnitTests
{
    public class AsyncStreamDecoderUnitTests
    {
        private const ushort deviceId = 1;
        private const int systemByte = 1223;

        private readonly SecsMessage message = new SecsMessage(s: 1, f: 2, replyExpected: false)
        {
            DeviceId = deviceId,
            Id = systemByte,
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
        public async Task Message_Can_Decode_From_Full_Buffer_And_Equivalent()
        {

            using var buffer = new ArrayPoolBufferWriter<byte>();
            SecsGem.EncodeMessage(message, buffer);
            var encodedBytes = buffer.WrittenMemory;

            var secsGem = Substitute.For<ISecsGem>();
            secsGem.T8.Returns(10000);

            var decoder = new AsyncStreamDecoder(initialBufferSize: encodedBytes.Length, Substitute.For<ISecsGem>());

            var decoderSource = new MemoryDecoderSource(encodedBytes);

            _ = Task.Run(() =>
            {
                Func<Task> act = () => decoder.StartReceivedAsync(decoderSource, default);
                act.Should().NotThrow();
            });

            var decodeMessage = await decoder.GetDataMessages(default).FirstAsync();

            decodeMessage.Id.Should().Be(systemByte);
            decodeMessage.DeviceId.Should().Be(deviceId);
            decodeMessage.Should().NotBeNull();
            decodeMessage.Should().BeEquivalentTo(message, options => options.ComparingByMembers<SecsMessage>());
        }

        [Fact]
        public async Task Message_Can_Decode_From_Streaming_And_Equivalent()
        {
            using var buffer = new ArrayPoolBufferWriter<byte>();
            SecsGem.EncodeMessage(message, buffer);
            var encodedBytes = buffer.WrittenMemory;

            var secsGem = Substitute.For<ISecsGem>();
            secsGem.T8.Returns(10000);

            var initialBufferSize = 11;
            var decoder = new AsyncStreamDecoder(initialBufferSize, Substitute.For<ISecsGem>());

            var decoderSource = new MemoryDecoderSource(encodedBytes);
            _ = Task.Run(() =>
            {
                Func<Task> act = () => decoder.StartReceivedAsync(decoderSource, default);
                act.Should().NotThrow();
            });

            var decodeMessage = await decoder.GetDataMessages(default).FirstAsync();

            decodeMessage.Id.Should().Be(systemByte);
            decodeMessage.DeviceId.Should().Be(deviceId);
            decodeMessage.Should().NotBeNull();
            decodeMessage.Should().BeEquivalentTo(message, options => options.ComparingByMembers<SecsMessage>());
        }
    }
}
