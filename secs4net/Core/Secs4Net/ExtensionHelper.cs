using Microsoft.Toolkit.HighPerformance;
using PooledAwait;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net
{
    internal static class SecsExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Reverse(this Span<byte> bytes, int offSet)
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
        internal static void HandleReplyMessage(this ValueTaskCompletionSource<SecsMessage> source, SecsMessage primaryMessage, SecsMessage secondaryMessage)
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

        internal static StringBuilder AppendArray<T>(this StringBuilder sb, ValueArray<T> arrary, int maxCount) where T : unmanaged
        {
            if (arrary.IsEmpty)
            {
                return sb;
            }

            var len = Math.Min(arrary.Length, maxCount);
            for (int i = 0; i < len - 1; i++)
            {
                sb.Append(arrary[i].ToString()).Append(' ');
            }

            sb.Append(arrary[len - 1].ToString());
            if (len < arrary.Length)
            {
                sb.Append(" ...");
            }

            return sb;
        }

        internal static StringBuilder AppendBinary(this StringBuilder sb, ReadOnlySpan<byte> array, int maxCount)
        {
            if (array.IsEmpty)
            {
                return sb;
            }

            var len = Math.Min(array.Length, maxCount);
            for (int i = 0; i < len - 1; i++)
            {
                AppendHexChars(sb, array.DangerousGetReferenceAt(i));
                sb.Append(' ');
            }

            AppendHexChars(sb, array.DangerousGetReferenceAt(len - 1));
            if (len < array.Length)
            {
                sb.Append(" ...");
            }

            return sb;

            static void AppendHexChars(StringBuilder sb, byte num)
            {
                var hex1 = Math.DivRem(num, 0x10, out var hex0);
                sb.Append(GetHexChar(hex1)).Append(GetHexChar(hex0));
            }

            static char GetHexChar(int i) => (i < 10) ? (char)(i + 0x30) : (char)(i - 10 + 0x41);
        }

    }

}
