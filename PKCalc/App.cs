using System;
using System.Collections.Generic;
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
    internal partial class App : Application
    {
        public static Logger.AppLog Logger { get; private set; }
        public static PokemonService Service { get; private set; }


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
#if DEBUG
            Logger = new(
                minLogLevel: NLog.LogLevel.Trace,
                useMetrics: true,
                debugLogging: true);
#else
            
#endif

            StartupSplashScreen splash = new();
            Service = PokemonService.Instance;
            splash.Show();
            TaskDialogHelper.ShowDebugMessage("SplashScreen showing.", 
                    "Here is a super fancy splash screen behind (or in front of) this dialog.");
            if (splash.DataContext != null)
                ((ViewModels.StartupSplashScreenViewModel)splash.DataContext).LoadData();
            else
            {
                TaskDialogHelper.ShowCriticalError(
                    new ApplicationException(
                        "Couldn't load data because of a critical initialization error!",
                        new Exception("DataContext is null")));
                Environment.Exit(10);
            }

            MainWindow main = new();
            Application.Current.MainWindow = main;
            splash.Close();
            main.Show();
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            NLog.LogManager.Shutdown();
        }

        private void Application_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.

                //Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
    }
}
