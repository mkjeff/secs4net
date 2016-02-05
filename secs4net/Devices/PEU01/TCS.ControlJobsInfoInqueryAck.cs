using Cim.Eap.Tx;
using System.Threading.Tasks;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(ControlJobsInfoInqueryAck tx) {
            foreach (var controlJob in tx.ControlJobs)
                if (controlJob.StartedFlag && !controlJob.EndFlag)
                    foreach (var processJob in controlJob.ProcessJobs)
                        _ProcessingJobs.Add(processJob);
        }
    }
}