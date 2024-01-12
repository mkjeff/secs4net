using CommunityToolkit.HighPerformance;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Secs4Net;

public partial class Item
{
    [DebuggerTypeProxy(typeof(MemoryItem<>.ItemDebugView))]
    [SkipLocalsInit]
    private class MemoryItem<T> : Item where T : unmanaged, IEquatable<T>
    {
        private readonly Memory<T> _value;

        internal MemoryItem(SecsFormat format, Memory<T> value)
            : base(format)
            => _value = value;

        public sealed override int Count => _value.Length;

        public sealed override ref TResult FirstValue<TResult>()
        {
            var span = _value.Span;
            if (span.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
            {
                ThrowHelper();
            }

            return ref Unsafe.As<T, TResult>(ref MemoryMarshal.GetReference(span));

            [DoesNotReturn]
            static void ThrowHelper() => throw new IndexOutOfRangeException($"The item is empty or data length less than sizeof({typeof(T).Name})");
        }

        public sealed override TResult FirstValueOrDefault<TResult>(TResult defaultValue = default)
        {
            var span = _value.Span;
            if (span.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
            {
                return defaultValue;
            }

            return Unsafe.As<T, TResult>(ref MemoryMarshal.GetReference(span));
        }

        public sealed override Memory<TResult> GetMemory<TResult>()
            => _value.Cast<T, TResult>();

        public sealed override unsafe void EncodeTo(IBufferWriter<byte> buffer)
        {
            var value = _value;
            if (value.IsEmpty)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }

            var valueByteSpan = MemoryMarshal.AsBytes(value.Span);
            var byteLength = valueByteSpan.Length;

            EncodeItemHeader(Format, byteLength, buffer);

            var bufferByteSpan = buffer.GetSpan(byteLength)[..byteLength];
            valueByteSpan.CopyTo(bufferByteSpan);
            ReverseEndiannessHelper<T>.Reverse(Cast(bufferByteSpan));
            buffer.Advance(byteLength);

#if NET
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Span<T> Cast(Span<byte> bytes)
                => MemoryMarshal.CreateSpan(ref Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(bytes)), bytes.Length / sizeof(T));
#else
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Span<T> Cast(Span<byte> bytes)
                => new(Unsafe.AsPointer(ref MemoryMarshal.GetReference(bytes)), bytes.Length / sizeof(T));
#endif
        }

        private protected sealed override bool IsEquals(Item other)
            => Format == other.Format && _value.Span.SequenceEqual(Unsafe.As<MemoryItem<T>>(other)._value.Span);

        private sealed class ItemDebugView(MemoryItem<T> item)
        {
            public Span<T> Value => item._value.Span;
            public EncodedByteDebugView EncodedBytes { get; } = new EncodedByteDebugView(item);
        }
    }
}
