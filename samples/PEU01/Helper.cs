using System.Collections.Generic;
using System.Linq;
using Cim.Eap.Data;
namespace Cim.Eap {
    partial class Driver {
        readonly List<ProcessJob> _ProcessingJobs = new List<ProcessJob>();

        public static string GetPortID(int portNo) {
            return "L0" + portNo;
        }

        public static byte GetPortNo(string portId) {
            return byte.Parse(portId.Substring(2));
        }

        /// <summary>
        /// 機台會報錯PJ ID,所以用CarrierId & slotNo當搜尋條件
        /// </summary>
        public ProcessJob GetProcessJob(string carrierId,byte slotNo) {
            return _ProcessingJobs.Find(pj =>
                pj.Carriers.First().Id == carrierId && pj.Carriers.First().SlotMap.Any(s => s.SlotNo == slotNo)) 
                ?? ProcessJob.DummyProcessJob;
        }
    }
}