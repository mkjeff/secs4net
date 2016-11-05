using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Secs4Net.Properties;

namespace Secs4Net
{
    public sealed class SecsMessage:IDisposable
    {
        static SecsMessage()
        {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("This version is only work on little endian hardware.");
        }

        public override string ToString() => $"'S{S}F{F}' {(ReplyExpected ? "W" : string.Empty)} {Name ?? string.Empty}";
        public void Dispose()
        {
            SecsItem?.Dispose();
        }

        /// <summary>
        /// message stream number
        /// </summary>
        public byte S { get; }

        /// <summary>
        /// messge function number
        /// </summary>
        public byte F { get; }

        /// <summary>
        /// expect reply message
        /// </summary>
        public bool ReplyExpected { get; internal set; }

        /// <summary>
        /// the root item of message
        /// </summary>
        public Item SecsItem { get; }

        public string Name { get; set; }

        [Obsolete("This property only for debuging. Don't use in production.")]
        public IReadOnlyList<ArraySegment<byte>> RawBytes
        {
            get
            {
                var result =  new List<ArraySegment<byte>>();
                EncodeTo(result,
                         new MessageHeader(new ArraySegment<byte>(new byte[10]))
                         {
                             S = S,
                             F = F,
                             ReplyExpected = ReplyExpected,
                             DeviceId = 0,
                             SystemBytes = 0
                         });
                return result;
            }
        } 

        /// <summary>
        /// constructor of SecsMessage
        /// </summary>
        /// <param name="stream">message stream number</param>
        /// <param name="function">message function number</param>
        /// <param name="replyExpected">expect reply message</param>
        /// <param name="name"></param>
        /// <param name="item">root item</param>
        public SecsMessage(byte stream, byte function, bool replyExpected = true, string name = null, Item item = null)
        {
            if (stream > 0x7F)
                throw new ArgumentOutOfRangeException(nameof(stream),
                                                      stream,
                                                      Resources.SecsMessageStreamNumberMustLessThan127);

            S = stream;
            F = function;
            Name = name;
            ReplyExpected = replyExpected;
            SecsItem = item;
        }

        /// <summary>
        /// constructor of SecsMessage
        /// </summary>
        /// <param name="stream">message stream number</param>
        /// <param name="function">message function number</param>
        /// <param name="name"></param>
        /// <param name="item">root item</param>
        public SecsMessage(byte stream, byte function, string name, Item item = null)
            : this(stream, function, true, name, item)
        { }

        internal SecsMessage(byte stream, byte function, bool replyExpected, byte[] itemBytes, ref int index)
            : this(stream, function, replyExpected, string.Empty, Decode(itemBytes, ref index))
        { }


        static Item Decode(byte[] bytes, ref int index)
        {
            var format = (SecsFormat)(bytes[index] & 0xFC);
            var lengthBits = (byte)(bytes[index] & 3);
            index++;

            var itemLengthBytes = ArrayPool<byte>.Shared.Rent(4);
            Array.Copy(bytes, index, itemLengthBytes, 0, lengthBits);
            Array.Reverse(itemLengthBytes, 0, lengthBits);
            int length = BitConverter.ToInt32(itemLengthBytes, 0);  // max to 3 byte length
            ArrayPool<byte>.Shared.Return(itemLengthBytes);
            index += lengthBits;

            if (format == SecsFormat.List)
            {
                if (length == 0)
                    return Item.L();

                using (var list = ListItemDecoderBuffer.Create(length))
                {
                    for (int i = 0; i < length; i++)
                        list.Add(Decode(bytes, ref index));
                    return Item.L(list.Items);
                }
            }
            var item = length == 0 ? format.BytesDecode() : format.BytesDecode(bytes, ref index, ref length);
            index += length;
            return item;
        }

        internal void EncodeTo(IList<ArraySegment<byte>> buffer, ArraySegment<byte> header)
        {
            if (SecsItem == null)
            {
                buffer.Add(SecsGem.GetEmptyDataMessageLengthBytes()); // total length = header
                buffer.Add(header); // header placeholder
                return;
            }

            buffer.Add(default(ArraySegment<byte>)); // total length
            buffer.Add(header); // header
            // encode item
            var totalLength = 10 + SecsItem.EncodeTo(buffer); // total length = item + header

            var msgLengthByte = GetBytes(totalLength);
            Array.Reverse(msgLengthByte.Array);
            buffer[0] = msgLengthByte;
        }

        static unsafe ArraySegment<byte> GetBytes(uint value)
        {
            byte[] array = ArrayPool<byte>.Shared.Rent(4);
            fixed (byte* ptr = array)
            {
                *(uint*)ptr = value;
            }
            return new ArraySegment<byte>(array, 0, 4);
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
