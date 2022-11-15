using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PKCalc.Converters
{
    internal class FloatToHealthColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is float i)
            {
                if (i > 50)
                    return Brushes.Green;
                else if (i > 20)
                    return Brushes.Yellow;
                else
                    return Brushes.Red;
            }
            else
                return Brushes.Black;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
