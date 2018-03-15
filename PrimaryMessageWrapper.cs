﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PrimaryMessageWrapper
    {
        private static readonly Task<bool> ReplyAsyncTrueCache = Task.FromResult(true);
        private static readonly Task<bool> ReplyAsyncFalseCache = Task.FromResult(false);
        private static readonly Func<Task<SecsMessage>, bool> ContinueWithFunc = ContinueWithSuccess;
        private static bool ContinueWithSuccess(Task<SecsMessage> _) => true;

        private int _isReplied = 0;
        private readonly WeakReference<SecsGem> _secsGem;
        private readonly MessageHeader _header;
        public SecsMessage Message { get; }
        public ref readonly int MessageId => ref _header.SystemBytes;

        internal PrimaryMessageWrapper(in SecsGem secsGem, in MessageHeader header, in SecsMessage msg)
        {
            _secsGem = new WeakReference<SecsGem>(secsGem);
            _header = header;
            Message = msg;
        }

        /// <summary>
        /// Each PrimaryMessageWrapper can invoke Reply method once.
        /// If the message already replied, will return false.
        /// </summary>
        /// <param name="replyMessage"></param>
        /// <returns>true, if reply message sent.</returns>
        public Task<bool> ReplyAsync(SecsMessage replyMessage)
        {
            if (Interlocked.Exchange(ref _isReplied, 1) == 1)
                return ReplyAsyncFalseCache;

            if (!Message.ReplyExpected || !_secsGem.TryGetTarget(out var secsGem))
                return ReplyAsyncTrueCache;

            replyMessage = replyMessage ?? new SecsMessage(9, 7, "Unknown Message", Item.B(_header.EncodeTo(new byte[10])), replyExpected: false);
            replyMessage.ReplyExpected = false;

            return secsGem.SendDataMessageAsync(replyMessage, replyMessage.S == 9 ? secsGem.NewSystemId : _header.SystemBytes)
                .ContinueWith(ContinueWithFunc);
        }

        public override string ToString() => Message.ToString();
    }
}
