using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct SpanChunked<T>
    {
        private readonly Span<T> _span;
        private readonly int _chunkSize;
        private int _start;
        private int _end;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpanChunked{T}" /> struct.
        /// </summary>
        /// <param name="span">The source <see cref="Span{T}"/> instance.</param>
        /// <param name="size">Max size of chunked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanChunked(Span<T> span, int size)
        {
            _span = span;
            _chunkSize = size;
            _start = 0;
            _end = 0;
        }

        /// <summary>
        /// Implements the duck-typed <see cref="System.Collections.Generic.IEnumerable{T}" /> method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SpanChunked<T> GetEnumerator()
            => this;

        /// <summary>
        /// Gets the duck-typed <see cref="System.Collections.Generic.IEnumerator{T}.Current" /> property.
        /// </summary>
        public readonly Span<T> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _span[_start.._end];
        }

        /// <summary>
        /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext" /> method.
        /// </summary>
        /// <returns><see langword="true" /> whether a new element is available, <see langword="false" /> otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            int length = _span.Length;
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
    public ref struct ReadOnlySpanChunk<T>
    {
        private readonly ReadOnlySpan<T> _span;
        private readonly int _chunkSize;
        private int _start;
        private int _end;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySpanChunk{T}" /> struct.
        /// </summary>
        /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> instance.</param>
        /// <param name="size">Max size of chunked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpanChunk(ReadOnlySpan<T> span, int size)
        {
            _span = span;
            _chunkSize = size;
            _start = 0;
            _end = 0;
        }

        /// <summary>
        /// Implements the duck-typed <see cref="System.Collections.Generic.IEnumerable{T}" /> method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpanChunk<T> GetEnumerator()
            => this;

        /// <summary>
        /// Gets the duck-typed <see cref="System.Collections.Generic.IEnumerator{T}.Current" /> property.
        /// </summary>
        public readonly ReadOnlySpan<T> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _span[_start.._end];
        }

        /// <summary>
        /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext" /> method.
        /// </summary>
        /// <returns><see langword="true" /> whether a new element is available, <see langword="false" /> otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            int length = _span.Length;
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
}
