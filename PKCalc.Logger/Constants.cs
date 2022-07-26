using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Logger
{
    internal static class Constants
    {
        public static string ConfigFolderPath
            => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + @"\darkstormgames\PKCalc\config\";
        public static string LogFolderPath
            => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + @"\darkstormgames\PKCalc\logs\";
        public static string MetricsFolderPath
            => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + @"\darkstormgames\PKCalc\metrics\";
    }
}
