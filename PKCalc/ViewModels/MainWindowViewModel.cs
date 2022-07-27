using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace PKCalc.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ICommand OnGithubButton { get; }
        public ICommand OnThemeToggle { get; }
        public ICommand OnDebugButton { get; }
        public ICommand OnAboutButton { get; }
        public ICommand SideBar_ItemInvoked { get; }
                
        public MainWindowViewModel()
        {
            // WindowCommands Left
            this.OnGithubButton = new UI.Command<Button>(c => true, f => Process.Start(new ProcessStartInfo("https://github.com/darkstormgames/PKCalc".Replace("&", "^&")) { UseShellExecute = true }));
            // WindowCommands Right
            this.OnThemeToggle = new UI.Command<ToggleSwitch>(o => true,
                x => {
                    if (x != null && !x.IsOn)
                    {
                        ThemeManager.Current.ChangeTheme(Application.Current, "Light.Steel");
                        App.Logger.Trace("Theme changed to Light.Steel.");
                    }
                    else
                    {
                        ThemeManager.Current.ChangeTheme(Application.Current, "Dark.Steel");
                        App.Logger.Trace("Theme changed to Dark.Steel.");
                    }
                });
            this.OnDebugButton = new UI.Command<Flyout>(f => f is not null && App.IsDebug, f => f!.SetCurrentValue(Flyout.IsOpenProperty, !f.IsOpen));
            this.OnAboutButton = new UI.Command<Flyout>(f => f is not null, f => f!.SetCurrentValue(Flyout.IsOpenProperty, !f.IsOpen));

            // Sidebar
            this.SideBar_ItemInvoked = new UI.Command<HamburgerMenu>(o => o is not null,
                x => {
                    if (x != null)
                    {
                        if (x.SelectedOptionsItem is HamburgerMenuItem SelectedOptionsItem && x.SelectedOptionsItem != x.Content)
                        {
                            App.Logger.Trace("SidebarOptionsItem \"{0}\" clicked.", SelectedOptionsItem.Label);
                            x.Content = x.SelectedOptionsItem;
                        }
                        else if (x.SelectedItem is HamburgerMenuItem SelectedItem && x.SelectedItem != x.Content)
                        {
                            App.Logger.Trace("SidebarItem \"{0}\" clicked.", SelectedItem.Label);
                            x.Content = x.SelectedItem;
                        }
                    }
                });
            
        }

        protected override void Cleanup()
        {
            
        }
    }
}
