using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secs4Net
{
    internal static class ValueTypeArrayPool<T>
        where T : struct
    {
        internal static readonly ArrayPool<T> Pool = ArrayPool<T>.Create(
            DecodePoolSetting<T>.MaxArrayLength,
            DecodePoolSetting<T>.MaxArrayPerBucket);
    }

    internal static class DecodePoolSetting<T>
        where T : struct
    {
        public static int MaxArrayLength = 1024*1024;
        public static int MaxArrayPerBucket = 500;
    }
}
