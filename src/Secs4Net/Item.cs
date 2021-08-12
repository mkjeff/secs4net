using Microsoft.Toolkit.HighPerformance.Buffers;
using Secs4Net.Extensions;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net;

[DebuggerDisplay("{GetDebugString()}")]
[DebuggerTypeProxy(typeof(EncodedItemDebugView))]
public abstract partial class Item : IEquatable<Item>, IEnumerable<Item>, IDisposable
{
    private const int DebuggerDisplayMaxCount = 20;
    public static readonly Encoding Jis8Encoding = Encoding.GetEncoding(50222);
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

    private protected Item(SecsFormat format)
    {
        Format = format;
    }

    public abstract int Count { get; }

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
        get => throw CreateNotSupportException(Format);
        set => throw CreateNotSupportException(Format);
    }

    /// <summary>
    /// Forms a slice out of the current List starting at a specified index for a specified length.
    /// </summary>
    /// <param name="start">The index at which to begin this slice.</param>
    /// <param name="length">The desired length for the slice</param>
    /// <exception cref="NotSupportedException">When the item's <see cref="Format"/> is not <see cref="SecsFormat.List"/></exception>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public virtual IEnumerable<Item> Slice(int start, int length)
        => throw CreateNotSupportException(Format);

    /// <summary>
    /// Get the first element of item array value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException">When item is empty or data length less than sizeof(<typeparamref name="T"/>)</exception>
    /// <exception cref="NotSupportedException">when the item's <see cref="Format"/> is <see cref="SecsFormat.List"/> or <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
    public virtual ref T FirstValue<T>() where T : unmanaged
        => throw CreateNotSupportException(Format);

    /// <summary>
    /// Get the first element of item array value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">when <see cref="Format"/> is <see cref="SecsFormat.List"/> or <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
    public virtual T FirstValueOrDefault<T>(T defaultValue = default) where T : unmanaged
        => throw CreateNotSupportException(Format);

    /// <summary>
    /// Get item array as <see cref="Memory{T}"/>
    /// </summary>
    /// <exception cref="NotSupportedException">when <see cref="Format"/> is <see cref="SecsFormat.List"/> or <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
    public virtual Memory<T> GetMemory<T>() where T : unmanaged
        => throw CreateNotSupportException(Format);

    /// <summary>
    /// Get item string value
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">when the <see cref="Format"/> is not <see cref="SecsFormat.ASCII"/> or <see cref="SecsFormat.JIS8"/></exception>
    public virtual string GetString()
        => throw CreateNotSupportException(Format);

    public virtual IEnumerator<Item> GetEnumerator()
        => throw CreateNotSupportException(Format);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static NotSupportedException CreateNotSupportException(SecsFormat format, [CallerMemberName] string? memberName = null)
        => new($"{memberName} is not supported, since the item's {nameof(Format)} is {format}");

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual void Dispose() { }

    public static bool operator !=(Item? r1, Item? r2)
        => !(r1 == r2);

    public static bool operator ==(Item? r1, Item? r2)
        => ReferenceEquals(r1, r2) || (r1?.Equals(r2) ?? false);

    public sealed override bool Equals(object? obj)
        => Equals(obj as Item);

    public bool Equals(Item? other)
        => other is not null && IsEquals(other);

    private protected abstract bool IsEquals(Item other);

    public sealed override string ToString() => $"{Format.GetName()}[{Count}]";

    private string GetDebugString() => this.GetDebugString(DebuggerDisplayMaxCount);

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
