using Cim.Eap.Tx;
using System.Threading.Tasks;

namespace Eap.Driver.MWN {
    partial class Driver {
        Task TCS_ControlJobsInfoInqueryAck(ControlJobsInfoInqueryAck tx) {
            foreach (var controlJob in tx.ControlJobs)
                if (controlJob.StartedFlag && !controlJob.EndFlag)
                    foreach (var processJob in controlJob.ProcessJobs)
                        _ProcessingJobs[processJob.Id] = processJob;
            return Task.FromResult(0);
        }
    }
}