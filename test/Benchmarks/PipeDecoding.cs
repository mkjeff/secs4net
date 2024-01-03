using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance.Buffers;
using Secs4Net;
using System;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks;

[Config(typeof(BenchmarkConfig))]
[MemoryDiagnoser]
public class PipeDecoding
{
    private byte[][] _encodedBytes;

    [Params(16, 64, 256)]
    public int InputChunkSize { get; set; }

    [Params(500)]
    public int MessageCount { get; set; }

    [GlobalSetup()]
    public void Setup()
    {
        const int ItemCount = 16;
        using var message = new SecsMessage(s: 1, f: 2, replyExpected: false)
        {
            SecsItem = L(
                L(),
                U1(MemoryOwner<byte>.Allocate(ItemCount)),
                U2(MemoryOwner<ushort>.Allocate(ItemCount)),
                U4(MemoryOwner<uint>.Allocate(ItemCount)),
                F4(MemoryOwner<float>.Allocate(ItemCount)),
                A(CreateString(ItemCount, Encoding.ASCII)),
                J(CreateString(ItemCount, Item.JIS8Encoding)),
                F8(MemoryOwner<double>.Allocate(ItemCount)),
                L(
                    I1(MemoryOwner<sbyte>.Allocate(ItemCount)),
                    I2(MemoryOwner<short>.Allocate(ItemCount)),
                    I4(MemoryOwner<int>.Allocate(ItemCount)),
                    F4(MemoryOwner<float>.Allocate(ItemCount)),
                    L(
                        I1(MemoryOwner<sbyte>.Allocate(ItemCount)),
                        I2(MemoryOwner<short>.Allocate(ItemCount)),
                        I4(MemoryOwner<int>.Allocate(ItemCount)),
                        F4(MemoryOwner<float>.Allocate(ItemCount)),
                        Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                        B(MemoryOwner<byte>.Allocate(ItemCount)),
                        L(
                            A(CreateString(ItemCount, Encoding.ASCII)),
                            J(CreateString(ItemCount, Item.JIS8Encoding)),
                            Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                            B(MemoryOwner<byte>.Allocate(ItemCount))),
                        F8(MemoryOwner<double>.Allocate(ItemCount))),
                    Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                    B(MemoryOwner<byte>.Allocate(ItemCount)),
                    L(
                        A(CreateString(ItemCount, Encoding.ASCII)),
                        J(CreateString(ItemCount, Item.JIS8Encoding)),
                        Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                        B(MemoryOwner<byte>.Allocate(ItemCount))),
                    F8(MemoryOwner<double>.Allocate(ItemCount))),
                U1(MemoryOwner<byte>.Allocate(ItemCount)),
                U2(MemoryOwner<ushort>.Allocate(ItemCount)),
                U4(MemoryOwner<uint>.Allocate(ItemCount)),
                F4(MemoryOwner<float>.Allocate(ItemCount))),
        };

        using var buffer = new ArrayPoolBufferWriter<byte>();

        for (var i = 0; i < MessageCount; i++)
        {
            SecsGem.EncodeMessage(message, 1000 + i, deviceId: 0, buffer);
        }

        _encodedBytes = buffer.WrittenSpan.ToArray().Chunk(InputChunkSize).ToArray();

        static string CreateString(int count, Encoding encoding)
        {
            if (count == 0)
            {
                return string.Empty;
            }
            using var spanOwner = SpanOwner<byte>.Allocate(count);
            return encoding.GetString(spanOwner.Span);
        }
    }

    [Benchmark]
    public async Task Message_Can_Decode_From_Chunked_Sequence()
    {
        var pipe = new Pipe();
        var decoder = new PipeDecoder(pipe.Reader, pipe.Writer);

        using var cts = new CancellationTokenSource();

        var decode = Task.Run(async () =>
        {
            try
            {
                await decoder.StartAsync(cts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) { }
        });

        var input = Task.Run(async () =>
        {
            foreach (var chunk in _encodedBytes)
            {
                await decoder.Input.WriteAsync(chunk).ConfigureAwait(false);
            }
        });

        var output = Task.Run(async () =>
        {
            var count = 1;
            await foreach (var (_, rootItem) in decoder.GetDataMessages(default).ConfigureAwait(false))
            {
                rootItem.Dispose();
                if (count++ == MessageCount)
                {
                    break;
                }
            }
            cts.Cancel();
        });

        await Task.WhenAll(input, output).ConfigureAwait(false);
    }
}
