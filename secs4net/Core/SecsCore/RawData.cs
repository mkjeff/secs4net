using System;
using System.Collections.ObjectModel;

namespace Secs4Net
{
    public sealed class RawData : ReadOnlyCollection<byte> {
        internal RawData(byte[] bytes) : base(bytes) { }
        internal byte[] Bytes => (byte[])Items;
    }
}
