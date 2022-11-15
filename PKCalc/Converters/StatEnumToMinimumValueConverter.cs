using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PKCalc.Converters
{
    internal class StatEnumToMinimumValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Client.Pokemon.StatEnum stat)
            {
                if (stat == Client.Pokemon.StatEnum.HP)
                    return 1;
            }
            return 5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
