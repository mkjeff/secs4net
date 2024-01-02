using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net;

public partial class Item
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void DecodeFormatAndLengthByteCount(in ReadOnlySequence<byte> sourceBytes, out SecsFormat format, out byte lengthByteCount)
    {
#if NET
        var formatSeqFirstSpan = sourceBytes.FirstSpan;
#else
        var formatSeqFirstSpan = sourceBytes.First.Span;
#endif

#if DEBUG
        byte formatAndLengthByte = formatSeqFirstSpan[0];
#else
        byte formatAndLengthByte = formatSeqFirstSpan.DangerousGetReferenceAt(0);
#endif
        format = (SecsFormat)(formatAndLengthByte >> 2);
        lengthByteCount = (byte)(formatAndLengthByte & 0b00000011);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    internal static int DecodeDataLength(in ReadOnlySequence<byte> sourceBytes)
    {
        var dataLength = 0;
        var lengthBytes = dataLength.AsBytes();
        sourceBytes.CopyTo(lengthBytes);
        lengthBytes[..(int)sourceBytes.Length].Reverse();
        return dataLength;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [SkipLocalsInit]
    public static Item DecodeFromFullBuffer(ref ReadOnlySequence<byte> bytes)
    {
        var formatSeq = bytes.Slice(0, 1);
        DecodeFormatAndLengthByteCount(formatSeq, out var format, out var lengthByteCount);

        var dataLengthSeq = bytes.Slice(formatSeq.End, lengthByteCount);
        var dataLength = DecodeDataLength(dataLengthSeq);
        bytes = bytes.Slice(dataLengthSeq.End);

        if (format == SecsFormat.List)
        {
            if (dataLength == 0)
            {
                return L();
            }

            var items = new Item[dataLength];
            foreach (ref var subItem in items.AsSpan())
            {
                subItem = DecodeFromFullBuffer(ref bytes);
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
            SecsFormat.Binary => DecodeMemoryItem<byte>(SecsFormat.Binary, bytes),
            SecsFormat.Boolean => DecodeMemoryItem<bool>(SecsFormat.Boolean, bytes),
            SecsFormat.ASCII => DecodeStringItem(format, bytes, Encoding.ASCII),
            SecsFormat.JIS8 => DecodeStringItem(format, bytes, Jis8Encoding),
            SecsFormat.I8 => DecodeMemoryItem<long>(SecsFormat.I8, bytes),
            SecsFormat.I1 => DecodeMemoryItem<sbyte>(SecsFormat.I1, bytes),
            SecsFormat.I2 => DecodeMemoryItem<short>(SecsFormat.I2, bytes),
            SecsFormat.I4 => DecodeMemoryItem<int>(SecsFormat.I4, bytes),
            SecsFormat.F8 => DecodeMemoryItem<double>(SecsFormat.F8, bytes),
            SecsFormat.F4 => DecodeMemoryItem<float>(SecsFormat.F4, bytes),
            SecsFormat.U8 => DecodeMemoryItem<ulong>(SecsFormat.U8, bytes),
            SecsFormat.U1 => DecodeMemoryItem<byte>(SecsFormat.U1, bytes),
            SecsFormat.U2 => DecodeMemoryItem<ushort>(SecsFormat.U2, bytes),
            SecsFormat.U4 => DecodeMemoryItem<uint>(SecsFormat.U4, bytes),
            _ => ThrowHelper(),
        };

        [SkipLocalsInit]
        static Item DecodeStringItem(SecsFormat format, in ReadOnlySequence<byte> bytes, Encoding encoding)
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return new StringItem(format, string.Empty);
                case >= 512:
                    {
                        var owner = MemoryOwner<byte>.Allocate(length);
                        bytes.CopyTo(owner.Memory.Span);
                        return new LazyStringItem(format, owner);
                    }
                default:
                    {
                        using var spanOwner = SpanOwner<byte>.Allocate(length);
                        bytes.CopyTo(spanOwner.Span);
                        return new StringItem(format, StringPool.Shared.GetOrAdd(spanOwner.Span, encoding));
                    }
            }
        }

        [SkipLocalsInit]
        static unsafe Item DecodeMemoryItem<T>(SecsFormat format, in ReadOnlySequence<byte> bytes) where T : unmanaged, IEquatable<T>
        {
            int length = (int)bytes.Length;
            switch (length)
            {
                case 0:
                    return new MemoryItem<T>(format);
                case >= 1024:
                    {
                        var owner = MemoryOwner<T>.Allocate(length / sizeof(T));
                        var span = owner.Span;
                        bytes.CopyTo(span.AsBytes());
                        ReverseEndiannessHelper<T>.Reverse(span);
                        return new MemoryOwnerItem<T>(format, owner);
                    }
                default:
                    {
                        var memory = new T[length / sizeof(T)];
                        var span = memory.AsSpan();
                        bytes.CopyTo(span.AsBytes());
                        ReverseEndiannessHelper<T>.Reverse(span);
                        return new MemoryItem<T>(format, memory);
                    }
            }
        }

        [DoesNotReturn]
        static Item ThrowHelper() => throw new ArgumentOutOfRangeException();
    }
}
