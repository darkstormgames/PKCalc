using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MahApps.Metro.Controls;

namespace PKCalc.Windows
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SidebarMenu_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            App.Logger.Trace("SidebarItem \"{0}\" clicked.", ((HamburgerMenuItem)e.InvokedItem).Label);
            this.SidebarMenu.Content = e.InvokedItem;
        }
    }
}
