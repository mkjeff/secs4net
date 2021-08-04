using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net;
using Secs4Net.Extensions;
using Secs4Netb.Benchmark;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Benchmarks
{
    //[Config(typeof(BenchmarkConfig))]
    //[MemoryDiagnoser]
    //[GenericTypeArguments(typeof(ushort))]
    //[GenericTypeArguments(typeof(short))]
    //[GenericTypeArguments(typeof(uint))]
    //[GenericTypeArguments(typeof(int))]
    //[GenericTypeArguments(typeof(ulong))]
    //[GenericTypeArguments(typeof(long))]
    //[GenericTypeArguments(typeof(float))]
    //[GenericTypeArguments(typeof(double))]
    public unsafe class SpanReverseEndianness<T> where T : unmanaged
    {
        private static int offSet = Unsafe.SizeOf<T>();
        private MemoryOwner<T> _data;

        [Params(16)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            ReverseEndiannessHelper<T>.Reverse(default);
            _data = MemoryOwner<T>.Allocate(Size);
        }

        [GlobalCleanup]
        public void Cleanup() => _data.Dispose();


        [Benchmark]
        public int SliceAndReverse()
        {
            var bytes = _data.Span.AsBytes();
            for (var i = 0; i < bytes.Length; i += offSet)
            {
                bytes.Slice(i, offSet).Reverse();
            }
            return bytes.Length;
        }

        [Benchmark]
        public int ReverseEndianness()
        {
            var data = _data.Memory.Span;

            for (int i = 0; i < data.Length; i++)
            {
                ref T value = ref data.DangerousGetReferenceAt(i);
                value = ReverseEndiannessHelper<T>.Reverse(value);
            }

            return data.Length;
        }
    }
}
