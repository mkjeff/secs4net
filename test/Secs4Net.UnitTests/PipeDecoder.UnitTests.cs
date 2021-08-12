using FluentAssertions;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net;
using Secs4Net.Extensions;
using System;
using System.IO.Pipelines;
using System.Linq;
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
            foreach (var chunk in encodedBytes.Chunk(23))
            {
                await Task.Delay(200); //simulate a slow connection
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
}
