using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace PKCalc.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ICommand OnGithubButton { get; }
        public ICommand OnDebugButton { get; }
        public ICommand OnAboutButton { get; }
        

        public MainWindowViewModel()
        {
            // WindowCommands Left
            this.OnGithubButton = new UI.Command<Button>(c => true, f => Process.Start(new ProcessStartInfo("https://github.com/darkstormgames/PKCalc".Replace("&", "^&")) { UseShellExecute = true }));
            // WindowCommands Right
            this.OnDebugButton = new UI.Command<Flyout>(f => f is not null && App.IsDebug, f => f!.SetCurrentValue(Flyout.IsOpenProperty, !f.IsOpen));
            this.OnAboutButton = new UI.Command<Flyout>(f => f is not null, f => f!.SetCurrentValue(Flyout.IsOpenProperty, !f.IsOpen));
        }

        protected override void Cleanup()
        {
            
        }
    }
}
