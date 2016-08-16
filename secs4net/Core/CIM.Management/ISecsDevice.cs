using System;
using System.Threading.Tasks;
using Secs4Net;
namespace Cim.Management {
    public interface ISecsDevice {
        string ToolId { get; }
        Task<SecsMessage> SendAsync(SecsMessage primaryMsg);
        IDisposable Subscribe(SecsEventSubscription subscription);
    }
}