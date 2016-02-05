using System;
using Cim.Management;
using Cim.Eap.Tx;
using System.Threading.Tasks;

namespace Cim.Eap
{
    public interface IEAP : ISecsDevice
    {
        SecsMessageList SecsMessages { get; }
        DefineLinkConfig EventReportLink { get; }
    }
}