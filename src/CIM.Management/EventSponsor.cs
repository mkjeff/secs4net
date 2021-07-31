using System;
using System.Runtime.Remoting.Lifetime;

namespace Cim.Management {
    public sealed class Sponsor : MarshalByRefObject, ISponsor {
        public TimeSpan Renewal(ILease lease) {
#if DEBUG
            return TimeSpan.FromMinutes(10);
#else
            return TimeSpan.FromHours(1);
#endif
        }

        public override object InitializeLifetimeService() => null;
    }
}
