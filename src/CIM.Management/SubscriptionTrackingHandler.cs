using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using System.Threading;
using Cim.Services;

namespace Cim.Management {
    sealed class SubscriptionTrackingHandler : ITrackingHandler {
        SubscriptionTrackingHandler() { }

        public static readonly SubscriptionTrackingHandler Instance = new SubscriptionTrackingHandler();

        void ITrackingHandler.DisconnectedObject(object obj) {
            var subscription = obj as SecsEventSubscription;
            if (subscription != null && !subscription.IsDisposed) {
                ThreadPool.QueueUserWorkItem(_ => {
                    while (!subscription.IsDisposed) {
                        try {
                            Trace.WriteLine(subscription.ToolId + "[" + subscription.Filter.Name + "] try to re-subscribe.....");
                            var manager = (ICentralService<ISecsDevice>)Activator.GetObject(typeof(IServiceManager<ISecsDevice>), ConfigurationManager.AppSettings["zmanagerUrl"]);
                            var device = manager.GetService(subscription.ToolId);
                            device.Subscribe(subscription);
                            break;
                        } catch {
                        }
                        Thread.Sleep(10000);
                    }
                });
            } else if (obj is IServerDisposable) {
                Trace.WriteLine("IServerDisposable disconnectioned!!");
                ((IServerDisposable)obj).Dispose();
            }
        }

        void ITrackingHandler.MarshaledObject(object obj, ObjRef or) { }
        void ITrackingHandler.UnmarshaledObject(object obj, ObjRef or) { }
    }
}
