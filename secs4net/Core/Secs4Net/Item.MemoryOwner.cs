using System;
using System.Buffers;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class MemoryOwnerItem<T> : MemoryItem<T> where T : unmanaged
        {
            private readonly IMemoryOwner<T> _owner;
            public MemoryOwnerItem(SecsFormat format, IMemoryOwner<T> memoryOwner) : base(format, memoryOwner.Memory)
            {
                _owner = memoryOwner;
            }

            public sealed override void Dispose()
            {
                _owner.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
