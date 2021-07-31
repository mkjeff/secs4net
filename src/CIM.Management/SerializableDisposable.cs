using System;
using System.Messaging;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Services;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Secs4Net;

namespace Cim.Management {
    [Serializable]
    public sealed class SerializableDisposable : IDisposable, ISerializable {
        static readonly ISponsor Sponsor = new Sponsor();

        static SerializableDisposable() {
            TrackingServices.RegisterTrackingHandler(SubscriptionTrackingHandler.Instance);
        }

        readonly string _queuePath;
        SecsEventSubscription _subscription;
        readonly IServerDisposable _serverDisposable;

        const int QUEUE_RECEIVE_TIMEOUT = 3;//second

        public SerializableDisposable(SecsEventSubscription subscription, IServerDisposable serverDisposable) {
            _serverDisposable = serverDisposable;
            _subscription = subscription;
            _queuePath = @".\private$\" + subscription.Id;
        }
        #region IDisposable Members

        volatile bool _isDisposed;
        internal bool IsDisposed => _isDisposed;

        public void Dispose() {
            if (!this.IsDisposed) {
                if (_subscription.Recoverable && !MessageQueue.Exists(_queuePath))
                    MessageQueue.Create(this._queuePath);
                try {
                    _serverDisposable.Dispose();
                    var serverLease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_serverDisposable);
                    if (serverLease != null)
                        serverLease.Unregister(Sponsor);
                } catch { } finally {
                    _isDisposed = true;
                    _subscription = null;
                }
            }
        }

        #endregion
        #region ISerializable Members
        void Recover(object sender, ReceiveCompletedEventArgs e) {
            var queue = sender as MessageQueue;
            try {
                Message msg = queue.EndReceive(e.AsyncResult);
                SecsMessage secsMsg = msg.Body as SecsMessage;
                if (secsMsg != null)
                    _subscription.Handle(secsMsg);
                queue.BeginReceive(TimeSpan.FromSeconds(QUEUE_RECEIVE_TIMEOUT));
            } catch (MessageQueueException) {
                _serverDisposable.RecoverComplete();
                queue.Close();
                MessageQueue.Delete(_queuePath);
            }
        }

        SerializableDisposable(SerializationInfo info, StreamingContext context) {
            this._queuePath = info.GetString("queuePath");
            this._serverDisposable = (IServerDisposable)info.GetValue("server", typeof(IServerDisposable));
            this._subscription = (SecsEventSubscription)info.GetValue("subscription", typeof(SecsEventSubscription));
            this._subscription._disposable = this;

            var serverLease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_serverDisposable);
            if (serverLease != null)
                serverLease.Register(Sponsor);

            if (_subscription.Recoverable && MessageQueue.Exists(_queuePath)) {
                var queue = new MessageQueue(_queuePath, true, false) {
                    Formatter = new BinaryMessageFormatter()
                };
                queue.ReceiveCompleted += Recover;
                queue.BeginReceive(TimeSpan.FromSeconds(QUEUE_RECEIVE_TIMEOUT));
            } else {
                if (MessageQueue.Exists(_queuePath))
                    MessageQueue.Delete(_queuePath);
                _serverDisposable.RecoverComplete();
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("queuePath", this._queuePath);
            info.AddValue("server", this._serverDisposable);
            info.AddValue("subscription", this._subscription);
        }

        #endregion
    }
}
