using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_FitnessClub.Converters
{
    /// <summary>
    /// Конвертер для инвертирования логического значения (true -> false, false -> true)
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            
            return false;
        }
    }
} 