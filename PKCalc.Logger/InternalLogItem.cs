using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Logger
{
    public class InternalLogItem
    {
        public NLog.LogLevel Level { get; private set; }
        public string LogSource { get; private set; }
        public string Message { get; private set; }
        public string? StackTrace { get; private set; }
        public DateTime Time { get; private set; }
        public string TimeString => Time.ToString("yyyy.MM.dd HH:mm:ss");
        
        public InternalLogItem(NLog.LogLevel level, string logSource, string message, string? stackTrace = null)
        {
            this.Level = level;
            this.LogSource = logSource;
            this.Message = message;
            this.StackTrace = stackTrace;
            this.Time = DateTime.UtcNow;
        }
    }
}
