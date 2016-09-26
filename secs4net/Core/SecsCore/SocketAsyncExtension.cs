using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Secs4Net
{
    static class SocketAsyncExtension
    {
        private static readonly Task<bool> _ConnectAsyncResultCache = Task.FromResult(true);

        public static Task<bool> ConnectAsync(this Socket socket, IPAddress target, int port)
        {
            var tcs = new TaskCompletionSource<bool>();
            var ce = new SocketAsyncEventArgs { RemoteEndPoint = new IPEndPoint(target, port), UserToken = tcs };
            ce.Completed += (sender, e) =>
            {
                var tcs2 = e.UserToken as TaskCompletionSource<bool>;
                if (e.SocketError == SocketError.Success)
                {
                    tcs2.SetResult(e.ConnectSocket != null);
                }
                else
                {
                    tcs2.SetException(new SocketException((int)e.SocketError));
                }
            };
            if (socket.ConnectAsync(ce))
                return tcs.Task;
            return _ConnectAsyncResultCache;
        }

        public static Task<Socket> AcceptAsync(this Socket socket)
        {
            var tcs = new TaskCompletionSource<Socket>();
            var ce = new SocketAsyncEventArgs { UserToken = tcs };
            ce.Completed += (sender, e) =>
            {
                var tcs2 = e.UserToken as TaskCompletionSource<Socket>;
                if (e.AcceptSocket != null && e.SocketError == SocketError.Success)
                {
                    tcs2.SetResult(e.AcceptSocket);
                }
                else
                {
                    tcs2.SetException(new SocketException((int)e.SocketError));
                }
            };
            if (socket.AcceptAsync(ce))
                return tcs.Task;
            return Task.FromResult(ce.AcceptSocket);
        }
    }
}
