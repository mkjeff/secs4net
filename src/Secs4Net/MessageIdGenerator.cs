using System.Runtime.CompilerServices;

namespace Secs4Net;

internal static class MessageIdGenerator
{
    private static int _id = new Random(Guid.NewGuid().GetHashCode()).Next();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NewId() => Interlocked.Increment(ref _id);
}
