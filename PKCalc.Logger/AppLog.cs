using System.Collections.ObjectModel;
using NLog;

namespace PKCalc.Logger
{
    public class AppLog
    {
        private readonly NLog.Config.LoggingConfiguration config;
        private readonly NLog.Logger nLogger;
        private readonly NLog.Targets.FileTarget logFile;
        private readonly ICollection<InternalLogItem>? internalLog;

        
        public AppLog(
            LogLevel minLogLevel,
            ICollection<InternalLogItem>? internalLogItems = null)
        {
            this.config = new();
            this.internalLog = internalLogItems;

            this.logFile = new NLog.Targets.FileTarget("logfile") 
            { 
                FileName = Constants.LogFolderPath + "app_" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log" 
            };

            this.config.AddRule(minLogLevel, LogLevel.Fatal, this.logFile);
            
            LogManager.Configuration = config;
            this.nLogger = LogManager.GetCurrentClassLogger();
            this.nLogger.Debug("Logger loaded.");
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            this.nLogger.Log(logLevel, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(logLevel, message));
        }
        public void Log(LogLevel logLevel, Exception ex, string message, params object[] args)
        {
            this.nLogger.Log(logLevel, ex, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(logLevel, ex.Message, ex));
        }

        public void Fatal(string message, params object[] args)
        {
            this.nLogger.Fatal(message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Fatal, message));
        }
        public void Fatal(Exception ex, string message, params object[] args)
        {
            this.nLogger.Fatal(ex, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Fatal, ex.Message, ex));
        }
        
        public void Error(string message, params object[] args)
        {
            this.nLogger.Error(message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Error, message));
        }
        public void Error(Exception ex, string message, params object[] args)
        {
            this.nLogger.Error(ex, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Error, ex.Message, ex));
        }

        public void Warn(string message, params object[] args)
        {
            this.nLogger.Warn(message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Warn, message));
        }
        public void Warn(Exception ex, string message, params object[] args)
        {
            this.nLogger.Warn(ex, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Warn, ex.Message, ex));
        }

        public void Info(string message, params object[] args)
        {
            this.nLogger.Info(message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Info, message));
        }
        public void Info(Exception ex, string message, params object[] args)
        {
            this.nLogger.Info(ex, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Info, ex.Message, ex));
        }

        public void Debug(string message, params object[] args)
        {
            this.nLogger.Debug(message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Debug, message));
        }
        public void Debug(Exception ex, string message, params object[] args)
        {
            this.nLogger.Debug(ex, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Debug, ex.Message, ex));
        }

        public void Trace(string message, params object[] args)
        {
            this.nLogger.Trace(message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Trace, message));
        }
        public void Trace(Exception ex, string message, params object[] args)
        {
            this.nLogger.Trace(ex, message, args);
            if (this.internalLog != null)
                this.internalLog.Add(new InternalLogItem(LogLevel.Trace, ex.Message, ex));
        }
    }
}