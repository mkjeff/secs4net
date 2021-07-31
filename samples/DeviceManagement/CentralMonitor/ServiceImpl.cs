using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Windows.Threading;
using Cim.Services;
using Cim.Management;

namespace CentralMonitor {
    sealed class ZCentralService : MarshalByRefObject, IServiceManager<ISecsDevice>, ICentralService<ISecsDevice> {
        public ObservableCollection<Tool> Tools { get; }
        readonly Dispatcher _UIDispatcher;
        public ZCentralService(Dispatcher dispatcher,IEnumerable<Tool> tools){
            Tools = new ObservableCollection<Tool>(tools);
            _UIDispatcher = dispatcher;
        }
        public override object InitializeLifetimeService() => null;

        [OneWay]
        void IServiceManager<ISecsDevice>.Publish(string serviceId,string serviceUrl) {
            _UIDispatcher.Invoke((Action)delegate {
                var tool = Tools.FirstOrDefault(t => t.Id == serviceId);
                if (tool == null)
                    Tools.Add(new Tool(serviceId) {
                        Url = serviceUrl,
                        RegisiterTime = DateTime.Now
                    });
                else
                    tool.Url = serviceUrl;
            });
        }

        ISecsDevice ICentralService<ISecsDevice>.GetService(string serviceId) {
            Tool tool = Tools.FirstOrDefault(t => t.Id == serviceId);

            if (tool == null) 
                throw new Exception("Service not exist!");

            var device = (ISecsDevice)RemotingServices.Connect(typeof(ISecsDevice), tool.Url);
            if (device.ToolId != serviceId) {
                throw new Exception("Url and Tool's id is unmatch. please check EAP.");
            }
            return device;
        }
    }
}