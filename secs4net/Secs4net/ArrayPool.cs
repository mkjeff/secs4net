using System.Buffers;

namespace Secs4Net
{
    internal static class ValueTypeArrayPool<T>
        where T : struct
    {
        private static int MaxArrayLength = 1024 * 1024;
        private static int MaxArrayPerBucket = 500;

        internal static readonly ArrayPool<T> Pool = ArrayPool<T>.Create(MaxArrayLength, MaxArrayPerBucket);
    }

    internal static class SecsItemArrayPool
    {
        private static int MaxArrayLength = 512;
        private static int MaxArrayPerBucket = 100;
        internal static readonly ArrayPool<SecsItem> Pool = ArrayPool<SecsItem>.Create(MaxArrayLength, MaxArrayPerBucket);
    }
}
