using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    [DebuggerDisplay("{GetDebugString()}")]
    [DebuggerTypeProxy(typeof(EncodedItemDebugView))]
    public abstract partial class Item : IEquatable<Item>, IEnumerable<Item>, IDisposable
    {
        private const int DebuggerDisplayMaxCount = 20;
        private static readonly Encoding Jis8Encoding = Encoding.GetEncoding(50222);
        private static readonly Item EmptyL = new ListItem(SecsFormat.List, Array.Empty<Item>());
        private static readonly Item EmptyA = new StringItem(SecsFormat.ASCII, string.Empty);
        private static readonly Item EmptyJ = new StringItem(SecsFormat.JIS8, string.Empty);
        private static readonly Item EmptyBoolean = new MemoryItem<bool>(SecsFormat.Boolean, Array.Empty<bool>());
        private static readonly Item EmptyBinary = new MemoryItem<byte>(SecsFormat.Binary, Array.Empty<byte>());
        private static readonly Item EmptyU1 = new MemoryItem<byte>(SecsFormat.U1, Array.Empty<byte>());
        private static readonly Item EmptyU2 = new MemoryItem<ushort>(SecsFormat.U2, Array.Empty<ushort>());
        private static readonly Item EmptyU4 = new MemoryItem<uint>(SecsFormat.U4, Array.Empty<uint>());
        private static readonly Item EmptyU8 = new MemoryItem<ulong>(SecsFormat.U8, Array.Empty<ulong>());
        private static readonly Item EmptyI1 = new MemoryItem<sbyte>(SecsFormat.I1, Array.Empty<sbyte>());
        private static readonly Item EmptyI2 = new MemoryItem<short>(SecsFormat.I2, Array.Empty<short>());
        private static readonly Item EmptyI4 = new MemoryItem<int>(SecsFormat.I4, Array.Empty<int>());
        private static readonly Item EmptyI8 = new MemoryItem<long>(SecsFormat.I8, Array.Empty<long>());
        private static readonly Item EmptyF4 = new MemoryItem<float>(SecsFormat.F4, Array.Empty<float>());
        private static readonly Item EmptyF8 = new MemoryItem<double>(SecsFormat.F8, Array.Empty<double>());

        public SecsFormat Format { get; }
        public int Count { get; }

        private protected Item(SecsFormat format, int count)
        {
            Format = format;
            Count = count;
        }

        /// <summary>
        /// Encode the item to SECS binary format
        /// </summary>
        public abstract void EncodeTo(IBufferWriter<byte> buffer);

        /// <summary>
        /// Indexer of List items.
        /// Be careful of setter operation. Since the original slot will be overridden.
        /// So, it has no chance to be Disposed along with the List's Dispose method.
        /// You can invoke <see cref="Dispose"/> method on the original item by yourself or till the GC collects it.
        /// </summary>
        /// <exception cref="NotSupportedException">When the item's <see cref="Format"/> is not <see cref="SecsFormat.List"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual Item this[int index]
        {
            get => throw CreateNotSupportException();
            set => throw CreateNotSupportException();
        }

        /// <summary>
        /// Forms a slice out of the current List starting at a specified index for a specified length.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice</param>
        /// <exception cref="NotSupportedException">When the item's <see cref="Format"/> is not <see cref="SecsFormat.List"/></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual IEnumerable<Item> Slice(int start, int length)
            => throw CreateNotSupportException();

        private NotSupportedException CreateNotSupportException([CallerMemberName] string? memberName = null)
            => new NotSupportedException($"{memberName} is not supported, since the item's {nameof(Format)} is {Format}");

        /// <summary>
        /// Get the first element of item array value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">When item is empty or data length less than sizeof(<typeparamref name="T"/>)</exception>
        /// <exception cref="NotSupportedException">when the item's <see cref="Format"/> is <see cref="SecsFormat.List"/> or <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
        public virtual ref T FirstValue<T>() where T : unmanaged
            => throw CreateNotSupportException();

        /// <summary>
        /// Get the first element of item array value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">when <see cref="Format"/> is <see cref="SecsFormat.List"/> or <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
        public virtual ref readonly T FirstValueOrDefault<T>(in T defaultValue = default) where T : unmanaged
            => throw CreateNotSupportException();

        /// <summary>
        /// Get item value array wrapper
        /// </summary>
        /// <exception cref="NotSupportedException">when <see cref="Format"/> is <see cref="SecsFormat.List"/> or <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
        public virtual ValueArray<T> GetValues<T>() where T : unmanaged
            => throw CreateNotSupportException();

        /// <summary>
        /// Get item string value
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">when the <see cref="Format"/> is not <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
        public virtual string GetString()
            => throw CreateNotSupportException();

        public virtual IEnumerator<Item> GetEnumerator()
            => throw CreateNotSupportException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual void Dispose()
        {
        }

        public sealed override bool Equals(object? obj)
            => Equals(obj as Item);

        public bool Equals(Item? other)
            => other is not null && IsMatch(this, other);

        private static bool IsMatch(Item left, Item right)
        {
            if (left.Format != right.Format)
            {
                return false;
            }

            if (left.Count != right.Count)
            {
                return false;
            }

            if (left.Count == 0)
            {
                return true;
            }

            return left.Format switch
            {
                SecsFormat.List => IsListMatch(left, right),
                SecsFormat.ASCII or SecsFormat.JIS8 => string.Equals(left.GetString(), right.GetString(), StringComparison.Ordinal),
                _ => left.GetValues<byte>().AsBytes().SequenceEqual(right.GetValues<byte>().AsBytes()),
            };

            static bool IsListMatch(Item listLeft, Item listRight)
            {
                for (int i = 0, count = listLeft.Count; i < count; i++)
                {
                    if (!IsMatch(listLeft[i], listRight[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public sealed override string ToString() => $"{Format}[{Count}]";

        private string GetDebugString()
        {
            var sb = new StringBuilder(Format.ToString()).Append('[').Append(Count).Append("]: ");
            return Format switch
            {
                SecsFormat.List => sb.Append("...").ToString(),
                SecsFormat.ASCII or SecsFormat.JIS8 => sb.Append(GetString()).ToString(),
                SecsFormat.Binary => sb.AppendBinary(GetValues<byte>().AsBytes(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.Boolean => sb.AppendArray(GetValues<bool>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.I1 => sb.AppendArray(GetValues<sbyte>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.I2 => sb.AppendArray(GetValues<short>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.I4 => sb.AppendArray(GetValues<int>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.I8 => sb.AppendArray(GetValues<long>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.U1 => sb.AppendArray(GetValues<byte>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.U2 => sb.AppendArray(GetValues<ushort>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.U4 => sb.AppendArray(GetValues<uint>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.U8 => sb.AppendArray(GetValues<ulong>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.F4 => sb.AppendArray(GetValues<float>(), DebuggerDisplayMaxCount).ToString(),
                SecsFormat.F8 => sb.AppendArray(GetValues<double>(), DebuggerDisplayMaxCount).ToString(),
                _ => sb.ToString(),
            };
        }

        internal byte[] GetEncodedBytes()
        {
            using var buffer = new ArrayPoolBufferWriter<byte>();
            EncodeTo(buffer);
            return buffer.WrittenSpan.ToArray();
        }

        private sealed class EncodedItemDebugView
        {
            private readonly Item _item;
            public EncodedItemDebugView(Item item) => _item = item;
            public byte[] EncodedBytes => _item.GetEncodedBytes();
        }
    }
}
