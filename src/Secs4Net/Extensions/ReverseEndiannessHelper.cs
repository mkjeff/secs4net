using System.Buffers.Binary;

namespace Secs4Net.Extensions;

public static unsafe class ReverseEndiannessHelper<T> where T : unmanaged
{
    public static readonly delegate*<Span<T>, void> Reverse;

    static ReverseEndiannessHelper()
    {
        var t = typeof(T);
        if (t == typeof(ushort))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseUInt16;
        }
        else if (t == typeof(uint))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseUInt32;
        }
        else if (t == typeof(ulong))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseUInt64;
        }
        else if (t == typeof(short))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseInt16;
        }
        else if (t == typeof(int))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseInt32;
        }
        else if (t == typeof(long))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseInt64;
        }
        else if (t == typeof(float))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseSingle;
        }
        else if (t == typeof(double))
        {
            Reverse = (delegate*<Span<T>, void>)ReverseHelper.ReverseDouble;
        }
        else
        {
            Reverse = &ReverseNothing;
        }

        static void ReverseNothing(Span<T> bytes) { }
    }
}

public static unsafe class ReverseHelper
{
    internal static readonly delegate*<Span<ushort>, void> ReverseUInt16 = &ReverseEndianness;
    internal static readonly delegate*<Span<uint>, void> ReverseUInt32 = &ReverseEndianness;
    internal static readonly delegate*<Span<ulong>, void> ReverseUInt64 = &ReverseEndianness;
    internal static readonly delegate*<Span<short>, void> ReverseInt16 = &ReverseEndianness;
    internal static readonly delegate*<Span<int>, void> ReverseInt32 = &ReverseEndianness;
    internal static readonly delegate*<Span<long>, void> ReverseInt64 = &ReverseEndianness;
    internal static readonly delegate*<Span<float>, void> ReverseSingle = &ReverseEndianness;
    internal static readonly delegate*<Span<double>, void> ReverseDouble = &ReverseEndianness;

    private static void ReverseEndianness(this Span<short> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    private static void ReverseEndianness(this Span<ushort> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    private static void ReverseEndianness(this Span<int> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    private static void ReverseEndianness(this Span<uint> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    private static void ReverseEndianness(this Span<long> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    private static void ReverseEndianness(this Span<ulong> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReverseEndianness(a);
        }
    }

    private static void ReverseEndianness(this Span<float> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReadSingleBigEndian(a.AsReadOnlyBytes());
        }
    }

    private static void ReverseEndianness(this Span<double> span)
    {
        foreach (ref var a in span)
        {
            a = BinaryPrimitives.ReadDoubleBigEndian(a.AsReadOnlyBytes());
        }
    }
}
