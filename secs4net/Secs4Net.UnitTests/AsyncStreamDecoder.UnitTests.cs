using FluentAssertions;
using Microsoft.Toolkit.HighPerformance.Buffers;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Secs4Net.Item;

namespace Secs4Net.UnitTests
{
    public class AsyncStreamDecoderUnitTests
    {
        private readonly SecsMessage message = new SecsMessage(s: 1, f: 2, replyExpected: false)
        {
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
            ushort deviceId = 1;
            var systemByte = 1223;

            using var buffer = new ArrayPoolBufferWriter<byte>();
            SecsGem.EncodeMessage(message, systemByte, deviceId, buffer);
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

            var (header, decodeMessage) = await decoder.GetDataMessages(default).FirstAsync();

            header.SystemBytes.Should().Be(systemByte);
            header.DeviceId.Should().Be(deviceId);
            decodeMessage.Should().NotBeNull();
            decodeMessage.Should().BeEquivalentTo(message, options => options.ComparingByMembers<SecsMessage>());
        }

        [Fact]
        public async Task Message_Can_Decode_From_Streaming_And_Equivalent()
        {
            ushort deviceId = 1;
            var systemByte = 1223;            

            using var buffer = new ArrayPoolBufferWriter<byte>();
            SecsGem.EncodeMessage(message, systemByte, deviceId, buffer);
            var encodedBytes = buffer.WrittenMemory;

            var secsGem = Substitute.For<ISecsGem>();
            secsGem.T8.Returns(10000);

            var initialBufferSize = 11;
            var decoder = new AsyncStreamDecoder(initialBufferSize, Substitute.For<ISecsGem>());

            var decoderSource = new MemoryDecoderSource(encodedBytes);
            _ = Task.Run(() =>
            {
                Func<Task> act = ()=> decoder.StartReceivedAsync(decoderSource, default);
                act.Should().NotThrow();
            });

            var (header, decodeMessage) = await decoder.GetDataMessages(default).FirstAsync();

            header.SystemBytes.Should().Be(systemByte);
            header.DeviceId.Should().Be(deviceId);
            decodeMessage.Should().NotBeNull();
            decodeMessage.Should().BeEquivalentTo(message, options => options.ComparingByMembers<SecsMessage>());
        }
    }
}
