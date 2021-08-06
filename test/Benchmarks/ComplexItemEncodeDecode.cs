using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using static Secs4Net.Item;

namespace Secs4Net.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    [MemoryDiagnoser]
    //[NativeMemoryProfiler]
    public class ComplexItemEncodeDecode
    {
        private Item _item;
        private byte[] _encodedBytes;

        [Params(0, 64, 128)]
        public int ItemCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _item =
                 L(
                     L(),
                     U1(MemoryOwner<byte>.Allocate(ItemCount)),
                     U2(MemoryOwner<ushort>.Allocate(ItemCount)),
                     U4(MemoryOwner<uint>.Allocate(ItemCount)),
                     F4(MemoryOwner<float>.Allocate(ItemCount)),
                     A(CreateString(Math.Min(ItemCount, 512))),
                     //J(CreateString(Math.Min(ItemCount, 512))), //JIS encoding cost more memory in coreclr
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
                             //J(CreateString(Math.Min(ItemCount, 512))),
                             Boolean(MemoryOwner<bool>.Allocate(ItemCount)),
                             B(MemoryOwner<byte>.Allocate(ItemCount))),
                         F8(MemoryOwner<double>.Allocate(ItemCount))),
                     U1(MemoryOwner<byte>.Allocate(ItemCount)),
                     U2(MemoryOwner<ushort>.Allocate(ItemCount)),
                     U4(MemoryOwner<uint>.Allocate(ItemCount)),
                     F4(MemoryOwner<float>.Allocate(ItemCount)));

            using var buffer = new ArrayPoolBufferWriter<byte>();
            _item.EncodeTo(buffer);
            _encodedBytes = buffer.WrittenMemory.ToArray();

            static string CreateString(int count)
            {
                using var spanOwner = SpanOwner<char>.Allocate(count);
                return spanOwner.Span.ToString();
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _item.Dispose();
        }

        [Benchmark]
        public int Encode()
        {
            using var buffer = new ArrayPoolBufferWriter<byte>(_encodedBytes.Length);
            _item.EncodeTo(buffer);
            return buffer.WrittenCount;
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
