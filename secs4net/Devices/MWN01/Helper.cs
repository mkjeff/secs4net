using System.Collections.Generic;
using Cim.Eap.Data;
using System;
namespace Eap.Driver.MWN {
    partial class Driver {
        readonly IDictionary<string, ProcessJob> _ProcessingJobs = new Dictionary<string, ProcessJob>(StringComparer.Ordinal);

        public static string GetPortID(int portNo) => "L0" + portNo;

        public static byte GetPortNo(string portId) => byte.Parse(portId.Substring(2));

        public ProcessJob GetProcessJob(string key) {
            ProcessJob pj;
            if (!_ProcessingJobs.TryGetValue(key, out pj))
                pj = ProcessJob.DummyProcessJob;
            return pj;
        }
    }

}