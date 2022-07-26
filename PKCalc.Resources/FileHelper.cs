using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace PKCalc.Resources
{
    public static class FileHelper
    {
        public static bool CheckBaseFiles()
        {
            string baseFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\darkstormgames\PKCalc\images\pkm_base\";
            if (!File.Exists(baseFolderPath + "index.json"))
            {
                
            }

            return true;
        }
    }
}