using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net;
using System;
using System.Buffers;
using System.Linq;
using System.Runtime.CompilerServices;
using static Secs4Net.Item;
namespace Secs4Netb.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    [MemoryDiagnoser]
    //[NativeMemoryProfiler]
    public class ItemEncode
    {
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

            _estimateEncodedByteLength = Encode(initalBufferCount: 1024, _item);

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
        }

        [Benchmark]
        public int EncodeTo()
        {
            return Encode(_estimateEncodedByteLength, _item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Encode(int initalBufferCount, Item item)
        {
            using var buffer = new ArrayPoolBufferWriter<byte>(initalBufferCount);
            item.EncodeTo(buffer);
            return buffer.WrittenCount;
        }
    }
}
