using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Secs4Net
{
	public sealed class SecsMessage : ISerializable
	{
		static SecsMessage()
		{
			if (!BitConverter.IsLittleEndian)
				throw new PlatformNotSupportedException("This version is only work on little endian hardware.");
		}

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
		public Item SecsItem { get; }

		public string Name { get; set; }

		internal readonly Lazy<List<ArraySegment<byte>>> RawDatas;

		public IReadOnlyList<ArraySegment<byte>> RawBytes => RawDatas.Value.AsReadOnly();

		private static readonly List<ArraySegment<byte>> EmptyMsgDatas =
			new List<ArraySegment<byte>>
			{
				new ArraySegment<byte>(new byte[]{ 0, 0, 0, 10 }), // total length = header
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
		public SecsMessage(byte s, byte f, string name = null, Item item = null, bool replyExpected = true)
		{
			if (s > 0b0111_1111)
				throw new ArgumentOutOfRangeException(nameof(s), s, Resources.SecsMessageStreamNumberMustLessThan127);

			S = s;
			F = f;
			Name = name;
			ReplyExpected = replyExpected;
			SecsItem = item;
			RawDatas = new Lazy<List<ArraySegment<byte>>>(() =>
			{
				if (SecsItem is null)
					return EmptyMsgDatas;

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

		//Binary Serialization
		protected SecsMessage(SerializationInfo info, StreamingContext context)
		{
			S = info.GetByte(nameof(S));
			F = info.GetByte(nameof(F));
			ReplyExpected = info.GetBoolean(nameof(ReplyExpected));
			Name = info.GetString(nameof(Name));
			//_rawDatas = Lazy.Create(info.GetValue(nameof(_rawDatas), typeof(ReadOnlyCollection<RawData>)) as ReadOnlyCollection<RawData>);
			//int i = 0;
			//if (_rawDatas.Value.Count > 2)
			//	SecsItem = Decode(_rawDatas.Value.Skip(2).SelectMany(arr => arr.Bytes).ToArray(), ref i);
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(S), S);
			info.AddValue(nameof(F), F);
			info.AddValue(nameof(ReplyExpected), ReplyExpected);
			info.AddValue(nameof(Name), Name);
			//info.AddValue(nameof(_rawDatas), _rawDatas.Value);
		}
	}
}
