using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net.Sml;
using System;
using System.IO;
using static Secs4Net.Item;

namespace Secs4Net.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    [MemoryDiagnoser]
    //[NativeMemoryProfiler]
    public class SmlSerialization
    {
        private SecsMessage _message;
        private string _sml;

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
                     A(CreateString(Math.Min(ItemCount, 512))),
                     J(CreateString(Math.Min(ItemCount, 512))), //JIS encoding cost more memory in coreclr
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
                                 A(CreateString(Math.Min(ItemCount, 512))),
                                 //J(CreateString(Math.Min(ItemCount, 512))),
                                 Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                                 B(MemoryOwner<byte>.Allocate(ItemCount))),
                             F8(MemoryOwner<double>.Allocate(ItemCount))),
                         Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                         B(MemoryOwner<byte>.Allocate(ItemCount)),
                         L(
                             A(CreateString(Math.Min(ItemCount, 512))),
                             J(CreateString(Math.Min(ItemCount, 512))),
                             Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                             B(MemoryOwner<byte>.Allocate(ItemCount))),
                         F8(MemoryOwner<double>.Allocate(ItemCount))),
                     U1(MemoryOwner<byte>.Allocate(ItemCount)),
                     U2(MemoryOwner<ushort>.Allocate(ItemCount)),
                     U4(MemoryOwner<uint>.Allocate(ItemCount)),
                     F4(MemoryOwner<float>.Allocate(ItemCount))),
            };

            _sml = _message.ToSml();

            static string CreateString(int count)
            {
                using var spanOwner = SpanOwner<char>.Allocate(count);
                return spanOwner.Span.ToString();
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _message.Dispose();
        }

        [Benchmark]
        public string Serialize()
        {
            using var sw = new StringWriter();
            _message.WriteSmlTo(sw);
            sw.Flush();
            return _sml;
        }

        [Benchmark]
        public SecsMessage Deserialze()
            => _sml.ToSecsMessage();
    }
}
