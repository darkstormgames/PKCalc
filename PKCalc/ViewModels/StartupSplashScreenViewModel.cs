﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.ViewModels
{
    internal class StartupSplashScreenViewModel
    {



        public bool LoadData()
        {
            App.Logger.Trace("Loading data.");

            // Load data from a database, web service, or other source
            App.Logger.Trace("Finished loading data.");
            return true;
        }
    }
}
