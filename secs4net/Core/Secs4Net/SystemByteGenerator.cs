using System;
using System.Threading;

namespace Secs4Net
{
    internal static class SystemByteGenerator
    {
        private static int _systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
        public static int New() => Interlocked.Increment(ref _systemByte);
    }
}
