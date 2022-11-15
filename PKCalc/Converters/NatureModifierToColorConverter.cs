using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using ControlzEx.Theming;

namespace PKCalc.Converters
{
    internal class NatureModifierToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is float f)
            {
                if (f > 1)
                    return Brushes.IndianRed;
                else if (f < 1)
                    return Brushes.LightBlue;
                else
                    return (Brush)ThemeManager.Current.DetectTheme(Application.Current).Resources["MahApps.Brushes.Text"];
            }
            else
                return (Brush)ThemeManager.Current.DetectTheme(Application.Current).Resources["MahApps.Brushes.Text"];

        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
