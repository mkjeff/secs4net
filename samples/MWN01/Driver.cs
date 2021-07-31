using Cim.Eap;
using Cim.Eap.Tx;
using Secs4Net;

namespace Eap.Driver.MWN
{
    sealed partial class Driver : EapDriver
    {
        protected override void Init()
        {
            // TX Handler
            EAP.SetTxHandler<ToolModeChangeRequest>(TCS_ToolModeChange);
            EAP.SetTxHandler<AccessModeChangeRequest>(TCS_AccessModeChange);
            EAP.SetTxHandler<AskLoadStatusRequest>(TCS_AskLoadtStatus);
            EAP.SetTxHandler<CreateProcessJobRequest>(TCS_CreateProcessJob);
            EAP.SetTxHandler<CreateControlJobRequest>(TCS_CreateControlJob);
            EAP.SetTxHandler<ProceedWithCarrierRequest>(TCS_ProceedWithCarrier);
            EAP.SetTxHandler<ProceedSlotMapRequest>(TCS_ProceedSlotMap);
            EAP.SetTxHandler<CancelCarrierRequest>(TCS_CancelCarrier);
            EAP.SetTxHandler<ControlJobsInfoInqueryAck>(TCS_ControlJobsInfoInqueryAck);

            // SecsMessage Handler
            EAP.SubscribeS6F11("LoadComplete", EQP_LoadComplete);
            EAP.SubscribeS6F11("ReadyToLoad", EQP_ReadyToLoad);
            EAP.SubscribeS6F11("ReadyToUnload", EQP_ReadyToUnload);
            EAP.SubscribeS6F11("UnloadComplete", EQP_UnloadComplete);
            EAP.SubscribeS6F11("CarrierIDRead", EQP_CarrierIDRead);
            EAP.SubscribeS6F11("CarrierIDReadFail", EQP_CarrierIDReadFail);
            EAP.SubscribeS6F11("SlotMapReport", EQP_SlotMapReport);
            EAP.SubscribeS6F11("ControlJobStart", EQP_ControlJobStart);
            EAP.SubscribeS6F11("ControlJobEnd", EQP_ControlJobEnd);
            EAP.SubscribeS6F11("ProcessJobStart", EQP_ProcessJobStart);
            EAP.SubscribeS6F11("ProcessJobEnd", EQP_ProcessJobEnd);
            EAP.SubscribeS6F11("WaferProcessData_LLH", EQP_WaferProcessData_LLH_LHC);
            EAP.SubscribeS6F11("WaferProcessData_LHC", EQP_WaferProcessData_LLH_LHC);
            EAP.SubscribeS6F11("WaferProcessData_PVD", EQP_WaferProcessData_PVD);
        }

        protected override void HandleToolAlarm(SecsMessage msg)
        {
        }
    }
}
