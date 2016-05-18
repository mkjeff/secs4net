using System;
using System.Threading;

namespace Secs4Net
{
    sealed class SystemByte
    {
        int _systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
        public int Next
        {
            get
            {
                Interlocked.Increment(ref _systemByte);
                return _systemByte;
            }
        }
    }
}
