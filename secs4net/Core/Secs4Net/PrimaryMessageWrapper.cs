using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
	public sealed class PrimaryMessageWrapper
	{
		private static readonly Task<bool> replyAsyncFalseCache = Task.FromResult(false);

		private static readonly Task<bool> replyAsyncTrueCache = Task.FromResult(true);

		private readonly MessageHeader header;

		private readonly WeakReference<SecsGem> weakReferenceSecsGem;

		private int isReplied = 0;

		internal PrimaryMessageWrapper(SecsGem secsGem, MessageHeader header, SecsMessage message)
		{
			this.weakReferenceSecsGem = new WeakReference<SecsGem>(secsGem);
			this.header = header;
			this.Message = message;
		}

		public SecsMessage Message { get; }

		public int MessageId => this.header.SystemBytes;

		/// <summary>
		/// Each PrimaryMessageWrapper can invoke Reply method once.
		/// If the message already replied, will return false.
		/// </summary>
		/// <param name="replyMessage">The <see cref="Message"/></param>
		/// <returns><see langword="true"/>, if reply message sent; otherwise <see langword="false"/>.</returns>
		public Task<bool> ReplyAsync(SecsMessage replyMessage)
		{
			if (Interlocked.Exchange(ref this.isReplied, 1) == 1)
			{
				return PrimaryMessageWrapper.replyAsyncFalseCache;
			}

			SecsGem secsGem;
			if (!this.Message.ReplyExpected
				|| !this.weakReferenceSecsGem.TryGetTarget(out secsGem))
			{
				return PrimaryMessageWrapper.replyAsyncTrueCache;
			}

			if (replyMessage == null)
			{
				replyMessage = new SecsMessage(9, 7, "Unknown Message", Item.B(this.header.EncodeTo(new byte[10])), replyExpected: false);
			}
			else
			{
				replyMessage.ReplyExpected = false;
			}

			return secsGem
				.SendDataMessageAsync(replyMessage, replyMessage.S == 9 ? secsGem.GetNewSystemId() : this.header.SystemBytes)
				.ContinueWith((_) => true);
		}

		public override string ToString() => this.Message.ToString();
	}
}