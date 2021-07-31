using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

namespace Secs4Net.Json
{
    // Taken from https://stackoverflow.com/questions/54983533/parsing-a-json-file-with-net-core-3-0-system-text-json
    // and fixed a few bugs with that
    internal ref struct Utf8JsonStreamReader
    {
        private readonly Stream _stream;
        // note: buffers will often be bigger than this - do not ever use this number for calculations.
        private readonly int _bufferSize;

        private SequenceSegment? _firstSegment;
        private int _firstSegmentStartIndex;
        private SequenceSegment? _lastSegment;
        private int _lastSegmentEndIndex;

        private Utf8JsonReader _jsonReader;
        private bool _keepBuffers;
        private bool _isFinalBlock;

        public Utf8JsonStreamReader(Stream stream, int bufferSize)
        {
            _stream = stream;
            _bufferSize = bufferSize;

            _firstSegment = null;
            _firstSegmentStartIndex = 0;
            _lastSegment = null;
            _lastSegmentEndIndex = -1;

            _jsonReader = default;
            _keepBuffers = false;
            _isFinalBlock = false;
        }

        public bool Read()
        {
            // read could be unsuccessful due to insufficient bufer size, retrying in loop with additional buffer segments
            while (!_jsonReader.Read())
            {
                if (_isFinalBlock)
                {
                    return false;
                }

                MoveNext();
            }
            return true;
        }

        private void MoveNext()
        {
            _firstSegmentStartIndex += (int)_jsonReader.BytesConsumed;

            // release previous segments if possible
            while (_firstSegmentStartIndex > 0 && _firstSegment?.Memory.Length <= _firstSegmentStartIndex)
            {
                var currFirstSegment = _firstSegment;
                _firstSegmentStartIndex -= _firstSegment.Memory.Length;
                _firstSegment = (SequenceSegment?)_firstSegment.Next;
                if (!_keepBuffers)
                {
                    currFirstSegment.Dispose();
                }
            }

            // create new segment
            var newSegment = new SequenceSegment(_bufferSize, _lastSegment);
            _lastSegment?.SetNext(newSegment);
            _lastSegment = newSegment;

            if (_firstSegment == null)
            {
                _firstSegment = newSegment;
                _firstSegmentStartIndex = 0;
            }

            // read data from stream
            _lastSegmentEndIndex = 0;
            int bytesRead;
            do
            {
#if NET
                bytesRead = _stream.Read(newSegment.Buffer.Memory.Span.Slice(_lastSegmentEndIndex));
#else
                var bytes = newSegment.Buffer.Memory.Span.Slice(_lastSegmentEndIndex).ToArray();
                bytesRead = _stream.Read(bytes, 0, bytes.Length);
#endif
                _lastSegmentEndIndex += bytesRead;
            } while (bytesRead > 0 && _lastSegmentEndIndex < newSegment.Buffer.Memory.Length);
            _isFinalBlock = _lastSegmentEndIndex < newSegment.Buffer.Memory.Length;
            var data = new ReadOnlySequence<byte>(_firstSegment, _firstSegmentStartIndex, _lastSegment,
                _lastSegmentEndIndex);
            _jsonReader =
                new Utf8JsonReader(data, _isFinalBlock, _jsonReader.CurrentState);
        }

        private void DeserialisePost()
        {
            // release memory if possible
            var firstSegment = _firstSegment;
            var firstSegmentStartIndex = _firstSegmentStartIndex + (int)_jsonReader.BytesConsumed;

            while (firstSegment?.Memory.Length < firstSegmentStartIndex)
            {
                firstSegmentStartIndex -= firstSegment.Memory.Length;
                firstSegment.Dispose();
                firstSegment = (SequenceSegment?)firstSegment.Next;
            }

            if (firstSegment != _firstSegment)
            {
                _firstSegment = firstSegment;
                _firstSegmentStartIndex = firstSegmentStartIndex;
                var data = new ReadOnlySequence<byte>(_firstSegment!, _firstSegmentStartIndex, _lastSegment!,
                    _lastSegmentEndIndex);
                _jsonReader =
                    new Utf8JsonReader(data, _isFinalBlock, _jsonReader.CurrentState);
            }
        }

        private long DeserialisePre(out SequenceSegment? firstSegment, out int firstSegmentStartIndex)
        {
            // JsonSerializer.Deserialize can read only a single object. We have to extract
            // object to be deserialized into separate Utf8JsonReader. This incurs one additional
            // pass through data (but data is only passed, not parsed).
            var tokenStartIndex = _jsonReader.TokenStartIndex;
            firstSegment = _firstSegment;
            firstSegmentStartIndex = _firstSegmentStartIndex;

            // loop through data until end of object is found
            _keepBuffers = true;
            int depth = 0;

            if (TokenType == JsonTokenType.StartObject || TokenType == JsonTokenType.StartArray)
                depth++;

            while (depth > 0 && Read())
            {
                if (TokenType == JsonTokenType.StartObject || TokenType == JsonTokenType.StartArray)
                    depth++;
                else if (TokenType == JsonTokenType.EndObject || TokenType == JsonTokenType.EndArray)
                    depth--;
            }

            _keepBuffers = false;
            return tokenStartIndex;
        }

        public T? Deserialise<T>(JsonSerializerOptions? options = null)
        {
            var tokenStartIndex = DeserialisePre(out var firstSegment, out var firstSegmentStartIndex);

            var newJsonReader =
                new Utf8JsonReader(new ReadOnlySequence<byte>(firstSegment!, firstSegmentStartIndex, _lastSegment!,
                    _lastSegmentEndIndex).Slice(tokenStartIndex, _jsonReader.Position), true, default);

            // deserialize value
            var result = JsonSerializer.Deserialize<T>(ref newJsonReader, options);

            DeserialisePost();
            return result;
        }

        public JsonDocument GetJsonDocument()
        {
            var tokenStartIndex = DeserialisePre(out var firstSegment, out var firstSegmentStartIndex);

            var newJsonReader =
                new Utf8JsonReader(new ReadOnlySequence<byte>(firstSegment!, firstSegmentStartIndex, _lastSegment!,
                    _lastSegmentEndIndex).Slice(tokenStartIndex, _jsonReader.Position), true, default);


            // deserialize value
            var result = JsonDocument.ParseValue(ref newJsonReader);
            DeserialisePost();
            return result;
        }

        public void Dispose() => _lastSegment?.Dispose();

        public int CurrentDepth => _jsonReader.CurrentDepth;
        public bool HasValueSequence => _jsonReader.HasValueSequence;
        public long TokenStartIndex => _jsonReader.TokenStartIndex;
        public JsonTokenType TokenType => _jsonReader.TokenType;
        public ReadOnlySequence<byte> ValueSequence => _jsonReader.ValueSequence;
        public ReadOnlySpan<byte> ValueSpan => _jsonReader.ValueSpan;

        public bool GetBoolean() => _jsonReader.GetBoolean();
        public byte GetByte() => _jsonReader.GetByte();
        public byte[] GetBytesFromBase64() => _jsonReader.GetBytesFromBase64();
        public string GetComment() => _jsonReader.GetComment();
        public DateTime GetDateTime() => _jsonReader.GetDateTime();
        public DateTimeOffset GetDateTimeOffset() => _jsonReader.GetDateTimeOffset();
        public decimal GetDecimal() => _jsonReader.GetDecimal();
        public double GetDouble() => _jsonReader.GetDouble();
        public Guid GetGuid() => _jsonReader.GetGuid();
        public short GetInt16() => _jsonReader.GetInt16();
        public int GetInt32() => _jsonReader.GetInt32();
        public long GetInt64() => _jsonReader.GetInt64();
        public sbyte GetSByte() => _jsonReader.GetSByte();
        public float GetSingle() => _jsonReader.GetSingle();
        public string? GetString() => _jsonReader.GetString();
        public uint GetUInt32() => _jsonReader.GetUInt32();
        public ulong GetUInt64() => _jsonReader.GetUInt64();
        public bool TryGetDecimal(out byte value) => _jsonReader.TryGetByte(out value);
        public bool TryGetBytesFromBase64([NotNullWhen(true)] out byte[]? value) => _jsonReader.TryGetBytesFromBase64(out value);
        public bool TryGetDateTime(out DateTime value) => _jsonReader.TryGetDateTime(out value);
        public bool TryGetDateTimeOffset(out DateTimeOffset value) => _jsonReader.TryGetDateTimeOffset(out value);
        public bool TryGetDecimal(out decimal value) => _jsonReader.TryGetDecimal(out value);
        public bool TryGetDouble(out double value) => _jsonReader.TryGetDouble(out value);
        public bool TryGetGuid(out Guid value) => _jsonReader.TryGetGuid(out value);
        public bool TryGetInt16(out short value) => _jsonReader.TryGetInt16(out value);
        public bool TryGetInt32(out int value) => _jsonReader.TryGetInt32(out value);
        public bool TryGetInt64(out long value) => _jsonReader.TryGetInt64(out value);
        public bool TryGetSByte(out sbyte value) => _jsonReader.TryGetSByte(out value);
        public bool TryGetSingle(out float value) => _jsonReader.TryGetSingle(out value);
        public bool TryGetUInt16(out ushort value) => _jsonReader.TryGetUInt16(out value);
        public bool TryGetUInt32(out uint value) => _jsonReader.TryGetUInt32(out value);
        public bool TryGetUInt64(out ulong value) => _jsonReader.TryGetUInt64(out value);

        private sealed class SequenceSegment : ReadOnlySequenceSegment<byte>, IDisposable
        {
            internal IMemoryOwner<byte> Buffer { get; }
            internal SequenceSegment? Previous { get; set; }
            private bool _disposed;

            public SequenceSegment(int size, SequenceSegment? previous)
            {
                Buffer = MemoryPool<byte>.Shared.Rent(size);
                Previous = previous;

                Memory = Buffer.Memory;
                RunningIndex = previous?.RunningIndex + previous?.Memory.Length ?? 0;
            }

            public void SetNext(SequenceSegment next) => Next = next;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    Buffer.Dispose();
                    Previous?.Dispose();
                }
            }
        }
    }
}
