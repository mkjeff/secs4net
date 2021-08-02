using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net;
using System;
using System.Buffers;
using static Secs4Net.Item;

namespace Secs4Netb.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    [MemoryDiagnoser]
    //[NativeMemoryProfiler]
    public class ComplexItemDecode
    {
        private byte[] _encodedBytes;

        [Params(0, 10, 1025)]
        public int ItemCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            using var item =
                 L(
                     L(),
                     U1(MemoryOwner<byte>.Allocate(ItemCount)),
                     U2(MemoryOwner<ushort>.Allocate(ItemCount)),
                     U4(MemoryOwner<uint>.Allocate(ItemCount)),
                     F4(MemoryOwner<float>.Allocate(ItemCount)),
                     A(CreateString(Math.Min(ItemCount, 512))),
                     J(CreateString(Math.Min(ItemCount, 512))),
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
                                 J(CreateString(Math.Min(ItemCount, 512))),
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
                     F4(MemoryOwner<float>.Allocate(ItemCount)));

            using var buffer = new ArrayPoolBufferWriter<byte>();
            item.EncodeTo(buffer);
            _encodedBytes = buffer.WrittenMemory.ToArray();

            static string CreateString(int count)
            {
                using var spanOwner = SpanOwner<char>.Allocate(count);
                return spanOwner.Span.ToString();
            }
        }

        [Benchmark]
        public long Decode()
        {
            var source = new ReadOnlySequence<byte>(_encodedBytes);
            using var item = Item.DecodeFromFullBuffer(ref source);
            return source.Length;
        }
    }
}
