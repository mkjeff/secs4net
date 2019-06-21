using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Secs4Net
{
	/// <summary>
	///  Stream based HSMS/SECS-II message decoder
	/// </summary>
	internal sealed class StreamDecoder
	{
		public byte[] Buffer => this._buffer;

		/// <summary>
		/// Control the range of data receiver 
		/// </summary>
		public int BufferOffset => this._bufferOffset;
		public int BufferCount => this.Buffer.Length - this._bufferOffset;

		/// <summary>
		/// decoder step
		/// </summary>
		/// <param name="length"></param>
		/// <param name="need"></param>
		/// <returns>pipeline decoder index</returns>
		private delegate int Decoder(ref int length, out int need);

		/// <summary>
		/// decode pipelines
		/// </summary>
		private readonly Decoder[] _decoders;

		private int _decoderStep;

		private readonly Action<MessageHeader, SecsMessage> _dataMsgHandler;
		private readonly Action<MessageHeader> _controlMsgHandler;

		/// <summary>
		/// data buffer
		/// </summary>
		private byte[] _buffer;

		/// <summary>
		/// Control the range of data decoder
		/// </summary>
		private int _decodeIndex;


		private int _bufferOffset;

		/// <summary>
		/// previous decoded remained count
		/// </summary>
		private int _previousRemainedCount;

		private readonly Stack<List<Item>> _stack = new Stack<List<Item>>();
		private uint _messageDataLength;
		private MessageHeader _msgHeader;
		private readonly byte[] _itemLengthBytes = new byte[4];
		private SecsFormat _format;
		private byte _lengthBits;
		private int _itemLength;

		public void Reset()
		{
			this._stack.Clear();
			this._decoderStep = 0;
			this._decodeIndex = 0;
			this._bufferOffset = 0;
			this._messageDataLength = 0;
			this._previousRemainedCount = 0;
		}

		internal StreamDecoder(in int streamBufferSize, Action<MessageHeader> controlMsgHandler, Action<MessageHeader, SecsMessage> dataMsgHandler)
		{
			this._buffer = new byte[streamBufferSize];
			this._bufferOffset = 0;
			this._decodeIndex = 0;
			this._dataMsgHandler = dataMsgHandler;
			this._controlMsgHandler = controlMsgHandler;

			this._decoders = new Decoder[]
			{
				GetTotalMessageLength,
				GetMessageHeader,
				GetItemHeader,
				GetItemLength,
				GetItem,
			};

			// 0: get total message length 4 bytes
			int GetTotalMessageLength(ref int length, out int need)
			{
				if (!CheckAvailable(length, 4, out need))
				{
					return 0;
				}

				Array.Reverse(this._buffer, this._decodeIndex, 4);
				this._messageDataLength = BitConverter.ToUInt32(this._buffer, this._decodeIndex);
				Trace.WriteLine($"Get Message Length: {this._messageDataLength}");
				this._decodeIndex += 4;
				length -= 4;
				return GetMessageHeader(ref length, out need);
			}

			// 1: get message header 10 bytes
			int GetMessageHeader(ref int length, out int need)
			{
				if (!CheckAvailable(length, 10, out need))
				{
					return 1;
				}

				this._msgHeader = MessageHeader.Decode(new ReadOnlySpan<byte>(this._buffer, this._decodeIndex, 10));
				this._decodeIndex += 10;
				this._messageDataLength -= 10;
				length -= 10;
				if (this._messageDataLength == 0)
				{
					if (this._msgHeader.MessageType == MessageType.DataMessage)
					{
						this._dataMsgHandler(this._msgHeader, new SecsMessage(this._msgHeader.S, this._msgHeader.F, string.Empty, replyExpected: this._msgHeader.ReplyExpected));
					}
					else
					{
						this._controlMsgHandler(this._msgHeader);
					}

					return 0;
				}

				if (length >= this._messageDataLength)
				{
					Trace.WriteLine("Get Complete Data Message with total data");
					this._dataMsgHandler(this._msgHeader, new SecsMessage(this._msgHeader.S, this._msgHeader.F, string.Empty, BufferedDecodeItem(this._buffer, ref this._decodeIndex), this._msgHeader.ReplyExpected));
					length -= (int)this._messageDataLength;
					this._messageDataLength = 0;
					return 0; //completeWith message received
				}
				return GetItemHeader(ref length, out need);
			}

			// 2: get _format + lengthBits(2bit) 1 byte
			int GetItemHeader(ref int length, out int need)
			{
				if (!CheckAvailable(length, 1, out need))
				{
					return 2;
				}

				this._format = (SecsFormat)(this._buffer[this._decodeIndex] & 0xFC);
				this._lengthBits = (byte)(this._buffer[this._decodeIndex] & 3);
				this._decodeIndex++;
				this._messageDataLength--;
				length--;
				return GetItemLength(ref length, out need);
			}

			// 3: get _itemLength _lengthBits bytes, at most 3 byte
			int GetItemLength(ref int length, out int need)
			{
				if (!CheckAvailable(length, this._lengthBits, out need))
				{
					return 3;
				}

				Array.Copy(this._buffer, this._decodeIndex, this._itemLengthBytes, 0, this._lengthBits);
				Array.Reverse(this._itemLengthBytes, 0, this._lengthBits);

				this._itemLength = BitConverter.ToInt32(this._itemLengthBytes, 0);
				Array.Clear(this._itemLengthBytes, 0, 4);
				Trace.WriteLineIf(this._format != SecsFormat.List, $"Get format: {this._format}, length: {this._itemLength}");

				this._decodeIndex += this._lengthBits;
				this._messageDataLength -= this._lengthBits;
				length -= this._lengthBits;
				return GetItem(ref length, out need);
			}

			// 4: get item value
			int GetItem(ref int length, out int need)
			{
				need = 0;
				Item item;
				if (this._format == SecsFormat.List)
				{
					if (this._itemLength == 0)
					{
						item = Item.L();
					}
					else
					{
						this._stack.Push(new List<Item>(this._itemLength));
						return GetItemHeader(ref length, out need);
					}
				}
				else
				{
					if (!CheckAvailable(length, this._itemLength, out need))
					{
						return 4;
					}

					item = Item.BytesDecode(this._format, this._buffer, this._decodeIndex, this._itemLength);
					Trace.WriteLine($"Complete Item: {this._format}");

					this._decodeIndex += this._itemLength;
					this._messageDataLength -= (uint)this._itemLength;
					length -= this._itemLength;
				}

				if (this._stack.Count == 0)
				{
					Trace.WriteLine("Get Complete Data Message by stream decoded");
					this._dataMsgHandler(this._msgHeader, new SecsMessage(this._msgHeader.S, this._msgHeader.F, string.Empty, item, this._msgHeader.ReplyExpected));
					return 0;
				}

				var list = this._stack.Peek();
				list.Add(item);
				while (list.Count == list.Capacity)
				{
					item = Item.L(this._stack.Pop());
					Trace.WriteLine($"Complete List: {item.Count}");
					if (this._stack.Count > 0)
					{
						list = this._stack.Peek();
						list.Add(item);
					}
					else
					{
						Trace.WriteLine("Get Complete Data Message by stream decoded");
						this._dataMsgHandler(this._msgHeader, new SecsMessage(this._msgHeader.S, this._msgHeader.F, string.Empty, item, this._msgHeader.ReplyExpected));
						return 0;
					}
				}

				return GetItemHeader(ref length, out need);
			}

			bool CheckAvailable(in int length, in int required, out int need)
			{
				need = required - length;
				if (need > 0)
				{
					return false;
				}
				need = 0;
				return true;
			}

			Item BufferedDecodeItem(byte[] bytes, ref int index)
			{
				var format = (SecsFormat)(bytes[index] & 0xFC);
				var lengthBits = (byte)(bytes[index] & 3);
				index++;

				var itemLengthBytes = new byte[4];
				Array.Copy(bytes, index, itemLengthBytes, 0, lengthBits);
				Array.Reverse(itemLengthBytes, 0, lengthBits);
				int dataLength = BitConverter.ToInt32(itemLengthBytes, 0); // max to 3 byte dataLength
				index += lengthBits;

				if (format == SecsFormat.List)
				{
					if (dataLength == 0)
					{
						return Item.L();
					}

					var list = new List<Item>(dataLength);
					for (var i = 0; i < dataLength; i++)
					{
						list.Add(BufferedDecodeItem(bytes, ref index));
					}

					return Item.L(list);
				}
				var item = Item.BytesDecode(format, bytes, index, dataLength);
				index += dataLength;
				return item;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="length">data length</param>
		/// <returns>true, if need more data to decode completed message. otherwise, return false</returns>
		public bool Decode(int length)
		{
			Debug.Assert(length > 0, "decode data length is 0.");
			var decodeLength = length;
			length += this._previousRemainedCount; // total available length = current length + previous remained
			int need;
			var nexStep = this._decoderStep;
			do
			{
				this._decoderStep = nexStep;
				nexStep = this._decoders[this._decoderStep](ref length, out need);
			} while (nexStep != this._decoderStep);

			Debug.Assert(this._decodeIndex >= this._bufferOffset, "decode index should ahead of buffer index");

			var remainCount = length;
			Debug.Assert(remainCount >= 0, "remain count is only possible grater and equal zero");
			Trace.WriteLine($"remain data length: {remainCount}");
			Trace.WriteLineIf(this._messageDataLength > 0, $"need data count: {need}");

			if (remainCount == 0)
			{
				if (need > this.Buffer.Length)
				{
					var newSize = need * 2;
					Trace.WriteLine($@"<<buffer resizing>>: current size = {this._buffer.Length}, new size = {newSize}");

					// increase buffer size
					this._buffer = new byte[newSize];
				}
				this._bufferOffset = 0;
				this._decodeIndex = 0;
				this._previousRemainedCount = 0;
			}
			else
			{
				this._bufferOffset += decodeLength; // move next receive index
				var nextStepReqiredCount = remainCount + need;
				if (nextStepReqiredCount > this.BufferCount)
				{
					if (nextStepReqiredCount > this.Buffer.Length)
					{
						var newSize = Math.Max(this._messageDataLength / 2, nextStepReqiredCount) * 2;
						Trace.WriteLine($@"<<buffer resizing>>: current size = {this._buffer.Length}, remained = {remainCount}, new size = {newSize}");

						// out of total buffer size
						// increase buffer size
						var newBuffer = new byte[newSize];
						// keep remained data to new buffer's head
						Array.Copy(this._buffer, this._bufferOffset - remainCount, newBuffer, 0, remainCount);
						this._buffer = newBuffer;
					}
					else
					{
						Trace.WriteLine($@"<<buffer recyling>>: available = {this.BufferCount}, need = {nextStepReqiredCount}, remained = {remainCount}");

						// move remained data to buffer's head
						Array.Copy(this._buffer, this._bufferOffset - remainCount, this._buffer, 0, remainCount);
					}
					this._bufferOffset = remainCount;
					this._decodeIndex = 0;
				}
				this._previousRemainedCount = remainCount;
			}

			return this._messageDataLength > 0;
		}
	}
}