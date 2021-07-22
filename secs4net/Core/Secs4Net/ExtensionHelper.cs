using PooledAwait;
using System;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    internal static class SecsExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void Reverse(this Span<byte> bytes, int offSet)
        {
            if (offSet <= 1)
            {
                return;
            }

            for (var i = 0; i < bytes.Length; i += offSet)
            {
                bytes.Slice(i, offSet).Reverse();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HandleReplyMessage(this ValueTaskCompletionSource<SecsMessage> source, SecsMessage primaryMessage, SecsMessage secondaryMessage)
        {
            secondaryMessage.Name = primaryMessage.Name;
            if (secondaryMessage.F == 0)
            {
                source.TrySetException(new SecsException(secondaryMessage, Resources.SxF0));
                return;
            }

            if (secondaryMessage.S == 9)
            {
                switch (secondaryMessage.F)
                {
                    case 1:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9F1));
                        break;
                    case 3:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9F3));
                        break;
                    case 5:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9F5));
                        break;
                    case 7:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9F7));
                        break;
                    case 9:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9F9));
                        break;
                    case 11:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9F11));
                        break;
                    case 13:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9F13));
                        break;
                    default:
                        source.TrySetException(new SecsException(secondaryMessage, Resources.S9Fy));
                        break;
                }
                return;
            }

            source.TrySetResult(secondaryMessage);
        }
    }

}
