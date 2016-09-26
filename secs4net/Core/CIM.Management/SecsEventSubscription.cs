using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using Secs4Net;

namespace Cim.Management {
    public sealed class SecsEventSubscription : MarshalByRefObject, IDisposable {
        public readonly string Id;
        public readonly string ClientAddress = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
        public readonly string ToolId;
        public readonly SecsMessage Filter;
        public readonly bool Recoverable;
        readonly Action<SecsMessage> _handler;

#if DEBUG
        public override object InitializeLifetimeService() {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial) {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(20);
                lease.RenewOnCallTime = TimeSpan.FromMinutes(20);
                lease.SponsorshipTimeout = TimeSpan.FromMinutes(1);
            }
            return lease;
        }

        ~SecsEventSubscription() {
            Trace.WriteLine("SecsEventSubcription destory!");
        }
#endif

        [OneWay]
        public void Handle(SecsMessage msg) {
            _handler(msg);
        }

        internal SecsEventSubscription(string toolId, SecsMessage filter, bool recoverable, Action<SecsMessage> callback) {
            this.Id = (toolId + filter.Name + callback.Method.DeclaringType + callback.Method.Name).Replace('\\', '-').Replace('+', '-').Replace(';', '-').Replace(',', '_').Replace('"', '|');
            this.ToolId = toolId;
            this.Recoverable = recoverable;
            this._handler = callback;
            this.Filter = filter;
        }

        #region IDisposable Members
        public bool IsDisposed => _disposable?.IsDisposed ?? false;
        internal SerializableDisposable _disposable;

        public void Dispose()
        {
            _disposable?.Dispose();
        }
        #endregion
    }
}
