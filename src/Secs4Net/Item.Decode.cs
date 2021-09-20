using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net;

partial class Item
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void DecodeFormatAndLengthByteCount(byte formatAndLengthByte, out SecsFormat format, out byte lengthByteCount)
    {
        format = (SecsFormat)(formatAndLengthByte >> 2);
        lengthByteCount = (byte)(formatAndLengthByte & 0b00000011);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    internal static void DecodeDataLength(in ReadOnlySequence<byte> sourceBytes, out int dataLength)
    {
        dataLength = 0;
        var lengthBytes = dataLength.AsBytes();
        sourceBytes.CopyTo(lengthBytes);
        lengthBytes.Slice(0, (int)sourceBytes.Length).Reverse();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [SkipLocalsInit]
    public static unsafe Item DecodeFromFullBuffer(ref ReadOnlySequence<byte> bytes)
    {
#if NET
        DecodeFormatAndLengthByteCount(bytes.FirstSpan.DangerousGetReferenceAt(0), out var format, out var lengthByteCount);
#else
        DecodeFormatAndLengthByteCount(bytes.First.Span.DangerousGetReferenceAt(0), out var format, out var lengthByteCount);
#endif

        var dataLengthSeq = bytes.Slice(1, lengthByteCount);
        DecodeDataLength(dataLengthSeq, out var dataLength);
        bytes = bytes.Slice(dataLengthSeq.End);

        if (format == SecsFormat.List)
        {
            if (dataLength == 0)
            {
                return L();
            }

            var items = new Item[dataLength];
            for (var i = 0; i < items.Length; i++)
            {
                items.DangerousGetReferenceAt(i) = DecodeFromFullBuffer(ref bytes);
            }

            return L(items);
        }

        var dataItemBytes = bytes.Slice(0, dataLength);
        var item = DecodeDataItem(format, dataItemBytes);
        bytes = bytes.Slice(dataItemBytes.End);
        return item;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Item DecodeDataItem(SecsFormat format, in ReadOnlySequence<byte> bytes)
    {
        return format switch
        {
            SecsFormat.Binary => DecodeBinary(bytes),
            SecsFormat.Boolean => DecodeBoolean(bytes),
            SecsFormat.ASCII => DecodeASCII(bytes),
            SecsFormat.JIS8 => DecodeJIS8(bytes),
            SecsFormat.I8 => DecodeI8(bytes),
            SecsFormat.I1 => DecodeI1(bytes),
            SecsFormat.I2 => DecodeI2(bytes),
            SecsFormat.I4 => DecodeI4(bytes),
            SecsFormat.F8 => DecodeF8(bytes),
            SecsFormat.F4 => DecodeF4(bytes),
            SecsFormat.U8 => DecodeU8(bytes),
            SecsFormat.U1 => DecodeU1(bytes),
            SecsFormat.U2 => DecodeU2(bytes),
            SecsFormat.U4 => DecodeU4(bytes),
            _ => ThrowHelper(bytes),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeASCII(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return A();
                case >= 512:
                    {
                        var owner = MemoryOwner<byte>.Allocate(length);
                        bytes.CopyTo(owner.Memory.Span);
                        return new LazyStringItem(SecsFormat.ASCII, owner);
                    }
                default:
                    {
                        using var spanOwner = SpanOwner<byte>.Allocate(length);
                        bytes.CopyTo(spanOwner.Span);
                        return A(StringPool.Shared.GetOrAdd(spanOwner.Span, Encoding.ASCII));
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeJIS8(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return J();
                case >= 512:
                    {
                        var owner = MemoryOwner<byte>.Allocate(length);
                        bytes.CopyTo(owner.Memory.Span);
                        return new LazyStringItem(SecsFormat.JIS8, owner);
                    }
                default:
                    {
                        using var spanOwner = SpanOwner<byte>.Allocate(length);
                        bytes.CopyTo(spanOwner.Span);
                        return J(StringPool.Shared.GetOrAdd(spanOwner.Span, Jis8Encoding));
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeBoolean(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return Boolean();
                case >= 1024:
                    {
                        var owner = MemoryOwner<bool>.Allocate(length);
                        bytes.CopyTo(owner.Span.AsBytes());
                        return Boolean(owner);
                    }
                default:
                    {
                        Memory<bool> memory = new bool[length];
                        bytes.CopyTo(memory.Span.AsBytes());
                        return Boolean(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeBinary(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return B();
                case >= 1024:
                    {
                        var owner = MemoryOwner<byte>.Allocate(length);
                        bytes.CopyTo(owner.Span);
                        return B(owner);
                    }
                default:
                    {
                        Memory<byte> memory = new byte[length];
                        bytes.CopyTo(memory.Span.AsBytes());
                        return B(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeU1(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return U1();
                case >= 1024:
                    {
                        var owner = MemoryOwner<byte>.Allocate(length);
                        bytes.CopyTo(owner.Span);
                        return U1(owner);
                    }
                default:
                    {
                        Memory<byte> memory = new byte[length];
                        bytes.CopyTo(memory.Span.AsBytes());
                        return U1(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeU2(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return U2();
                case >= 1024:
                    {
                        var owner = MemoryOwner<ushort>.Allocate(length / sizeof(ushort));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return U2(owner);
                    }
                default:
                    {
                        Memory<ushort> memory = new ushort[length / sizeof(ushort)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return U2(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeU4(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return U4();
                case >= 1024:
                    {
                        var owner = MemoryOwner<uint>.Allocate(length / sizeof(uint));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return U4(owner);
                    }
                default:
                    {
                        Memory<uint> memory = new uint[length / sizeof(uint)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return U4(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeU8(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return U8();
                case >= 1024:
                    {
                        var owner = MemoryOwner<ulong>.Allocate(length / sizeof(ulong));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return U8(owner);
                    }
                default:
                    {
                        Memory<ulong> memory = new ulong[length / sizeof(ulong)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return U8(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeI1(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return I1();
                case >= 1024:
                    {
                        var owner = MemoryOwner<sbyte>.Allocate(length);
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        return I1(owner);
                    }
                default:
                    {
                        Memory<sbyte> memory = new sbyte[length];
                        bytes.CopyTo(memory.Span.AsBytes());
                        return I1(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeI2(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return I2();
                case >= 1024:
                    {
                        var owner = MemoryOwner<short>.Allocate(length / sizeof(short));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return I2(owner);
                    }
                default:
                    {
                        Memory<short> memory = new short[length / sizeof(short)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return I2(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeI4(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return I4();
                case >= 1024:
                    {
                        var owner = MemoryOwner<int>.Allocate(length / sizeof(int));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return I4(owner);
                    }
                default:
                    {
                        Memory<int> memory = new int[length / sizeof(int)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return I4(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SkipLocalsInit]
        static Item DecodeI8(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return I8();
                case >= 1024:
                    {
                        var owner = MemoryOwner<long>.Allocate(length / sizeof(long));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return I8(owner);
                    }
                default:
                    {
                        Memory<long> memory = new long[length / sizeof(long)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return I8(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [SkipLocalsInit]
        static Item DecodeF4(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return F4();
                case >= 1024:
                    {
                        var owner = MemoryOwner<float>.Allocate(length / sizeof(float));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return F4(owner);
                    }
                default:
                    {
                        Memory<float> memory = new float[length / sizeof(float)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return F4(memory);
                    }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [SkipLocalsInit]
        static Item DecodeF8(in ReadOnlySequence<byte> bytes)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return F8();
                case >= 1024:
                    {
                        var owner = MemoryOwner<double>.Allocate(length / sizeof(double));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return F8(owner);
                    }
                default:
                    {
                        Memory<double> memory = new double[length / sizeof(double)];
                        var span = memory.Span;
                        bytes.CopyTo(span.AsBytes());
                        span.ReverseEndianness();
                        return F8(memory);
                    }
            }
        }

        [DoesNotReturn]
        static Item ThrowHelper(in ReadOnlySequence<byte> _) => throw new ArgumentOutOfRangeException();
    }
}
