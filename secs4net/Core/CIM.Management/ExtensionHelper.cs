using System;
using Secs4Net;

namespace Cim.Management {
    public static class ExtensionHelper
    {
        public static IDisposable Subscribe(this ISecsDevice device, SecsMessage filter, Action<SecsMessage> callback) 
            => device.Subscribe(filter, false, callback);

        public static IDisposable Subscribe(this ISecsDevice device, SecsMessage filter, bool recoverable, Action<SecsMessage> callback)
        {
            var subscription = new SecsEventSubscription(device.ToolId, filter, recoverable, callback);
            device.Subscribe(subscription);
            return subscription;
        }
    }

    public interface IServerDisposable : IDisposable {
        void RecoverComplete();
    }
}
