using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Text;
using NLog;
using NLog.Targets;
using System.Reflection;

namespace PKCalc.Logger
{
    public class Log
    {
        private static ICollection<InternalLogItem>? internalLog;
        
        private readonly NLog.Logger nLogger;
        
        public Log(
            LogLevel minLogLevel,
            ICollection<InternalLogItem>? internalLogItems = null)
        {
            internalLog = internalLogItems;
            
            NLog.Config.LoggingConfiguration config = new()
            {
                DefaultCultureInfo = System.Globalization.CultureInfo.CurrentCulture,
            };
            
            FileTarget logFile = new("logfile") 
            { 
                FileName = Constants.LogFolderPath + "app_" + DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log" 
            };
            config.AddRule(minLogLevel, LogLevel.Fatal, logFile);

            if (minLogLevel.Ordinal <= 1)
            {
                MethodCallTarget logMethod = new("logmethod")
                {
                    ClassName = typeof(Log).AssemblyQualifiedName,
                    MethodName = "LogInternal"
                };
                logMethod.Parameters.Add(
                    new MethodCallParameter(
                        "${logLevel}",
                        NLog.Layouts.Layout.FromMethod(l => l.Level),
                        typeof(LogLevel)
                    )
                );
                logMethod.Parameters.Add(
                    new MethodCallParameter(
                        "${logSource}",
                        NLog.Layouts.Layout.FromMethod(l => l.LoggerName),
                        typeof(string)
                    )
                );
                logMethod.Parameters.Add(new MethodCallParameter("${message}"));
                logMethod.Parameters.Add(
                    new MethodCallParameter(
                        "${args}",
                        NLog.Layouts.Layout.FromMethod(l =>
                        {
                            if (l.Parameters == null) return string.Empty;
                            var args = l.Parameters;
                            var sb = new StringBuilder();
                            for (int i = 0; i < args.Length; i++)
                            {
                                sb.Append(args[i]);
                                if (i < args.Length - 1)
                                {
                                    sb.Append("||");
                                }
                            }
                            return sb.ToString();
                        }),
                        typeof(string)
                    )
                );
                logMethod.Parameters.Add(
                    new MethodCallParameter(
                        "${stackTrace}",
                        NLog.Layouts.Layout.FromMethod(l => l.Exception?.StackTrace),
                        typeof(string)
                    )
                );
                config.AddRule(minLogLevel, LogLevel.Fatal, logMethod);
            }
            
            LogManager.Configuration = config;
            this.nLogger = LogManager.GetLogger(Assembly.GetCallingAssembly().ManifestModule.Name);
            this.nLogger.Debug("Logger loaded from LogLevel {0}.", minLogLevel.Name);
        }

        public void Message(LogLevel logLevel, string message, params object[] args)
        {
            this.nLogger.Log(logLevel, message, args);
        }
        public void Message(LogLevel logLevel, Exception ex, string message, params object[] args)
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

        public static void LogInternal(LogLevel logLevel, string logSource, string message, string args, string? stackTrace)
        {
            if (internalLog != null)
            {
                string[] argsArray = args.Split("||");
                internalLog.Add(new InternalLogItem(logLevel, logSource, string.Format(message, argsArray), stackTrace));
            }
        }
    }
}