using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct ChunkedSpan<T>
    {
        private readonly Span<T> _source;
        private readonly int _chunkSize;
        private int _start;
        private int _end;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ChunkedSpan(Span<T> span, int size)
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
        public readonly ChunkedSpan<T> GetEnumerator()
            => this;

        /// <summary>
        /// Gets the duck-typed <see cref="System.Collections.Generic.IEnumerator{T}.Current" /> property.
        /// </summary>
        public readonly Span<T> Current
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
    public ref struct ChunkedReadOnlySpan<T>
    {
        private readonly ReadOnlySpan<T> _source;
        private readonly int _chunkSize;
        private int _start;
        private int _end;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ChunkedReadOnlySpan(ReadOnlySpan<T> span, int size)
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
        public readonly ChunkedReadOnlySpan<T> GetEnumerator()
            => this;

        /// <summary>
        /// Gets the duck-typed <see cref="System.Collections.Generic.IEnumerator{T}.Current" /> property.
        /// </summary>
        public readonly ReadOnlySpan<T> Current
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
}
