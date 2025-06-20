using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_FitnessClub.Converters
{
    /// <summary>
    /// Конвертер, который преобразует отрицательное булево значение в Visibility
    /// true -> Visibility.Collapsed, false -> Visibility.Visible
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
} 