using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
namespace Secs4Net {

    [Serializable]
    public class SecsException : Exception {
        public SecsMessage SecsMsg { get; }

        protected SecsException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
            SecsMsg = info.GetValue("msg", typeof(SecsMessage)) as SecsMessage;
        }

        public SecsException(SecsMessage msg, string description)
            : base(description) {
            SecsMsg = msg;
        }

        public SecsException(string msg)
            : this(null, msg) { }

        [SecurityPermission(SecurityAction.LinkDemand,Flags= SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("msg", SecsMsg);
        }
    }
}