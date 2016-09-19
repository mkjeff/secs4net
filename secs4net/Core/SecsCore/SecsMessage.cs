using System;
using System.Collections.Generic;

namespace Secs4Net
{
    [Serializable]
    public sealed class SecsMessage {
        static SecsMessage() {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("This version is only work on little endian hardware.");
        }

        public override string ToString() => $"'S{S}F{F}' {(ReplyExpected ? "W" : string.Empty)} {Name ?? string.Empty}";

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

        internal readonly Lazy<List<ArraySegment<byte>>> RawDatas;

        public IReadOnlyList<ArraySegment<byte>> RawBytes => RawDatas.Value.AsReadOnly();

        static readonly List<ArraySegment<byte>> emptyMsgDatas =
            new List<ArraySegment<byte>>
            {
                new ArraySegment<byte>(new byte[]{ 0, 0, 0, 10 }), // total length = header
                new ArraySegment<byte>(Array.Empty<byte>())        // header placeholder
            };

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
                throw new ArgumentOutOfRangeException(nameof(stream), stream, "Stream number must be less than 127");

            S = stream;
            F = function;
            Name = name;
            ReplyExpected = replyExpected;
            SecsItem = item;
            RawDatas = new Lazy<List<ArraySegment<byte>>>(() =>
            {
                if (SecsItem == null)
                    return emptyMsgDatas;

                var result = new List<ArraySegment<byte>> {
                    default(ArraySegment<byte>),    // total length
                    new ArraySegment<byte>(Array.Empty<byte>())     // header
                    // item
                };

                uint length = 10 + SecsItem.Encode(result); // total length = item + header
         
                byte[] msgLengthByte = BitConverter.GetBytes(length);
                Array.Reverse(msgLengthByte);
                result[0] = new ArraySegment<byte>(msgLengthByte);

                return result;
            });
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

            var itemLengthBytes = new byte[4];
            Array.Copy(bytes, index, itemLengthBytes, 0, lengthBits);
            Array.Reverse(itemLengthBytes, 0, lengthBits);
            int length = BitConverter.ToInt32(itemLengthBytes, 0);  // max to 3 byte length
            index += lengthBits;

            if (format == SecsFormat.List)
            {
                if (length == 0)
                    return Item.L();

                var list = new List<Item>(length);
                for (int i = 0; i < length; i++)
                    list.Add(Decode(bytes, ref index));
                return Item.L(list);
            }
            var item = length == 0 ? format.BytesDecode() : format.BytesDecode(new ArraySegment<byte>(bytes, index, length));
            index += length;
            return item;
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
