#if !NET
using CommunityToolkit.HighPerformance.Buffers;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class EncodingExtenstions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int GetBytes(this Encoding encoding, string str, Span<byte> span)
        {
            fixed (char* chars = str.AsSpan())
            {
                fixed (byte* bytes = span)
                {
                    return encoding.GetBytes(chars, str.Length, bytes, span.Length);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe int GetCharCount(this Encoding encoding, ReadOnlySpan<byte> span)
        {
            fixed (byte* bytes = span)
            {
                return encoding.GetCharCount(bytes, span.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe string GetString(this Encoding encoding, ReadOnlySpan<byte> bytes)
        {
            using var spanOwner = SpanOwner<byte>.Allocate((int)bytes.Length);
            bytes.CopyTo(spanOwner.Span);

            fixed (byte* spanBytes = spanOwner.Span)
            {
                return encoding.GetString(spanBytes, spanOwner.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe string GetString(this Encoding encoding, in ReadOnlySequence<byte> bytes)
        {
            using var spanOwner = SpanOwner<byte>.Allocate((int)bytes.Length);
            bytes.CopyTo(spanOwner.Span);

            fixed (byte* spanBytes = spanOwner.Span)
            {
                return encoding.GetString(spanBytes, spanOwner.Length);
            }
        }
    }
}
#endif
