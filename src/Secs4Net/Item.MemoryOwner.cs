using System;
using System.Buffers;

namespace Secs4Net
{
    partial class Item
    {
        private sealed class MemoryOwnerItem<T> : MemoryItem<T> where T : unmanaged, IEquatable<T>
        {
            private readonly IMemoryOwner<T> _owner;
            private protected override ReadOnlyMemory<T> Value => _owner.Memory;

            internal MemoryOwnerItem(SecsFormat format, IMemoryOwner<T> memoryOwner) 
                : base(format, memoryOwner.Memory)
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
