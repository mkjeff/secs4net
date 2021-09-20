using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Secs4Net;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct ChunkedMemory<T>
{
    private readonly Memory<T> _source;
    private readonly int _chunkSize;
    private int _start;
    private int _end;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ChunkedMemory(Memory<T> span, int size)
    {
        _source = span;
        _chunkSize = size;
        _start = 0;
        _end = 0;
    }

    /// <summary>
    /// Implements the duck-typed <see cref="System.Collections.Generic.IEnumerable{T}" /> method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ChunkedMemory<T> GetEnumerator()
        => this;

    /// <summary>
    /// Gets the duck-typed <see cref="System.Collections.Generic.IEnumerator{T}.Current" /> property.
    /// </summary>
    public readonly Memory<T> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _source[_start.._end];
    }

    /// <summary>
    /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext" /> method.
    /// </summary>
    /// <returns><see langword="true" /> whether a new element is available, <see langword="false" /> otherwise</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        int length = _source.Length;
        if (_end < length)
        {
            _start = _end;
            _end = _start + _chunkSize;
            if (_end > length)
            {
                _end = length;
            }
            return true;
        }
        return false;
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]
public struct ChunkedReadOnlyMemory<T>
{
    private readonly ReadOnlyMemory<T> _source;
    private readonly int _chunkSize;
    private int _start;
    private int _end;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ChunkedReadOnlyMemory(ReadOnlyMemory<T> span, int size)
    {
        _source = span;
        _chunkSize = size;
        _start = 0;
        _end = 0;
    }

    /// <summary>
    /// Implements the duck-typed <see cref="System.Collections.Generic.IEnumerable{T}" /> method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ChunkedReadOnlyMemory<T> GetEnumerator()
        => this;

    /// <summary>
    /// Gets the duck-typed <see cref="System.Collections.Generic.IEnumerator{T}.Current" /> property.
    /// </summary>
    public readonly ReadOnlyMemory<T> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _source[_start.._end];
    }

    /// <summary>
    /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext" /> method.
    /// </summary>
    /// <returns><see langword="true" /> whether a new element is available, <see langword="false" /> otherwise</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        int length = _source.Length;
        if (_end < length)
        {
            _start = _end;
            _end = _start + _chunkSize;
            if (_end > length)
            {
                _end = length;
            }
            return true;
        }
        return false;
    }
}
