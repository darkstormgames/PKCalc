using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using PKCalc.Client;
using PKCalc.UI;
using PKCalc.Windows;

namespace PKCalc
{
    public partial class App : Application
    {
        private static Logger.Log logger;
        public static Logger.Log Logger 
        { 
            get
            {
                if (logger == null)
                {
                    logger = new(minLogLevel: Configuration.RegistryHelper.GetLogLevel(),
                                 internalLogItems: InternalLog);
                }
                return logger;
            }
        }
        public static ObservableCollection<Logger.InternalLogItem> InternalLog { get; private set; } = new();
        public static PokemonService Service { get; private set; }
        public static bool IsDebug => Configuration.RegistryHelper.GetLogLevel().Ordinal <= 1;
        public static bool IsExiting { get; private set; }

        public App()
        {
            this.Startup += this.application_Startup;
            this.Exit += this.application_Exit;
            this.DispatcherUnhandledException += application_UnhandledException; ;
            InitializeComponent();
        }

        private void application_Startup(object sender, StartupEventArgs e)
        {
            Logger.Trace("Starting Application.");
            StartupSplashScreen splash = new();
            Service = PokemonService.Instance;
            splash.Show();
            if (splash.DataContext != null && splash.DataContext is ViewModels.StartupSplashScreenViewModel splashVM)
            {
                // Check for updates
                splashVM.CheckForUpdates();

                // Download missing/updated data
                
                
                // Load application data to cache
                splashVM.LoadData();
            }
            else
            {
                Logger.Fatal("StartupSplashScreenViewModel is null.");
                TaskDialogHelper.ShowCriticalError(
                    new ApplicationException(
                        "Couldn't load data because of a critical initialization error!",
                        new Exception("StartupSplashScreenViewModel is null")));
                Environment.Exit(10);
            }

            Logger.Trace("Starting MainWindow.");
            MainWindow main = new();
            Current.MainWindow = main;
            splash.Close();
            main.Show();
            Logger.Info("Application started.");
        }

        private void application_Exit(object sender, EventArgs e)
        {
            Logger.Info("Exiting Application.");
            // Finish Network Tasks before telling the whole Application, that we're about to exit
            IsExiting = true;
            NLog.LogManager.Shutdown();
        }

        private void application_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "Unhandled Exception.");
            TaskDialogHelper.ShowError(e.Exception);
            
            e.Handled = true;
        }
    }
}
