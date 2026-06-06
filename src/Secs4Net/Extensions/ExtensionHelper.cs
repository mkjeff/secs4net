using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Secs4Net.Extensions;

public static class SecsExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetName(this SecsFormat format)
    {
        return format switch
        {
            SecsFormat.List => nameof(SecsFormat.List),
            SecsFormat.Binary => nameof(SecsFormat.Binary),
            SecsFormat.Boolean => nameof(SecsFormat.Boolean),
            SecsFormat.ASCII => nameof(SecsFormat.ASCII),
            SecsFormat.JIS8 => nameof(SecsFormat.JIS8),
            SecsFormat.I8 => nameof(SecsFormat.I8),
            SecsFormat.I1 => nameof(SecsFormat.I1),
            SecsFormat.I2 => nameof(SecsFormat.I2),
            SecsFormat.I4 => nameof(SecsFormat.I4),
            SecsFormat.F8 => nameof(SecsFormat.F8),
            SecsFormat.F4 => nameof(SecsFormat.F4),
            SecsFormat.U8 => nameof(SecsFormat.U8),
            SecsFormat.U1 => nameof(SecsFormat.U1),
            SecsFormat.U2 => nameof(SecsFormat.U2),
            SecsFormat.U4 => nameof(SecsFormat.U4),
            _ => ThrowHelper(format),
        };

        [DoesNotReturn]
        static string ThrowHelper(SecsFormat format) => throw new ArgumentOutOfRangeException(nameof(format), (int)format, "Invalid enum value");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe Span<byte> AsBytes<T>(this scoped ref T value) where T : unmanaged
       => MemoryMarshal.CreateSpan(ref Unsafe.As<T, byte>(ref value), sizeof(T));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe ReadOnlySpan<byte> AsReadOnlyBytes<T>(this scoped ref T value) where T : unmanaged
        => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref value), sizeof(T));
}
