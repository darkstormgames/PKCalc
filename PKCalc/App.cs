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
        private static Logger.AppLog logger;
        public static Logger.AppLog Logger 
        { 
            get
            {
                if (logger == null)
                {
#if DEBUG
                    logger = new(minLogLevel: NLog.LogLevel.Trace, internalLogItems: InternalLog);
#else
                    logger = new(minLogLevel: Configuration.RegistryHelper.GetLogLevel());
#endif
                }
                return logger;
            }
        }
        public static ObservableCollection<Logger.InternalLogItem> InternalLog { get; private set; } = new();
        public static PokemonService Service { get; private set; }
#if DEBUG
        public static bool IsDebug => true;
#else
        public static bool IsDebug => false;
#endif


        public string[] CLArgs { get; internal set; }

        public App()
        {
            this.CLArgs = Array.Empty<string>();
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.DispatcherUnhandledException += Application_UnhandledException; ;
            InitializeComponent();
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Logger.Trace("Starting Application.");
            StartupSplashScreen splash = new();
            Service = PokemonService.Instance;
            splash.Show();
            if (splash.DataContext != null)
                ((ViewModels.StartupSplashScreenViewModel)splash.DataContext).LoadData();
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

        private void Application_Exit(object sender, EventArgs e)
        {
            Logger.Info("Exiting Application.");
            NLog.LogManager.Shutdown();
        }

        private void Application_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "Unhandled Exception.");
            TaskDialogHelper.ShowError(e.Exception);
        }
    }
}
