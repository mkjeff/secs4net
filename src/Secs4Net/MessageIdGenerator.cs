using System.Runtime.CompilerServices;

namespace Secs4Net;

internal static class MessageIdGenerator
{
#if NET
    private static int _id = Random.Shared.Next();
#else
    private static int _id = new Random(Guid.NewGuid().GetHashCode()).Next();
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NewId() => Interlocked.Increment(ref _id);
}
