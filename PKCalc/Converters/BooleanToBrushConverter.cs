using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PKCalc.Converters
{
    internal class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                string paraString = "Red|Green";
                if (parameter != null && parameter is string str && !string.IsNullOrEmpty(str))
                {
                    paraString = str;
                }
                string[] parameters = paraString.Split('|');

                if (boolValue)
                {
                    App.Logger.Trace("BooleanToBrushConverter: {0}", parameters[1]);
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(parameters[1]));
                }
                else
                {
                    App.Logger.Trace("BooleanToBrushConverter: {0}", parameters[0]);
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(parameters[0]));
                }
            }
            App.Logger.Trace("BooleanToBrushConverter: Default (Gray)");
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
