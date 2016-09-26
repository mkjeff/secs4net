using System;
using Cim.Management;
using Cim.Eap.Tx;
using System.Threading.Tasks;

namespace Cim.Eap
{
    public interface IEAP : ISecsDevice
    {
        EapDriver Driver { get; }
        SecsMessageList SecsMessages { get; }
        DefineLinkConfig EventReportLink { get; }

        /// <summary>
        /// Setup TX message handler
        /// </summary>
        /// <typeparam name="T">Name rule: [TxName][TxType]</typeparam>
        /// <param name="handler">Tx handler method</param>
        void SetTxHandler<T>(Func<T, Task> handler) where T : struct, ITxMessage;

        void Report<T>(T report) where T : struct, ITxReport;
        void Report(DataCollectionReport report);
        void Report(DataCollectionCompleteReport report);
    }
}