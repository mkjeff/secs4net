using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
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
    public void Setup()
    {
        _uint16 = new ushort[Size];
        _uint32 = new uint[Size];
        _uint64 = new ulong[Size];
        _int16 = new short[Size];
        _int32 = new int[Size];
        _int64 = new long[Size];
        _single = new float[Size];
        _double = new double[Size];
    }

    [Benchmark(Description = "Unsafe Loop")]
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

    [Benchmark(Description = "Unsafe Loop")]
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

    [Benchmark(Description = "Unsafe Loop")]
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

    [Benchmark(Description = "Unsafe Loop")]
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

    [Benchmark(Description = "Unsafe Loop")]
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

    [Benchmark(Description = "Unsafe Loop")]
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

    [Benchmark(Description = "Unsafe Loop")]
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
            a = BinaryPrimitives.ReadSingleBigEndian(a.AsReadOnlyBytes());
        }
    }

    [Benchmark(Description = "Unsafe Loop")]
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
            a = BinaryPrimitives.ReadDoubleBigEndian(a.AsReadOnlyBytes());
        }
    }
}
