using System;
using Secs4Net;
using System.Threading.Tasks;

namespace Cim.Management {
    public interface ISecsDevice {
        string ToolId { get; }
        Task<SecsMessage> SendAsync(SecsMessage primaryMsg);
        IDisposable Subscribe(SecsEventSubscription subscription);
    }
}