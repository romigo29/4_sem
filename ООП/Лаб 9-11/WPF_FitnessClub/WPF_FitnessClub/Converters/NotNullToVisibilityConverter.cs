using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_FitnessClub.Converters
{
    public class NotNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если параметр указан и равен "Inverse", инвертируем результат
            if (parameter is string paramStr && paramStr == "Inverse")
            {
                return value == null ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Конвертация обратно не имеет смысла, т.к. мы не можем восстановить объект из Visibility
            return Binding.DoNothing;
        }
    }
} 