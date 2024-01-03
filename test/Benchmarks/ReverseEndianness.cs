using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using CommunityToolkit.HighPerformance;
using Secs4Net.Extensions;
using System;
using System.Buffers.Binary;

namespace Benchmarks;

[Config(typeof(BenchmarkConfig))]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ReverseEndianness
{
    private ushort[] _uint16;
    private uint[] _uint32;
    private ulong[] _uint64;
    private short[] _int16;
    private int[] _int32;
    private long[] _int64;
    private float[] _single;
    private double[] _double;


    [Params(64)]
    public int Size { get; set; }

    [GlobalSetup]
    public unsafe void Setup()
    {
        ReverseEndiannessHelper<ushort>.Reverse(default);
        ReverseEndiannessHelper<uint>.Reverse(default);
        ReverseEndiannessHelper<ulong>.Reverse(default);
        ReverseEndiannessHelper<short>.Reverse(default);
        ReverseEndiannessHelper<int>.Reverse(default);
        ReverseEndiannessHelper<long>.Reverse(default);
        ReverseEndiannessHelper<float>.Reverse(default);
        ReverseEndiannessHelper<double>.Reverse(default);
        _uint16 = new ushort[Size];
        _uint32 = new uint[Size];
        _uint64 = new ulong[Size];
        _int16 = new short[Size];
        _int32 = new int[Size];
        _int64 = new long[Size];
        _single = new float[Size];
        _double = new double[Size];
    }


    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("UInt16")]
    public int UInt16_SliceAndReverse()
    {
        var bytes = _uint16.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(ushort))
        {
            bytes.Slice(i, sizeof(ushort)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("UInt16")]
    public unsafe int UInt16_ReverseEndiannessHelper()
    {
        var data = _uint16.AsSpan();
        ReverseEndiannessHelper<ushort>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("UInt16")]
    public int UInt16_BinaryPrimitives()
    {
        var data = _uint16.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("UInt16")]
    public void UInt16_ForeachRef()
    {
        foreach (ref var a in _uint16.AsSpan())
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("UInt32")]
    public int UInt32_SliceAndReverse()
    {
        var bytes = _uint32.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(uint))
        {
            bytes.Slice(i, sizeof(uint)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("UInt32")]
    public unsafe int UInt32_ReverseEndiannessHelper()
    {
        var data = _uint32.AsSpan();
        ReverseEndiannessHelper<uint>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("UInt32")]
    public int UInt32_BinaryPrimitives()
    {
        var data = _uint32.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("UInt32")]
    public void UInt32_ForeachRef()
    {
        foreach (ref var a in _uint32.AsSpan())
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("UInt64")]
    public int UInt64_SliceAndReverse()
    {
        var bytes = _uint64.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(ulong))
        {
            bytes.Slice(i, sizeof(ulong)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("UInt64")]
    public unsafe int UInt64_ReverseEndiannessHelper()
    {
        var data = _uint64.AsSpan();
        ReverseEndiannessHelper<ulong>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("UInt64")]
    public int UInt64_BinaryPrimitives()
    {
        var data = _uint64.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("UInt64")]
    public void UInt64_ForeachRef()
    {
        foreach (ref var a in _uint64.AsSpan())
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("Int16")]
    public int Int16_SliceAndReverse()
    {
        var bytes = _int16.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(short))
        {
            bytes.Slice(i, sizeof(short)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("Int16")]
    public unsafe int Int16_ReverseEndiannessHelper()
    {
        var data = _int16.AsSpan();
        ReverseEndiannessHelper<short>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("Int16")]
    public int Int16_BinaryPrimitives()
    {
        var data = _int16.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("Int16")]
    public void Int16_ForeachRef()
    {
        foreach (ref var a in _int16.AsSpan())
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("Int32")]
    public int Int32_SliceAndReverse()
    {
        var bytes = _int32.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(int))
        {
            bytes.Slice(i, sizeof(int)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("Int32")]
    public unsafe int Int32_ReverseEndiannessHelper()
    {
        var data = _int32.AsSpan();
        ReverseEndiannessHelper<int>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("Int32")]
    public int Int32_BinaryPrimitives()
    {
        var data = _int32.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("Int32")]
    public void Int32_ForeachRef()
    {
        foreach (ref var a in _int32.AsSpan())
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("Int64")]
    public int Int64_SliceAndReverse()
    {
        var bytes = _int64.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(long))
        {
            bytes.Slice(i, sizeof(long)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("Int64")]
    public unsafe int Int64_ReverseEndiannessHelper()
    {
        var data = _int64.AsSpan();
        ReverseEndiannessHelper<long>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("Int64")]
    public int Int64_BinaryPrimitives()
    {
        var data = _int64.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("Int64")]
    public void Int64_ForeachRef()
    {
        foreach (ref var a in _int64.AsSpan())
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("Single")]
    public int Single_SliceAndReverse()
    {
        var bytes = _single.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(float))
        {
            bytes.Slice(i, sizeof(float)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("Single")]
    public unsafe int Single_ReverseEndiannessHelper()
    {
        var data = _single.AsSpan();
        ReverseEndiannessHelper<float>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("Single")]
    public int Single_BinaryPrimitives()
    {
        var data = _single.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("Single")]
    public void Single_ForeachRef()
    {
        foreach (ref var a in _single.AsSpan())
        {
#if NET
            a = BinaryPrimitives.ReadSingleBigEndian(a.AsBytes());
#else
            a = ReverseHelper.ReadSingleBigEndian(a.AsBytes());
#endif
        }
    }

    //[Benchmark(Description = "SliceReverse")]
    [BenchmarkCategory("Double")]
    public int Double_SliceAndReverse()
    {
        var bytes = _double.AsSpan().AsBytes();
        for (var i = 0; i < bytes.Length; i += sizeof(double))
        {
            bytes.Slice(i, sizeof(double)).Reverse();
        }
        return bytes.Length;
    }

    [Benchmark(Description = "ReverseEndiannessHelper")]
    [BenchmarkCategory("Double")]
    public unsafe int Double_ReverseEndiannessHelper()
    {
        var data = _double.AsSpan();
        ReverseEndiannessHelper<double>.Reverse(data);
        return data.Length;
    }

    [Benchmark(Description = "BinaryPrimitives")]
    [BenchmarkCategory("Double")]
    public int Double_BinaryPrimitives()
    {
        var data = _double.AsSpan();
        data.ReverseEndianness();
        return data.Length;
    }

    [Benchmark(Description = "ForeachRef")]
    [BenchmarkCategory("Double")]
    public void Double_ForeachRef()
    {
        foreach (ref var a in _double.AsSpan())
        {
#if NET
            a = BinaryPrimitives.ReadDoubleBigEndian(a.AsBytes());
#else
            a = ReverseHelper.ReadDoubleBigEndian(a.AsBytes());
#endif
        }
    }
}
