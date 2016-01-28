using System;
using Secs4Net;
namespace Cim.Management {
    public interface ISecsDevice {
        string ToolId { get; }
        SecsMessage Send(SecsMessage primaryMsg);
        IDisposable Subscribe(SecsEventSubscription subscription);
    }
}