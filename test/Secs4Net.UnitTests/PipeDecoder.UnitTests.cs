using CommunityToolkit.HighPerformance.Buffers;
using FluentAssertions;
using System;
using System.IO.Pipelines;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Secs4Net.UnitTests;

public class PipeDecoderUnitTests
{
    private readonly SecsMessage message = new(s: 1, f: 2, replyExpected: false)
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
    public void Message_Equals_Should_Be_True()
    {
        var subject = new SecsMessage(s: 1, f: 2, replyExpected: false)
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

        subject.Should().BeEquivalentTo(message);
    }

    [Fact]
    public async Task Message_Can_Decode_From_Full_Buffer()
    {
        var messageIds = Enumerable.Range(start: 10001, count: 4).ToArray();
        using var buffer = new ArrayPoolBufferWriter<byte>();

        foreach (var id in messageIds)
        {
            SecsGem.EncodeMessage(message, id, deviceId: 0, buffer);
        }
        var encodedBytes = buffer.WrittenMemory;

        var pipe = new Pipe();
        var decoder = new PipeDecoder(pipe.Reader, pipe.Writer);

        _ = Task.Run(() =>
        {
            Func<Task> act = () => decoder.StartAsync(default);
            act.Should().NotThrowAsync();
        });

        await decoder.Input.WriteAsync(encodedBytes);

        var decodeMessages = await decoder.GetDataMessages(default)
            .Take(messageIds.Length)
            .Select(m => new
            {
                m.header.Id,
                Message = new SecsMessage(m.header.S, m.header.F, m.header.ReplyExpected)
                {
                    SecsItem = m.rootItem,
                },
            })
            .ToListAsync();

        foreach (var (id, index) in messageIds.Select((a, index) => (a, index)))
        {
            decodeMessages[index].Id.Should().Be(id);
            decodeMessages[index].Message.Should().BeEquivalentTo(message);
        }
    }

    [Fact]
    public async Task Message_Can_Decode_From_Chunked_Sequence()
    {
        var messageIds = Enumerable.Range(start: 10001, count: 4).ToArray();
        using var buffer = new ArrayPoolBufferWriter<byte>();

        foreach (var id in messageIds)
        {
            SecsGem.EncodeMessage(message, id, deviceId: 0, buffer);
        }
        var encodedBytes = buffer.WrittenMemory;

        var pipe = new Pipe();
        var decoder = new PipeDecoder(pipe.Reader, pipe.Writer);

        _ = Task.Run(() =>
        {
            Func<Task> act = () => decoder.StartAsync(default);
            act.Should().NotThrowAsync();
        });

        _ = Task.Run(async () =>
        {
            var random = Random.Shared;
            foreach (var chunk in new ChunkedReadOnlyMemory<byte>(encodedBytes, size: 23))
            {
                await Task.Delay(200); //simulate a slow connection

                if (random.Next() % 2 == 0)
                {
                    await decoder.Input.WriteAsync(ReadOnlyMemory<byte>.Empty);
                }

                await decoder.Input.WriteAsync(chunk);
            }
        });

        var decodeMessages = await decoder.GetDataMessages(default)
            .Take(messageIds.Length)
            .Select(m => new
            {
                m.header.Id,
                Message = new SecsMessage(m.header.S, m.header.F, m.header.ReplyExpected)
                {
                    SecsItem = m.rootItem,
                },
            })
            .ToListAsync();

        foreach (var (id, index) in messageIds.Select((a, index) => (a, index)))
        {
            decodeMessages[index].Id.Should().Be(id);
            decodeMessages[index].Message.Should().BeEquivalentTo(message);
        }
    }

    [Fact]
    public async Task Control_Message_With_Or_Without_Body_Should_Route_To_Control_Channel()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var ct = cts.Token;

        var pipe = new Pipe();
        var decoder = new PipeDecoder(pipe.Reader, pipe.Writer);

        _ = Task.Run(() => decoder.StartAsync(ct));

        // Select.req: header-only control message (no body)
        // Select.rsp: carries a 4-byte status body per E37, written in two chunks to
        //             exercise the fragmented-body path
        // S1F2: a regular data message that must still decode after the control frames
        var selectReq = EncodeControlMessage(MessageType.SelectRequest, id: 0x11111111);
        var selectRsp = EncodeControlMessage(MessageType.SelectResponse, id: 0x22222222, body: new byte[] { 0, 0, 0, 0 });
        var dataMessageId = 33333;
        using var dataBuffer = new CommunityToolkit.HighPerformance.Buffers.ArrayPoolBufferWriter<byte>();
        SecsGem.EncodeMessage(message, dataMessageId, deviceId: 0, dataBuffer);

        await decoder.Input.WriteAsync(selectReq, ct);
        await decoder.Input.WriteAsync(selectRsp[..14], ct);
        await Task.Yield();
        await decoder.Input.WriteAsync(selectRsp[14..], ct);
        await decoder.Input.WriteAsync(dataBuffer.WrittenMemory, ct);

        var controlMessages = await decoder.GetControlMessages(ct).Take(2).ToListAsync(ct);
        controlMessages.Should().HaveCount(2);
        controlMessages[0].MessageType.Should().Be(MessageType.SelectRequest);
        controlMessages[0].Id.Should().Be(0x11111111);
        controlMessages[1].MessageType.Should().Be(MessageType.SelectResponse);
        controlMessages[1].Id.Should().Be(0x22222222);

        var dataMessages = await decoder.GetDataMessages(ct).Take(1).ToListAsync(ct);
        dataMessages.Should().HaveCount(1);
        dataMessages[0].header.Id.Should().Be(dataMessageId);
        dataMessages[0].header.S.Should().Be(message.S);
        dataMessages[0].header.F.Should().Be(message.F);
    }

    private static byte[] EncodeControlMessage(MessageType type, int id, byte[]? body = null)
    {
        body ??= Array.Empty<byte>();
        var bytes = new byte[4 + 10 + body.Length];
        System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(bytes, (uint)(10 + body.Length));
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16BigEndian(bytes.AsSpan(4), 0xFFFF); // DeviceId
        bytes[6] = 0; // W-bit / stream
        bytes[7] = 0; // function
        bytes[8] = 0; // PType
        bytes[9] = (byte)type;
        System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(bytes.AsSpan(10), id);
        body.CopyTo(bytes, 14);
        return bytes;
    }
}
