using PooledAwait;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Secs4Net.Extensions;

public static class SecsExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChunkedSpan<T> Chunk<T>(ref this Span<T> span, int count) => new(span, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChunkedReadOnlySpan<T> Chunk<T>(ref this ReadOnlySpan<T> span, int count) => new(span, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChunkedMemory<T> Chunk<T>(this Memory<T> memory, int count) => new(memory, count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ChunkedReadOnlyMemory<T> Chunk<T>(this ReadOnlyMemory<T> memory, int count) => new(memory, count);

    public static IEnumerable<Memory<T>> AsEnumerable<T>(this ChunkedMemory<T> source)
    {
        foreach (var m in source)
        {
            yield return m;
        }
    }

    public static IEnumerable<ReadOnlyMemory<T>> AsEnumerable<T>(this ChunkedReadOnlyMemory<T> source)
    {
        foreach (var m in source)
        {
            yield return m;
        }
    }

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

#if NET
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> AsBytes<T>(this ref T value) where T : unmanaged
        => MemoryMarshal.CreateSpan(ref Unsafe.As<T, byte>(ref value), Unsafe.SizeOf<T>());
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<byte> AsBytes<T>(this ref T value) where T : unmanaged
        => new(Unsafe.AsPointer(ref value), Unsafe.SizeOf<T>());
#endif

    internal static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
    {
        var tcs = ValueTaskCompletionSource<object?>.Create();

        // This disposes the registration as soon as one of the tasks trigger
        using (cancellationToken.Register(static state => ((TaskCompletionSource<object?>)state!).TrySetResult(null), tcs))
        {
            var resultTask = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);
            if (resultTask == tcs.Task)
            {
                // Operation cancelled
                throw new OperationCanceledException(cancellationToken);
            }

            await task;
        }
    }

    internal static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var tcs = ValueTaskCompletionSource<object?>.Create();

        // This disposes the registration as soon as one of the tasks trigger
        using (cancellationToken.Register(static state => ((TaskCompletionSource<object?>)state!).TrySetResult(null), tcs))
        {
            var resultTask = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);
            if (resultTask == tcs.Task)
            {
                // Operation cancelled
                throw new OperationCanceledException(cancellationToken);
            }

            return await task;
        }
    }
}
