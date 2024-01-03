using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance.Buffers;
using Secs4Net;
using Secs4Net.Json;
using System.Text;
using System.Text.Json;

namespace Benchmarks;

[Config(typeof(BenchmarkConfig))]
[MemoryDiagnoser(displayGenColumns: false)]
//[NativeMemoryProfiler]
public class JsonSerialization
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new ItemJsonConverter() },
    };

    private SecsMessage _message;
    private string _json;

    [Params(0, 64, 128)]
    public int ItemCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _message = new(s: 1, f: 2, replyExpected: false)
        {
            Name = "Test",
            SecsItem = L(
                 L(),
                 U1(MemoryOwner<byte>.Allocate(ItemCount)),
                 U2(MemoryOwner<ushort>.Allocate(ItemCount)),
                 U4(MemoryOwner<uint>.Allocate(ItemCount)),
                 F4(MemoryOwner<float>.Allocate(ItemCount)),
                 A(CreateString(ItemCount, Encoding.ASCII)),
                 J(CreateString(ItemCount, Item.JIS8Encoding)), //JIS encoding cost more memory in coreclr
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

        _json = JsonSerializer.Serialize(_message, JsonSerializerOptions);

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

    [GlobalCleanup]
    public void Cleanup()
    {
        _message.Dispose();
    }

    [Benchmark]
    public string Serialize()
        => JsonSerializer.Serialize(_message, JsonSerializerOptions);

    [Benchmark]
    public SecsMessage Deserialze()
        => JsonSerializer.Deserialize<SecsMessage>(_json, JsonSerializerOptions);
}
