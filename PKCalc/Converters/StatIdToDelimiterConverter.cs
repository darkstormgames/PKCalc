using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using PKCalc.Client.Pokemon;

namespace PKCalc.Converters
{
    internal class StatIdToDelimiterConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatEnum statId)
                return statId switch
                {
                    StatEnum.Attack or 
                        StatEnum.Defense or 
                        StatEnum.SpecialAttack or 
                        StatEnum.SpecialDefense or 
                        StatEnum.Speed
                      => "|",
                    _ => "",
                };
            else
                return "";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
