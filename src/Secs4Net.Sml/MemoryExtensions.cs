using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System;

internal delegate TResult SpanParser<TResult>(ReadOnlySpan<char> span);

//https://github.com/bbartels/coreclr/blob/master/src/System.Private.CoreLib/shared/System/MemoryExtensions.Split.cs
// https://github.com/dotnet/runtime/pull/295

internal static partial class MemoryExtensions
{
    /// <summary>
    /// Returns an enumerator that iterates through a <see cref="ReadOnlySpan{T}"/>,
    /// which is split by separator <paramref name="separator"/>.
    /// </summary>
    /// <param name="span">The source span which should be iterated over.</param>
    /// <param name="separator">The separator used to separate the <paramref name="span"/>.</param>
    /// <param name="options">The <see cref="StringSplitOptions"/> which should be applied with this operation.</param>
    /// <returns>Returns an enumerator for the specified sequence.</returns>
    public static SpanSplitEnumerator<char> Split(in this ReadOnlySpan<char> span, char separator, StringSplitOptions options = StringSplitOptions.None)
        => new(span, separator, options == StringSplitOptions.RemoveEmptyEntries);

    public static bool IsEmpty<T>(this SpanSplitEnumerator<T> source) where T : unmanaged, IEquatable<T>
        => !source.MoveNext();

    public static TResult[] ToArray<TResult>(ref this SpanSplitEnumerator<char> source, SpanParser<TResult> selector, int? size)
    {
        if (size.HasValue)
        {
            var list = new TResult[size.GetValueOrDefault()];
            ref var r0 = ref MemoryMarshal.GetReference(list.AsSpan());
            uint i = 0;
            foreach (var span in source)
            {
                Unsafe.Add(ref r0, i++) = selector.Invoke(span);
                if (i == list.Length)
                {
                    break;
                }
            }

            return list;
        }
        else
        {
            var list = new List<TResult>();
            foreach (var span in source)
            {
                list.Add(selector.Invoke(span));
            }

            return list.ToArray();
        }
    }
}

internal ref struct SpanSplitEnumerator<T> where T : unmanaged, IEquatable<T>
{
    private ReadOnlySpan<T> _sequence;
    private readonly T _separator;
    private SpanSplitInfo _spanSplitInfo;

    private bool ShouldRemoveEmptyEntries => _spanSplitInfo.HasFlag(SpanSplitInfo.RemoveEmptyEntries);
    private bool IsFinished => _spanSplitInfo.HasFlag(SpanSplitInfo.FinishedEnumeration);

    /// <summary>
    /// Gets the element at the current position of the enumerator.
    /// </summary>
    public ReadOnlySpan<T> Current { get; private set; }

    /// <summary>
    /// Returns the current enumerator.
    /// </summary>
    /// <returns>Returns the current enumerator.</returns>
    public SpanSplitEnumerator<T> GetEnumerator() => this;

    internal SpanSplitEnumerator(ReadOnlySpan<T> span, T separator, bool removeEmptyEntries)
    {
        Current = default;
        _sequence = span;
        _separator = separator;
        _spanSplitInfo = default(SpanSplitInfo) | (removeEmptyEntries ? SpanSplitInfo.RemoveEmptyEntries : 0);
    }

    /// <summary>
    /// Advances the enumerator to the next element in the <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <returns>Returns whether there is another item in the enumerator.</returns>
    public bool MoveNext()
    {
        if (IsFinished) { return false; }

        do
        {
            int index = _sequence.IndexOf(_separator);
            if (index < 0)
            {
                Current = _sequence;
                _spanSplitInfo |= SpanSplitInfo.FinishedEnumeration;
                return !(ShouldRemoveEmptyEntries && Current.IsEmpty);
            }

            Current = _sequence[..index];
            _sequence = _sequence[(index + 1)..];
        } while (Current.IsEmpty && ShouldRemoveEmptyEntries);

        return true;
    }

    [Flags]
    private enum SpanSplitInfo : byte
    {
        RemoveEmptyEntries = 0x1,
        FinishedEnumeration = 0x2
    }
}
