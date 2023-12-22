using System.Buffers;

namespace Secs4Net;

partial class Item
{
    private sealed class MemoryOwnerItem<T> : MemoryItem<T> where T : unmanaged, IEquatable<T>
#if NET8_0
        , ISpanParsable<T>
#endif
    {
        private readonly IMemoryOwner<T> _owner;

        internal MemoryOwnerItem(SecsFormat format, IMemoryOwner<T> memoryOwner)
            : base(format, memoryOwner.Memory)
            => _owner = memoryOwner;

        public override void Dispose()
            => _owner.Dispose();
    }
}
