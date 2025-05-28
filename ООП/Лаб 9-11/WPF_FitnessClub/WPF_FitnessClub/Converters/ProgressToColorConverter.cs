using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_FitnessClub.Converters
{
    public class ProgressToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double progress)
            {
                // Округляем значение до 2 знаков после запятой
                progress = Math.Round(progress, 2);
                
                // Ограничиваем значения от 0 до 1
                progress = Math.Max(0, Math.Min(1, progress));
                
                // Красный при 0%, зеленый при 100%, желтый при 50%
                if (progress < 0.5)
                {
                    // От красного к желтому (0% - 50%)
                    byte r = 255;
                    byte g = (byte)(255 * (progress * 2)); // 0-255
                    byte b = 0;
                    return new SolidColorBrush(Color.FromRgb(r, g, b));
                }
                else
                {
                    // От желтого к зеленому (50% - 100%)
                    byte r = (byte)(255 * (2 - progress * 2)); // 255-0
                    byte g = 255;
                    byte b = 0;
                    return new SolidColorBrush(Color.FromRgb(r, g, b));
                }
            }
            
            // По умолчанию - серый цвет
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Обратная конвертация не поддерживается
            return Binding.DoNothing;
        }
    }
} 