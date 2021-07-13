using System;
using System.Collections.Generic;
using System.Text;

namespace Secs4Net
{
    public sealed class SecsMessage
    {
        public override string ToString() => $"'S{S}F{F}' {(ReplyExpected ? "W" : string.Empty)} {Name ?? string.Empty}";

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
        public Item? SecsItem { get; init; }

        public string? Name { get; set; }

        internal readonly Lazy<List<ArraySegment<byte>>> RawDatas;

        public IReadOnlyList<ArraySegment<byte>> RawBytes => RawDatas.Value.AsReadOnly();

        private static readonly List<ArraySegment<byte>> EmptyMsgDatas =
            new()
            {
                new ArraySegment<byte>(new byte[] { 0, 0, 0, 10 }), // total length = header
                new ArraySegment<byte>(Array.Empty<byte>())        // header placeholder
            };

        /// <summary>
        /// constructor of SecsMessage
        /// </summary>
        /// <param name="s">message stream number</param>
        /// <param name="f">message function number</param>
        /// <param name="replyExpected">expect reply message</param>
        /// <param name="name"></param>
        /// <param name="item">root item</param>
        public SecsMessage(byte s, byte f, bool replyExpected = true)
        {
            if (s > 0b0111_1111)
            {
                throw new ArgumentOutOfRangeException(nameof(s), s, Resources.SecsMessageStreamNumberMustLessThan127);
            }

            S = s;
            F = f;
            ReplyExpected = replyExpected;
            RawDatas = new Lazy<List<ArraySegment<byte>>>(() =>
            {
                if (SecsItem is null)
                {
                    return EmptyMsgDatas;
                }

                var result = new List<ArraySegment<byte>> {
                    default,    // total length
                    new ArraySegment<byte>(Array.Empty<byte>())     // header
                    // item
                };

                var length = 10 + SecsItem.EncodeTo(result); // total length = item + header

                var msgLengthByte = BitConverter.GetBytes(length);
                Array.Reverse(msgLengthByte);
                result[0] = new ArraySegment<byte>(msgLengthByte);

                return result;
            });
        }
    }
}
