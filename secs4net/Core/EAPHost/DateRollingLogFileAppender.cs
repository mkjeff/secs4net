using System;
using System.Linq;
using System.IO;
using Cim.Eap;
using log4net.Appender;
using log4net.Core;
using Cim.Eap.Properties;

namespace Cim.Eap {
	sealed class RollingLogFileAppender : FileAppender {
        static readonly string LOG_DIR = Environment.CurrentDirectory + @"\..\Log\";
        static readonly string FileFormat = LOG_DIR + EAPConfig.Instance.ToolId + "_{0:yyyyMMdd}_{1}.log";

        int _currentRollCount;

		public RollingLogFileAppender() {
            if (!Directory.Exists(LOG_DIR))
                Directory.CreateDirectory(LOG_DIR);

            //get last rolled file
            int lastCount = GetLastRollCount();
            _currentRollCount = lastCount == 0 ? 1 : lastCount;
            ClearExpiredFile();
            ImmediateFlush = false;
            AppendToFile = true;
            File = CurrnetFileName;
        }

        static int GetLastRollCount() {
            Func<string, int> parser = str => {
                int result;
                return int.TryParse(str, out result) ? result : 0;
            };
            var c = from fi in new DirectoryInfo(LOG_DIR).GetFiles(string.Format("*_{0:yyyyMMdd}_*.log", DateTime.Now))
                    let _i = fi.FullName.LastIndexOf('_')
                    let dot = fi.FullName.LastIndexOf('.')
                    where _i != -1 && dot != -1 && dot > _i
                    let count = parser(fi.FullName.Substring(_i + 1, dot - _i - 1))
                    orderby count descending
                    select count;

            return c.FirstOrDefault();
        }

        string CurrnetFileName { get { return string.Format(FileFormat, DateTime.Now, _currentRollCount); } }

		protected override void Append(LoggingEvent loggingEvent) {
			if (!base.File.Contains(loggingEvent.TimeStamp.ToString("yyyyMMdd"))) {
                //new day
				base.CloseFile();
				ClearExpiredFile();
                _currentRollCount = 1;
				base.SafeOpenFile(CurrnetFileName, true);
            } else if (new FileInfo(base.File).Length > Settings.Default.RollSize) {
                base.CloseFile();
                _currentRollCount++;
                base.SafeOpenFile(CurrnetFileName, true);
            }

			base.Append(loggingEvent);
		}

		void ClearExpiredFile() {
			foreach (var fi in new DirectoryInfo(LOG_DIR).GetFiles("*.log")) 
				if (DateTime.Now.Date.Subtract(fi.CreationTime.Date).TotalDays >= Settings.Default.LogDay) 
					fi.Delete();
		}
	}
}