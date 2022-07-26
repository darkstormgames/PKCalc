using System;
using System.Runtime.InteropServices;

namespace PKCalc.Net
{
    public static class NetworkHelper
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool IsOnline => InternetGetConnectedState(out _, 0);

        
    }
}