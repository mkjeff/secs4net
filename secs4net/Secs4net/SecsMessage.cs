using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Secs4Net
{
    public sealed class SecsMessage : IDisposable
    {
        static SecsMessage()
        {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("This version is only work on little endian hardware.");
        }

        public override string ToString() => $"'S{S}F{F}' {(ReplyExpected ? "W" : string.Empty)} {Name}";

        private int _isDisposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
                return;

            SecsItem?.Release();
        }

        /// <summary>
        /// message stream number
        /// </summary>
        public byte S { get; }

        /// <summary>
        /// message function number
        /// </summary>
        public byte F { get; }

        /// <summary>
        /// expect reply message
        /// </summary>
        public bool ReplyExpected { get; internal set; }

        /// <summary>
        /// the root item of message
        /// </summary>
        public SecsItem SecsItem { get; }

        public string Name { get; set; }

        [Browsable(false),Obsolete("This property only for debugging. Don't use in production.")]
        public IList<ArraySegment<byte>> RawBytes =>
            EncodeTo(
                new List<ArraySegment<byte>>(),
                new ArraySegment<byte>(new MessageHeader
                                       {
                                           S = S,
                                           F = F,
                                           ReplyExpected = ReplyExpected
                                       }.EncodeTo(new byte[10])));

        /// <summary>
        /// constructor of SecsMessage
        /// </summary>
        /// <param name="s">message stream number</param>
        /// <param name="f">message function number</param>
        /// <param name="replyExpected">expect reply message</param>
        /// <param name="name"></param>
        /// <param name="item">root item</param>
        public SecsMessage(byte s, byte f, bool replyExpected = true, string name = null, SecsItem item = null)
        {
            if (s > 0b0111_1111)
                throw new ArgumentOutOfRangeException(nameof(s),
                                                      s,
                                                      Resources.SecsMessageStreamNumberMustLessThan127);

            S = s;
            F = f;
            Name = name;
            ReplyExpected = replyExpected;
            SecsItem = item;
        }

        /// <summary>
        /// constructor of SecsMessage
        /// </summary>
        /// <param name="s">message stream number</param>
        /// <param name="f">message function number</param>
        /// <param name="name"></param>
        /// <param name="item">root item</param>
        public SecsMessage(byte s, byte f, string name = null, SecsItem item = null)
            : this(s, f, true, name, item)
        { }

        internal IList<ArraySegment<byte>> EncodeTo(IList<ArraySegment<byte>> buffer, ArraySegment<byte> header)
        {
            if (SecsItem is null)
            {
                buffer.Add(GetEmptyDataMessageLengthBytes()); // total length = header
                buffer.Add(header); // header
                return buffer;
            }

            buffer.Add(default(ArraySegment<byte>)); // total length
            buffer.Add(header); // header
            // encode item
            var totalLength = 10 + SecsItem.EncodeTo(buffer); // total length = item + header

            // encode total length
            var msgLengthByte = SecsGem.EncodedBytePool.Rent(4);
            unsafe
            {
                Unsafe.Write(Unsafe.AsPointer(ref msgLengthByte[0]), totalLength);
            }

            Array.Reverse(msgLengthByte, 0, 4);
            buffer[0] = new ArraySegment<byte>(msgLengthByte, 0, 4);
            return buffer;
        }

        private static ArraySegment<byte> GetEmptyDataMessageLengthBytes()
        {
            var lengthBytes = SecsGem.EncodedBytePool.Rent(4);
            lengthBytes[0] = 0;
            lengthBytes[1] = 0;
            lengthBytes[2] = 0;
            lengthBytes[3] = 10;
            return new ArraySegment<byte>(lengthBytes, 0, 4);
        }

        #region ISerializable Members
        ////Binary Serialization
        //SecsMessage(SerializationInfo info, StreamingContext context)
        //{
        //    S = info.GetByte(nameof(S));
        //    F = info.GetByte(nameof(F));
        //    ReplyExpected = info.GetBoolean(nameof(ReplyExpected));
        //    Name = info.GetString(nameof(Name));
        //    _rawDatas = Lazy.Create(info.GetValue(nameof(_rawDatas), typeof(ReadOnlyCollection<RawData>)) as ReadOnlyCollection<RawData>);
        //    int i = 0;
        //    if (_rawDatas.Value.Count > 2)
        //        SecsItem = Decode(_rawDatas.Value.Skip(2).SelectMany(arr => arr.Bytes).ToArray(), ref i);
        //}

        //[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
        //    info.AddValue(nameof(S), S);
        //    info.AddValue(nameof(F), F);
        //    info.AddValue(nameof(ReplyExpected), ReplyExpected);
        //    info.AddValue(nameof(Name), Name);
        //    info.AddValue(nameof(_rawDatas), _rawDatas.Value);
        //}
        #endregion
    }
}
