using CommunityToolkit.HighPerformance;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Secs4Net;

public partial class Item
{
    [DebuggerTypeProxy(typeof(LazyMemoryItem<>.ItemDebugView))]
    [SkipLocalsInit]
    private class LazyMemoryItem<T> : Item where T : unmanaged, IEquatable<T>
    {
        private readonly LazyMemory _lazy;

        internal LazyMemoryItem(SecsFormat format, Memory<T> value)
            : base(format)
            => _lazy = new LazyMemory(value);

        private sealed class LazyMemory(Memory<T> value)
        {
            private readonly Memory<T> _value = value;
            private bool _reversed = false;

            public Memory<T> Value
            {
                get
                {
                    if (!_reversed)
                    {
                        _value.Span.Reverse();
                        _reversed = true;
                    }
                    return _value;
                }
            }
        }

        public sealed override int Count => _lazy.Value.Length;

        public sealed override ref TResult FirstValue<TResult>()
        {
            var span = _lazy.Value.Span;
            if (span.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
            {
                ThrowHelper();
            }

            return ref Unsafe.As<T, TResult>(ref span.DangerousGetReferenceAt(0));

            [DoesNotReturn]
            static void ThrowHelper() => throw new IndexOutOfRangeException($"The item is empty or data length less than sizeof({typeof(T).Name})");
        }

        public sealed override TResult FirstValueOrDefault<TResult>(TResult defaultValue = default)
        {
            var span = _lazy.Value.Span;
            if (span.Length * Unsafe.SizeOf<T>() < Unsafe.SizeOf<TResult>())
            {
                return defaultValue;
            }

            return Unsafe.As<T, TResult>(ref span.DangerousGetReferenceAt(0));
        }

        public sealed override Memory<TResult> GetMemory<TResult>()
            => _lazy.Value.Cast<T, TResult>();

        public sealed override unsafe void EncodeTo(IBufferWriter<byte> buffer)
        {
            var value = _lazy.Value;
            if (value.IsEmpty)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }

            var valueByteSpan = value.Span.AsBytes();
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
            => Format == other.Format && GetMemory<T>().Span.SequenceEqual(other.GetMemory<T>().Span);

        private sealed class ItemDebugView(LazyMemoryItem<T> item)
        {
            public Span<T> Value => item._lazy.Value.Span;
            public EncodedByteDebugView EncodedBytes { get; } = new EncodedByteDebugView(item);
        }
    }
}
