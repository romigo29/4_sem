using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_FitnessClub.Converters
{
    /// <summary>
    /// Конвертер, который комбинирует два значения Visibility с помощью логического ИЛИ
    /// </summary>
    public class OrVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если любое из значений Visible, то результат будет Visible
            bool isVisible = false;

            // Проверяем первое значение
            if (value is Visibility visibility1)
            {
                isVisible = visibility1 == Visibility.Visible;
            }

            // Проверяем второе значение (если передано)
            if (!isVisible && parameter is Visibility visibility2)
            {
                isVisible = visibility2 == Visibility.Visible;
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 