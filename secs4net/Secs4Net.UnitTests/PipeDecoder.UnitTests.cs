using FluentAssertions;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net.UnitTests.Extensions;
using System;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Secs4Net.Item;

namespace Secs4Net.UnitTests
{
    public class PipeDecoderUnitTests
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
        public void Message_Equals_Should_Be_True()
        {
            var subject = new SecsMessage(s: 1, f: 2, replyExpected: false)
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

            subject.Should().BeEquivalentTo(message);
        }

        [Fact]
        public async Task Message_Can_Decode_From_Full_Buffer_And_Equivalent()
        {
            using var buffer = new ArrayPoolBufferWriter<byte>();
            SecsGem.EncodeMessage(message, buffer);
            var encodedBytes = buffer.WrittenMemory;

            var pipe = new Pipe();
            var decoder = new PipeDecoder(pipe.Reader, pipe.Writer);
            await decoder.Input.WriteAsync(encodedBytes);

            _ = Task.Run(() =>
            {
                Func<Task> act = () => decoder.StartAsync(default);
                act.Should().NotThrow();
            });

            var decodeMessage = await decoder.GetDataMessages(default).FirstAsync();

            decodeMessage.Id.Should().Be(systemByte);
            decodeMessage.DeviceId.Should().Be(deviceId);
            decodeMessage.Should().NotBeNull();
            decodeMessage.Should().BeEquivalentTo(message);
        }

        [Fact]
        public async Task Message_Can_Decode_From_Streaming_And_Equivalent()
        {
            using var buffer = new ArrayPoolBufferWriter<byte>();
            SecsGem.EncodeMessage(message, buffer);
            var encodedBytes = buffer.WrittenMemory.ToArray();

            var pipe = new Pipe();
            var decoder = new PipeDecoder(pipe.Reader, pipe.Writer);

            _ = Task.Run(async () =>
            {
                foreach (var chunk in encodedBytes.Chunk(11))
                {
                    await Task.Delay(1000); //simulate slow producer
                    await decoder.Input.WriteAsync(chunk);
                }
            });

            _ = Task.Run(() =>
            {
                Func<Task> act = () => decoder.StartAsync(default);
                act.Should().NotThrow();
            });

            var decodeMessage = await decoder.GetDataMessages(default).FirstAsync();

            decodeMessage.Id.Should().Be(systemByte);
            decodeMessage.DeviceId.Should().Be(deviceId);
            decodeMessage.Should().NotBeNull();
            Assert.Equal(message, decodeMessage);
        }
    }
}
