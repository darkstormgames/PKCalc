using Microsoft.Win32;

namespace PKCalc.Configuration
{
    public static class RegistryHelper
    {
        private static RegistryKey getRootKey()
        {
            if (OperatingSystem.IsWindows())
            {
                RegistryKey orgKey = Registry.CurrentUser.OpenSubKey(@"Software\darkstormgames");
                if (orgKey == null)
                {
                    orgKey = Registry.CurrentUser.CreateSubKey(@"Software\darkstormgames");
                }
                RegistryKey rootKey = orgKey.OpenSubKey(@"PKCalc");
                if (rootKey == null)
                {
                    rootKey = orgKey.CreateSubKey(@"PKCalc");
                }
                return rootKey;
            }
            else
                return null;
        }
        
#pragma warning disable CA1416 // Plattformkompatibilität überprüfen
    // useless warning, because non-windows-OS's always return the default value

        #region LogLevel
        public static NLog.LogLevel GetLogLevel()
        {
            RegistryKey root = getRootKey();
            if (root == null) return NLog.LogLevel.Info;
            if (int.TryParse(root.GetValue("LogLevel")?.ToString(), out int logOrdinal))
            {
                return logOrdinal switch
                {
                    0 => NLog.LogLevel.Trace,
                    1 => NLog.LogLevel.Debug,
                    2 => NLog.LogLevel.Info,
                    3 => NLog.LogLevel.Warn,
                    4 => NLog.LogLevel.Error,
                    5 => NLog.LogLevel.Fatal,
                    6 => NLog.LogLevel.Off,
                    _ => NLog.LogLevel.Info,
                };
            }
            else
            {
                root.SetValue("LogLevel", 2);
                root.Close();
                return NLog.LogLevel.Info;
            }
        }
        
        public static void SetLogLevel(NLog.LogLevel logLevel)
        {
            RegistryKey root = getRootKey();
            if (root == null) return;
            root.SetValue("LogLevel", logLevel.Ordinal);
            root.Close();
        }
        #endregion

        #region Metrics
        public static bool GetMetrics()
        {
            RegistryKey root = getRootKey();
            if (root == null) return false;
            if (bool.TryParse(root.GetValue("Metrics")?.ToString(), out bool metrics))
            {
                return metrics;
            }
            else
            {
                root.SetValue("Metrics", false);
                root.Close();
                return false;
            }
        }

        public static void SetMetrics(bool metrics)
        {
            RegistryKey root = getRootKey();
            if (root == null) return;
            root.SetValue("Metrics", metrics);
            root.Close();
        }
        #endregion


#pragma warning restore CA1416 // Plattformkompatibilität überprüfen
    }
}