using Microsoft.Toolkit.HighPerformance.Buffers;
using System;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class MemoryOwnerItem<T> : MemoryItem<T> where T : unmanaged
        {
            private readonly MemoryOwner<T> _owner;
            internal MemoryOwnerItem(SecsFormat format, MemoryOwner<T> memoryOwner) : base(format, memoryOwner.Memory)
            {
                _owner = memoryOwner;
            }

            public override void Dispose()
            {
                _owner.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
