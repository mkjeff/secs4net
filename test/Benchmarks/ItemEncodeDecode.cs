using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Linq;
using static Secs4Net.Item;

namespace Secs4Net.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    [MemoryDiagnoser]
    //[NativeMemoryProfiler]
    public class ItemEncodeDecode
    {
        private ArrayPoolBufferWriter<byte> _encodedBuffer;
        private int _estimateEncodedByteLength;
        private Item _item;

        [ParamsAllValues]
        public SecsFormat Format { get; set; }

        [Params(0, 10, 1024)]
        public int Size { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _item = Format switch
            {
                SecsFormat.List => L(Enumerable.Repeat(L(), Size)),
                SecsFormat.Binary => B(CreateArray<byte>(Size)),
                SecsFormat.Boolean => Boolean(CreateArray<bool>(Size)),
                SecsFormat.ASCII => A(CreateString(Size)),
                SecsFormat.JIS8 => J(CreateString(Size)),
                SecsFormat.I8 => I8(CreateArray<long>(Size)),
                SecsFormat.I1 => I1(CreateArray<sbyte>(Size)),
                SecsFormat.I2 => I2(CreateArray<short>(Size)),
                SecsFormat.I4 => I4(CreateArray<int>(Size)),
                SecsFormat.F8 => F8(CreateArray<double>(Size)),
                SecsFormat.F4 => F4(CreateArray<float>(Size)),
                SecsFormat.U8 => U8(CreateArray<ulong>(Size)),
                SecsFormat.U1 => U1(CreateArray<byte>(Size)),
                SecsFormat.U2 => U2(CreateArray<ushort>(Size)),
                SecsFormat.U4 => U4(CreateArray<uint>(Size)),
                _ => throw new ArgumentOutOfRangeException(nameof(Format), Format, "invalid format"),
            };

            _encodedBuffer = new ArrayPoolBufferWriter<byte>();
            _item.EncodeTo(_encodedBuffer);

            _estimateEncodedByteLength = _encodedBuffer.WrittenCount;

            static IMemoryOwner<T> CreateArray<T>(int count) where T : unmanaged => MemoryOwner<T>.Allocate(count);

            static string CreateString(int count)
            {
                using var spanOwner = SpanOwner<char>.Allocate(count);
                return spanOwner.Span.ToString();
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _item.Dispose();
            _encodedBuffer.Dispose();
        }

        [Benchmark]
        public int EncodeTo()
        {
            using var buffer = new ArrayPoolBufferWriter<byte>(_estimateEncodedByteLength);
            _item.EncodeTo(buffer);
            return buffer.WrittenCount;
        }

        [Benchmark]
        public int DecodeFromFullBuffer()
        {
            var seq = new ReadOnlySequence<byte>(_encodedBuffer.WrittenMemory);
            using var item = Item.DecodeFromFullBuffer(ref seq);
            return item.Count;
        }
    }
}
