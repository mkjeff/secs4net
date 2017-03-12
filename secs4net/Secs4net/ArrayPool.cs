using System.Buffers;

namespace Secs4Net
{
    internal static class ValueTypeArrayPool<T>
        where T : struct
    {
        internal static readonly ArrayPool<T> Pool = ArrayPool<T>.Create(
            ValueItemDecodePoolSetting<T>.MaxArrayLength,
            ValueItemDecodePoolSetting<T>.MaxArrayPerBucket);
    }

    internal static class ValueItemDecodePoolSetting<T>
        where T : struct
    {
        public static int MaxArrayLength = 1024*1024;
        public static int MaxArrayPerBucket = 500;
    }

    internal static class SecsItemArrayPool
    {
        internal static readonly ArrayPool<SecsItem> Pool = ArrayPool<SecsItem>.Create(
            ListItemDecodePoolSetting.MaxArrayLength,
            ListItemDecodePoolSetting.MaxArrayPerBucket);
    }

    internal static class ListItemDecodePoolSetting
    {
        public static int MaxArrayLength = 512;
        public static int MaxArrayPerBucket = 100;
    }
}
