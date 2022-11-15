using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.ViewModels
{
    internal class StartupSplashScreenViewModel
    {


        public bool CheckForUpdates()
        {
            App.Logger.Debug("Checking for updates.");

            // Check for updates
            App.Logger.Trace("Finished checking for updates.");
            return true;
        }

        public bool LoadData()
        {
            App.Logger.Debug("Loading data.");

            // Load data into cache
            App.Service.ReloadCache();
            
            App.Logger.Trace("Finished loading data.");
            return true;
        }

    }
}
