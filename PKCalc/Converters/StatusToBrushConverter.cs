using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PKCalc.Converters
{
    internal class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Client.Status status)
            {
                App.Logger.Trace("StatusToBrushConverter: {0}", status.ToString());
                return status switch
                {
                    Client.Status.Enabled => Brushes.Green,
                    Client.Status.Disrupted => Brushes.Yellow,
                    Client.Status.Paused => Brushes.Blue,
                    Client.Status.Disabled => Brushes.Red,
                    _ => Brushes.Gray,
                };
            }
            App.Logger.Trace("StatusToBrushConverter: Default (Gray)");
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
