using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WPF_FitnessClub.Models;
using System.Collections.Generic;
using System.Linq;

namespace WPF_FitnessClub.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return "/Images/default-profile.jpg";

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RatingToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rating && parameter != null)
            {
                int ratingToCheck;
                if (int.TryParse(parameter.ToString(), out ratingToCheck))
                {
                    if (rating >= ratingToCheck)
                    {
                        return Visibility.Visible;
                    }
                }
                else
                {
                    // Если не удалось преобразовать параметр в число, используем значение по умолчанию (1)
                    if (rating >= 1)
                    {
                        return Visibility.Visible;
                    }
                }
            }
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ZeroRatingToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rating && parameter != null)
            {
                int ratingToCheck;
                if (int.TryParse(parameter.ToString(), out ratingToCheck))
                {
                    if (rating < ratingToCheck)
                    {
                        return Visibility.Visible;
                    }
                }
                else
                {
                    // Если не удалось преобразовать параметр в число, используем значение по умолчанию (1)
                    if (rating < 1)
                    {
                        return Visibility.Visible;
                    }
                }
            }
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UserRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserRole role)
            {
                switch (role)
                {
                    case UserRole.Admin:
                        return "Администратор";
                    case UserRole.Client:
                        return "Клиент";
                    case UserRole.Coach:
                        return "Тренер";
                    default:
                        return "Неизвестная роль";
                }
            }

            return "Неизвестная роль";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string roleStr)
            {
                switch (roleStr)
                {
                    case "Администратор":
                        return UserRole.Admin;
                    case "Клиент":
                        return UserRole.Client;
                    case "Тренер":
                        return UserRole.Coach;
                    default:
                        return UserRole.Client;
                }
            }

            return UserRole.Client;
        }
    }

    // Дополнительный класс NullToInvertedVisibilityConverter, который будет использоваться в PersonalAccountView
    public class NullToInvertedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    // Конвертер, который преобразует null в Visibility.Visible, а не-null в Visibility.Collapsed
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    // Конвертер для преобразования рейтинга в коллекцию звезд
    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rating)
            {
                // Создаем список звезд
                var stars = new List<Star>();
                
                // Количество полных звезд
                int fullStars = (int)Math.Floor(rating);
                
                // Есть ли половина звезды
                bool hasHalfStar = (rating - fullStars) >= 0.5;
                
                // Добавляем полные звезды
                for (int i = 0; i < fullStars; i++)
                {
                    stars.Add(new Star { Type = StarType.Full });
                }
                
                // Добавляем половину звезды если нужно
                if (hasHalfStar)
                {
                    stars.Add(new Star { Type = StarType.Half });
                }
                
                // Добавляем пустые звезды до общего количества 5
                while (stars.Count < 5)
                {
                    stars.Add(new Star { Type = StarType.Empty });
                }
                
                return stars;
            }
            
            // Если передано не число, возвращаем 5 пустых звезд
            return Enumerable.Repeat(new Star { Type = StarType.Empty }, 5).ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    
    // Класс для представления звезды
    public class Star
    {
        public StarType Type { get; set; }
    }
    
    // Типы звезд
    public enum StarType
    {
        Empty,  // Пустая звезда
        Half,   // Половина звезды
        Full    // Полная звезда
    }
} 