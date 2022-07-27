using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskDialogInterop;
using ControlzEx.Theming;

namespace PKCalc.UI
{
    /// <see cref="https://mahapps.com/docs/themes/usage"/>
    /// <example>
    /// Possible Theme Options:
    ///     Primary:   Light, Dark
    ///     Secondary: Red, Green, Blue, Purple, Orange, Lime, Emerald, Teal, Cyan, Cobalt, 
    ///                Indigo, Violet, Pink, Magenta, Crimson, Amber, Yellow, Brown, Olive, 
    ///                Steel, Mauve, Taupe, Sienna
    /// </example>


    public static class TaskDialogHelper
    {
        private static bool messageCallback(IActiveTaskDialog dialog, TaskDialogNotificationArgs args, object data)
        {
            bool result = false;
            switch (args.Notification)
            {
                case TaskDialogNotification.HyperlinkClicked:
                    Process.Start(new ProcessStartInfo(args.Hyperlink.Replace("&", "^&")) { UseShellExecute = true });
                    result = true;
                    break;
            }
            return result;
        }

        public static void ShowCriticalError(Exception ex)
        {
            TaskDialogOptions config = new()
            {
                Title = "Critical Error",
                MainInstruction = "An error has occurred. The application will now close.",
                Content = "Try downloading the newest version of this application from <a href=\"https://github.com/darkstormgames/PKCalc/releases\">GitHub</a>.",
                MainIcon = TaskDialogIcon.Error,
                ExpandedInfo = ex?.Message,
                AllowDialogCancellation = false,
                CustomButtons = new[] { "&Send Error Data and Close", "Close" },
                Callback = messageCallback,
                Theme = "Light.Crimson"
            };
            TaskDialogResult result = TaskDialog.Show(config);
            switch (result.CustomButtonResult)
            {
                case 0:
                    string errorData = $"{ex.Message}\n{ex.StackTrace}";
                    errorData = errorData.Replace('"', '\'').Replace("\r\n", @"\n");
                    break;
            }
        }

        public static void ShowError(Exception ex)
        {
            TaskDialogOptions config = new()
            {
                Title = "Error",
                MainInstruction = "An error has occurred.",
                Content = ex?.Message,
                MainIcon = TaskDialogIcon.Error,
                AllowDialogCancellation = false,
                CustomButtons = new[] { "&Send Error Data and Continue", "Continue" },
                Callback = messageCallback,
                Theme = "Light.Crimson"
            };
            if (Configuration.RegistryHelper.GetLogLevel().Ordinal <= 1)
                config.ExpandedInfo = ex?.StackTrace;

            TaskDialogResult result = TaskDialog.Show(config);
            switch (result.CustomButtonResult)
            {
                case 0:
                    string errorData = $"{ex.Message}\n{ex.StackTrace}";
                    errorData = errorData.Replace('"', '\'').Replace("\r\n", @"\n");
                    break;
            }
        }

        public static void ShowDebugMessage(string caption, string message)
        {
            TaskDialogOptions config = new()
            {
                Title = "Debug Message",
                MainInstruction = caption,
                Content = message,
                MainIcon = TaskDialogIcon.Information,
                AllowDialogCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Close,
                Callback = messageCallback,
                Theme = "Light.Pink"
            };
            TaskDialogResult result = TaskDialog.Show(config);
        }
    }
}
