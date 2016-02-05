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
            var ce = new SocketAsyncEventArgs {RemoteEndPoint = new IPEndPoint(target, port),};
            ce.Completed += (sender, e) =>
            {
                if (e.SocketError == SocketError.Success)
                {
                    tcs.SetResult(e.ConnectSocket != null);
                }
                else
                {
                    tcs.SetException(new SocketException((int) e.SocketError));
                }
            };
            if (socket.ConnectAsync(ce))
                return tcs.Task;
            return _ConnectAsyncResultCache;
        }

        public static Task<Socket> AcceptAsync(this Socket socket)
        {
            var tcs = new TaskCompletionSource<Socket>();
            var ce = new SocketAsyncEventArgs();
            ce.Completed += (sender, e) =>
            {
                if (e.AcceptSocket != null && e.SocketError == SocketError.Success)
                {
                    tcs.SetResult(e.AcceptSocket);
                }
                else
                {
                    tcs.SetException(new SocketException((int)e.SocketError));
                }
            };
            if (socket.AcceptAsync(ce))
                return tcs.Task;
            return Task.FromResult(ce.AcceptSocket);
        }
    }
}
