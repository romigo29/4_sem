using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_FitnessClub.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDarkTheme = ThemeManager.Instance.CurrentTheme == ThemeManager.AppTheme.Dark;
            
            if (value is bool isCompleted)
            {
                if (isDarkTheme)
                {
                    // Более тёмные оттенки для тёмной темы
                    return isCompleted 
                        ? new SolidColorBrush(Color.FromRgb(56, 142, 60)) // тёмно-зелёный
                        : new SolidColorBrush(Color.FromRgb(198, 40, 40)); // тёмно-красный
                }
                else
                {
                    // Стандартные цвета для светлой темы
                    return isCompleted 
                        ? new SolidColorBrush(Color.FromRgb(76, 175, 80)) // зелёный
                        : new SolidColorBrush(Color.FromRgb(244, 67, 54)); // красный
                }
            }
            
            // Цвет по умолчанию
            return isDarkTheme 
                ? new SolidColorBrush(Color.FromRgb(97, 97, 97)) // темно-серый для темной темы
                : new SolidColorBrush(Color.FromRgb(158, 158, 158)); // светло-серый для светлой темы
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 