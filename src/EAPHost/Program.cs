using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Cim.Eap.Properties;
using Cim.Management;
using Cim.Services;
using log4net;
using log4net.Core;
using log4net.Layout;
using log4net.ObjectRenderer;
using log4net.Repository.Hierarchy;
using Secs4Net;
using Secs4Net.Sml;

namespace Cim.Eap
{
    static class Program {
        sealed class SecsMessageRenderer : IObjectRenderer {
            public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer) {
                var msgInfo = obj as SecsMessageLogInfo;
                if (msgInfo != null) {
                    writer.WriteLine($"EAP {(msgInfo.In ? "<<" : ">>")} EQP : [id=0x{msgInfo.SystemByte:X8}]");
                    msgInfo.Message.WriteTo(writer);
                }
            }
        }

        [STAThread]//Postman COM interop need STA appartment
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        static void Main()
        {
            Application.ThreadException += (sender, e) =>
            {
                EapLogger.Error(e.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                EapLogger.Error(e.ExceptionObject as Exception);
                if (e.IsTerminating)
                    MessageBox.Show(e.ExceptionObject.ToString(), "程式發生嚴重錯誤而終止");
            };

            var fileAppender = new RollingLogFileAppender
            {
                Layout = new PatternLayout("%date{HH:mm:ss,fff} %-6level [%4thread] %message%newline")
            };
            fileAppender.ActivateOptions();
            var l = (Logger)LogManager.GetLogger("EAP").Logger;
            l.Level = Level.All;
            l.Repository.RendererMap.Put(typeof(SecsMessageLogInfo), new SecsMessageRenderer());
            l.Repository.Threshold = Level.All;
            l.Repository.Configured = true;
            l.AddAppender(fileAppender);

            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            ChannelServices.RegisterChannel(
                new TcpChannel(
                    new Hashtable { ["port"] = 0, ["bindTo"] = Settings.Default.TcpBindTo },
                    null,
                    new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full }),
                false);

            bool newMutexCreated;
            using (new Mutex(true, "EAP" + EAPConfig.Instance.ToolId, out newMutexCreated))
            {
                if (!newMutexCreated)
                {
                    MessageBox.Show($"系統中已經執行了EAP:{EAPConfig.Instance.ToolId}啟動錯誤");
                    return;
                }
                EapLogger.Info("__________________ EAP Started __________________");

                var form = new HostMainForm();
                RemotingServices.Marshal(form, EAPConfig.Instance.ToolId, typeof(ISecsDevice));
                PublishZService();

                Application.Run(form);
                EapLogger.Info("___________________ EAP Stop ____________________");
            }
        }

        internal static void PublishZService() {
            var channels = ChannelServices.RegisteredChannels.OfType<IChannelReceiver>();
            var url = channels.First().GetUrlsForUri(EAPConfig.Instance.ToolId)[0];
            var serviceManager = (IServiceManager<ISecsDevice>)RemotingServices.Connect(typeof(IServiceManager<ISecsDevice>), Settings.Default.ZUrl);
            serviceManager.Publish(EAPConfig.Instance.ToolId, url);
        }

        private static readonly XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { Indent = true };

        public static string ToFormatedXml(this XDocument xmlDoc) {
            using (var swr = new StringWriter()) {
                using (var xwr = XmlWriter.Create(swr, xmlWriterSettings))
                    xmlDoc.WriteTo(xwr);
                return swr.ToString();
            }
        }

        public static Action<T> AddHandler<T, TKey>(this ConcurrentDictionary<TKey, Action<T>> handlers, TKey key, Action<T> handler) 
            => handlers.AddOrUpdate(key, handler, (_, old) => old += handler);

        public static Action<T> RemoveHandler<T, TKey>(this ConcurrentDictionary<TKey, Action<T>> handlers, TKey key, Action<T> handler) 
            => handlers.AddOrUpdate(key, (Action<T>)null, (_, old) => old -= handler);

        public static int GetKey(this SecsEventSubscription subscription) 
            => subscription.Filter.S << 8 | subscription.Filter.F;

        public static int GetKey(this SecsMessage msg) => msg.S << 8 | msg.F;
    }

    sealed class SecsMessageLogInfo {
        public bool In;
        public int SystemByte;
        public SecsMessage Message;
    }
}
