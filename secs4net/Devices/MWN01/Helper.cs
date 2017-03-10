using System.Collections.Generic;
using Cim.Eap.Data;
using System;
namespace Cim.Eap {
    partial class Driver {
        readonly IDictionary<string, ProcessJob> _ProcessingJobs = new Dictionary<string, ProcessJob>(StringComparer.Ordinal);

        public static string GetPortID(int portNo) => "L0" + portNo;

        public static byte GetPortNo(string portId) => byte.Parse(portId.Substring(2));

        public ProcessJob GetProcessJob(string key) {
            if (!_ProcessingJobs.TryGetValue(key, out var pj))
                pj = ProcessJob.DummyProcessJob;
            return pj;
        }
    }

}