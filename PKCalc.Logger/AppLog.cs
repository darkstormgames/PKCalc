using NLog;

namespace PKCalc.Logger
{
    public class AppLog : IDisposable
    {
        private readonly NLog.Config.LoggingConfiguration config;
        private readonly NLog.Logger nLogger;
        private readonly string fileLogPath;
        
        private NLog.Targets.FileTarget logFile;
        List<string> strings = new();
        
        public AppLog(
            LogLevel minLogLevel,
            bool useMetrics = false,
            bool debugLogging = false)
        {
            this.config = new();
            this.fileLogPath = Constants.LogFolderPath + "app_" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log";

            this.logFile = new NLog.Targets.FileTarget("logfile") { FileName = fileLogPath };

            this.config.AddRule(minLogLevel, NLog.LogLevel.Fatal, this.logFile);
            
            LogManager.Configuration = config;
            this.nLogger = LogManager.GetCurrentClassLogger();
            this.nLogger.Debug("Logger loaded.");
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            this.nLogger.Log(logLevel, message, args);
        }
        public void Log(LogLevel logLevel, Exception ex, string message, params object[] args)
        {
            this.nLogger.Log(logLevel, ex, message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            this.nLogger.Fatal(message, args);
        }
        public void Fatal(Exception ex, string message, params object[] args)
        {
            this.nLogger.Fatal(ex, message, args);
        }
        
        public void Error(string message, params object[] args)
        {
            this.nLogger.Error(message, args);
        }
        public void Error(Exception ex, string message, params object[] args)
        {
            this.nLogger.Error(ex, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            this.nLogger.Warn(message, args);
        }
        public void Warn(Exception ex, string message, params object[] args)
        {
            this.nLogger.Warn(ex, message, args);
        }

        public void Info(string message, params object[] args)
        {
            this.nLogger.Info(message, args);
        }
        public void Info(Exception ex, string message, params object[] args)
        {
            this.nLogger.Info(ex, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            this.nLogger.Debug(message, args);
        }
        public void Debug(Exception ex, string message, params object[] args)
        {
            this.nLogger.Debug(ex, message, args);
        }

        public void Trace(string message, params object[] args)
        {
            this.nLogger.Trace(message, args);
        }
        public void Trace(Exception ex, string message, params object[] args)
        {
            this.nLogger.Trace(ex, message, args);
        }

        public void Dispose()
        {
            
            GC.SuppressFinalize(this);
        }
    }
}