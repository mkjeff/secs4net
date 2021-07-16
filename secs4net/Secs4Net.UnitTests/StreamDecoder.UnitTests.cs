using FluentAssertions;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Linq;
using Xunit;
using static Secs4Net.Item;

namespace Secs4Net.UnitTests
{
    public class StreamDecoderUnitTests
    {
        [Fact(Skip ="StreamDecoder is hard to test. will change the implementation")]
        public void Item_Encode_Decode_Should_Be_Equivalent()
        {
            ushort deviceId = 1;
            var systemByte = 1223;
            var message = new SecsMessage(s: 1, f: 2, replyExpected: false)
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

            using var buffer = new ArrayPoolBufferWriter<byte>();
            message.EncodeTo(buffer, deviceId, systemByte);
            var DecoderBufferSize = 11;
            var dataTransaferChunkSize = 10;
            SecsMessage decodeMessage = null;
            var decoder = new StreamDecoder(DecoderBufferSize, delegate { }, (header, msg) =>
            {
                decodeMessage = msg;
            });

            var encodedBytes = buffer.WrittenSpan.ToArray();
            foreach (var chuck in encodedBytes.Chunk(dataTransaferChunkSize))
            {
                var bytesToDecode = chuck.ToArray();
                Buffer.BlockCopy(bytesToDecode, 0, decoder.Buffer, decoder.BufferOffset, bytesToDecode.Length);

                if (decoder.Decode(dataTransaferChunkSize) == false)
                {
                    break;
                }
            }

            decodeMessage.Should().NotBeNull();
            decodeMessage.Should().BeEquivalentTo(message, options => options.ComparingByMembers<SecsMessage>());
        }
    }
}
