using System.Runtime.CompilerServices;

namespace Secs4Net;

internal static class MessageIdGenerator
{
    private static int _id = Random.Shared.Next();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NewId() => Interlocked.Increment(ref _id);
}
