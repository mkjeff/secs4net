using System;
using Secs4Net;

namespace Cim.Management {
    public static class ExtensionHelper {
        public static IDisposable Subscribe(this ISecsDevice device, ISecsFilter filter, Action<SecsMessage> callback) => device.Subscribe(filter, false, callback);

        public static IDisposable Subscribe(this ISecsDevice device, ISecsFilter filter, bool recoverable, Action<SecsMessage> callback) {
            var subscription = new SecsEventSubscription(device.ToolId, filter, recoverable, callback);
            device.Subscribe(subscription);
            return subscription;
        }

        public static IDisposable Subscribe(this ISecsDevice device, byte s, byte f, string name, Action<SecsMessage> callback) => device.Subscribe(new SecsFilter { S = s, F = f, Name = name }, callback);

        public static IDisposable SubscribeS6F11(this ISecsDevice device, string ceid, string name, Action<SecsMessage> callback) => device.Subscribe(new S6F11Filter { CEID = ceid, Name = name }, callback);
    }

    public interface IServerDisposable : IDisposable {
        void RecoverComplete();
    }
}
