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
        public string Message { get; private set; }
        public Exception? Exception { get; private set; }
        public DateTime Time { get; private set; }
        public string TimeString => Time.ToString("yyyy.MM.dd_HH:mm:ss");

        public InternalLogItem(NLog.LogLevel level, string message, Exception? ex = null)
        {
            this.Level = level;
            this.Message = message;
            this.Exception = ex;
            this.Time = DateTime.UtcNow;
        }
    }
}
