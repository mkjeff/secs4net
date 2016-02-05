using Cim.Eap.Tx;
using Secs4Net;

namespace Cim.Eap {
    sealed partial class Driver:EapDriver {
        protected override void Init() {
            // SecsMessage handler
            EAP.SubscribeS6F11("LoadComplete", EQP_LoadComplete);
            EAP.SubscribeS6F11("ReadyToLoad", EQP_ReadyToLoad);
            EAP.SubscribeS6F11("ReadyToUnload", EQP_ReadyToUnload);
            EAP.SubscribeS6F11("UnloadComplete", EQP_UnloadComplete);
            EAP.SubscribeS6F11("CarrierIDRead", EQP_CarrierIdRead);
            EAP.SubscribeS6F11("CarrierIDReadFail", EQP_CarrierIdReadFail);
            EAP.SubscribeS6F11("SlotMapReport", EQP_SlotMapReport);
            EAP.SubscribeS6F11("ControlJobStart", EQP_ControlJobStart);
            EAP.SubscribeS6F11("ControlJobEnd", EQP_ControlJobEnd);
            EAP.SubscribeS6F11("ProcessJobStart", EQP_ProcessJobStart);
            EAP.SubscribeS6F11("ProcessJobEnd", EQP_ProcessJobEnd);
            EAP.SubscribeS6F11("WaferProcessData", EQP_DataCollection);
            EAP.SubscribeS6F11("WaferStatusChange", EQP_WaferStatusChange);
            EAP.SubscribeS6F11("DataCollectionComplete",EQP_DataCollectionComplete);          
        }

        protected override void HandleToolAlarm(SecsMessage msg)
        {
        }
    }
}
