using System;
using System.Linq;
using Secs4Net;
using Cim.Management;
namespace Cim.Eap {
    public static class Helper {
        public static IDisposable SubscribeS6F11(this IEAP eap, string eventName, Action<SecsMessage> callback) {
            try {
                var ceid = eap.EventReportLink.Events.FirstOrDefault(id => id.Name == eventName);
                return eap.Subscribe(
                    new SecsMessage(16, 11, eventName,
                        Item.L(
                            eap.Driver.LinkDataIdCreator(string.Empty),
                            eap.Driver.CeidLinkCreator(ceid.Id),
                            Item.L())),
                    callback);
            } catch (Exception ex) {
                EapLogger.Warn($"EAP.Subscribe_S6F11 error, event({eventName});{ex.Message}");
                return null;
            }
        }

        public static TEnum ToEnum<TEnum>(this string str) where TEnum : struct => ToEnum<TEnum>(str, false);

        public static TEnum ToEnum<TEnum>(this string str, bool ignoreCase) where TEnum : struct => (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
    }
}