using System;
using System.Reflection;
using log4net;
using log4net.Core;

namespace Cim.Eap {
    public static class EapLogger {
        static readonly ILog log = LogManager.GetLogger("EAP");

        public static void Info(object msg) {
            log.Info(msg);
        }

        public static void Info(string formatStr, params object[] objs) {
            log.Info(string.Format(formatStr, objs));
        }

        public static void Warn(object msg) {
            log.Warn(msg);
        }

        public static void Warn(string formatStr, params object[] objs) {
            log.Warn(string.Format(formatStr, objs));
        }

        public static void Notice(string msg) {
            log.Logger.Log(
                new LoggingEvent(MethodBase.GetCurrentMethod().DeclaringType, log.Logger.Repository, "root", Level.Notice, msg,
                                 null));
        }

        public static void Notice(string formatStr, params object[] objs) {
            Notice(string.Format(formatStr, objs));
        }

        public static void Error(string msg) {
            log.Error(msg);
        }

        public static void Error(string formatStr, params object[] objs) {
            log.Error(string.Format(formatStr, objs));
        }

        public static void Error(Exception ex) {
            log.Error(ex);
        }

        public static void Error(string msg, Exception ex) {
            log.Error(msg + "," + ex.Message, ex);
        }
    }    
}