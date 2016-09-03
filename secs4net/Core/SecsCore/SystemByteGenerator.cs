using System;
using System.Threading;

namespace Secs4Net
{
    sealed class SystemByteGenerator
    {
        int _systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
        public int New()
        {
            Interlocked.Increment(ref _systemByte);
            return _systemByte;
        }
    }
}
