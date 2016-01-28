using Cim.Eap.Tx;
namespace Cim.Eap {
    partial class Driver {
        void HandleTCS(ControlJobsInfoInqueryAck tx) {
            foreach (var controlJob in tx.ControlJobs)
                if (controlJob.StartedFlag && !controlJob.EndFlag)
                    foreach (var processJob in controlJob.ProcessJobs)
                        _ProcessingJobs.Add(processJob);
        }
    }
}