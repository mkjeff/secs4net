using Cim.Eap.Tx;
namespace Cim.Eap {
    partial class Driver {
        void TCS_ControlJobsInfoInqueryAck(ControlJobsInfoInqueryAck tx) {
            foreach (var controlJob in tx.ControlJobs)
                if (controlJob.StartedFlag && !controlJob.EndFlag)
                    foreach (var processJob in controlJob.ProcessJobs)
                        _ProcessingJobs[processJob.Id] = processJob;
        }
    }
}