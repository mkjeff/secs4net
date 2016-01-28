using System;
using System.Linq;
using Secs4Net;
using Cim.Management;
namespace Cim.Eap {
    public static class Helper {
        public static IDisposable SubscribeS6F11(this IEAP eap, string eventName, Action<SecsMessage> callback) {
            try {
                return eap.SubscribeS6F11(
                    eap.EventReportLink.Events.First(ceid => ceid.Name == eventName).Id,
                    eventName,
                    callback);
            } catch (Exception ex) {
                EapLogger.Warn("EAP.Subscribe_S6F11 error, event({0});{1}", eventName, ex.Message);
                return null;
            }
        }

        public static TEnum ToEnum<TEnum>(this string str) where TEnum : struct => ToEnum<TEnum>(str, false);

        public static TEnum ToEnum<TEnum>(this string str, bool ignoreCase) where TEnum : struct => (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
    }
}