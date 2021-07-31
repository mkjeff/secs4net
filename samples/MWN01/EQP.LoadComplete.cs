using Cim.Eap.Tx;
using Secs4Net;
using System.Threading;
using System;
namespace Eap.Driver.MWN {
    partial class Driver {
        async void EQP_LoadComplete(SecsMessage msg) {
            byte portNo = (byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            string portId = GetPortID(portNo);
            EAP.Report(new LoadCompReport {
                PortID = portId
            });

            // Wait CarrierIDRead Event 10 sec.
            {
                // Method 1:
                //ThreadPool.QueueUserWorkItem(delegate {
                //    EapLogger.Notice(portId + " StartAsync CarrierIDRead Timer");
                //    using (var ev = new ManualResetEvent(false)) {
                //        var callback = new Action<SecsMessage>(secsMsg => {
                //            if (portNo == (byte)secsMsg.SecsItem.Items[2].Items[0].Items[1].Items[0])
                //                ev.Set();
                //        });
                //        using (EAP.SubscribeS6F11("CarrierIDRead", callback))
                //        using (EAP.SubscribeS6F11("CarrierIDReadFail", callback)) {
                //            if (!ev.WaitOne(10000)) {
                //                EapLogger.Error(portId + " CarrierIDRead Timeout!!");
                //                EAP.Report(new CarrierIDReport {
                //                    LoadPortId = portId
                //                });
                //            }
                //        }
                //    }
                //});
            }
            {
                // Method 2:
                //EapLogger.Notice(portId + " StartAsync CarrierIDRead Timer");
                //var ev = new ManualResetEvent(false);
                //var callback = new Action<SecsMessage>(secsMsg => {
                //    if (portNo == (byte)secsMsg.SecsItem.Items[2].Items[0].Items[1].Items[0])
                //        ev.Set();
                //});
                //var read = EAP.SubscribeS6F11("CarrierIDRead", callback);
                //var readfail = EAP.SubscribeS6F11("CarrierIDReadFail", callback);
                //ThreadPool.RegisterWaitForSingleObject(ev, (state, timeout) => {
                //    if (read != null)
                //        read.Dispose();
                //    if (readfail != null)
                //        readfail.Dispose();
                //    if (timeout) {
                //        EapLogger.Error(portId + " CarrierIDRead Timeout!!");
                //        EAP.Report(new CarrierIDReport {
                //            LoadPortId = portId
                //        });
                //    }
                //}, null, 10000, true);
            }
        }
    }
}