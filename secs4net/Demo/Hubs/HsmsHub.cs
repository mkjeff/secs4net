using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Secs4Net;
using Secs4Net.Sml;

namespace HsmsWebHost.Hubs
{
    public class HsmsHub: Hub
	{
		private static readonly ConcurrentDictionary<string, (SecsGem gem, ConcurrentDictionary<int, PrimaryMessageWrapper> penddingMessages)> _devices = new ConcurrentDictionary<string, (SecsGem, ConcurrentDictionary<int, PrimaryMessageWrapper>)>();

        public override Task OnConnectedAsync()
		{
			if (_devices.TryRemove(Context.ConnectionId, out var device))
			{
				device.penddingMessages.Clear();
				device.gem.Dispose();
			}

			var query = ParseQueryString(Context.GetHttpContext());
			if (query == default)
			{
				return Task.CompletedTask;
			}

			device.penddingMessages = new ConcurrentDictionary<int, PrimaryMessageWrapper>();
			var gem = new SecsGem(query.active, query.ip, query.port);

			var caller = Clients.Caller;
			gem.ConnectionChanged += (sender, e) => 
				caller.SendAsync(nameof(gem.ConnectionChanged), gem.State.ToString());
            gem.Logger = new DeviceLogger(caller);
			gem.PrimaryMessageReceived += (_, primaryMessage) =>
			{
				if (device.penddingMessages.TryAdd(primaryMessage.MessageId, primaryMessage))
				{
					caller.SendAsync(nameof(gem.PrimaryMessageReceived), primaryMessage.MessageId, primaryMessage.Message.ToString(), primaryMessage.Message.ToSml());
				}
			};

			gem.Start();
            device.gem = gem;

            _devices.TryAdd(Context.ConnectionId, device);

			return base.OnConnectedAsync();
		}

		(IPAddress ip, int port, bool active) ParseQueryString(HttpContext httpContext)
		{
			var query = httpContext?.Request.Query;
			if (query == null)
			{
				BadRequest(httpContext.Response, "");
				return default;
			}

			if (!query.TryGetValue("ipaddress", out var ipadderss)
				|| !IPAddress.TryParse(ipadderss, out var ip))
			{
				BadRequest(httpContext.Response, "ipaddress (IPv4 address)");
				return default;
			}

			if (!query.TryGetValue("port", out var port)
				|| !int.TryParse(port, out var portNumber))
			{
				BadRequest(httpContext.Response, "port (integer)");
				return default;
			}

			if (!query.TryGetValue("active", out var active)
				|| !bool.TryParse(active, out var isActive))
			{
				BadRequest(httpContext.Response, "active (boolean)");
				return default;
			}

			return (ip, portNumber, isActive);
		}

		void BadRequest(HttpResponse response, string paramName)
		{
			//response.StatusCode = (int)HttpStatusCode.BadRequest;
			//response.WriteAsync($"please provide query string: {paramName}");
			Context.Abort();
		}

        public async Task ReplyMessage(int messageId, string sml)
        {
            if (_devices.TryGetValue(Context.ConnectionId, out var device) &&
                device.penddingMessages.TryRemove(messageId, out var primaryMsg))
            {
				if (string.IsNullOrEmpty(sml))
					await primaryMsg.ReplyAsync(null);
				else
					await primaryMsg.ReplyAsync(sml.ToSecsMessage());
            }
        }

        public async Task<string> SendMessage(string sml)
        {
            if (_devices.TryGetValue(Context.ConnectionId, out var device))
            {
                var msg = await device.gem.SendAsync(sml.ToSecsMessage());
                return msg.ToSml();
            }
            return string.Empty;
        }

        public override Task OnDisconnectedAsync(Exception exception)
		{
			if (exception == null && _devices.TryRemove(Context.ConnectionId, out var device))
			{
				device.gem.Dispose();
				device.penddingMessages.Clear();
			}
			return base.OnDisconnectedAsync(exception);
		}

        sealed class DeviceLogger : ISecsGemLogger
        {
            private readonly IClientProxy _client;
            public DeviceLogger(IClientProxy client)
            {
                _client = client;
            }

            public void Debug(string msg) => _client.SendAsync("Debug", msg);
            public void Error(string msg, Exception ex = null) => _client.SendAsync("Error", msg + "\n" + ex);
            public void Info(string msg) => _client.SendAsync("Info", msg);
            public void MessageIn(SecsMessage msg, int systembyte) => _client.SendAsync("MessageIn", $"<< [0x{systembyte:X8}] {msg.ToSml()}");
			public void MessageOut(SecsMessage msg, int systembyte) => _client.SendAsync("MessageOut", $">> [0x{systembyte:X8}] {msg.ToSml()}\n");
            public void Warning(string msg) => _client.SendAsync("Warning", msg);
        }
    }
}
