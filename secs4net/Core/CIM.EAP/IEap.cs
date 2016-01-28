using System;
using Cim.Management;
using Cim.Eap.Tx;

namespace Cim.Eap
{
    public interface IEAP : ISecsDevice
    {
        SecsMessageList SecsMessages { get; }
        DefineLinkConfig EventReportLink { get; }

        /// <summary>
        /// Setup TX message handler
        /// </summary>
        /// <typeparam name="T">Name rule: [TxName][TxType]</typeparam>
        /// <param name="handler">Tx handler method</param>
        void SetTxHandler<T>(Action<T> handler) where T : struct, ITxMessage;

        void Report<T>(T report) where T : struct, ITxReport;
        void Report(DataCollectionReport report);
        void Report(DataCollectionCompleteReport report);
    }
}