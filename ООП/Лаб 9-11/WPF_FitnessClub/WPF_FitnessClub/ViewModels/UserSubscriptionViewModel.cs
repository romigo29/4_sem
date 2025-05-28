using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using WPF_FitnessClub.Models;

namespace WPF_FitnessClub.ViewModels
{
    public class UserSubscriptionViewModel : ViewModelBase
    {
        private readonly UserSubscription _userSubscription;

        public UserSubscriptionViewModel(UserSubscription userSubscription)
        {
            _userSubscription = userSubscription ?? throw new ArgumentNullException(nameof(userSubscription));
            
            // Подписываемся на изменение темы
            ThemeManager.Instance.ThemeChanged += ThemeManager_ThemeChanged;
        }
        
        // Обработчик изменения темы
        private void ThemeManager_ThemeChanged(object sender, ThemeManager.AppTheme e)
        {
            OnPropertyChanged(nameof(StatusColor));
        }
        
        // Отписка от событий при удалении объекта
        public void Dispose()
        {
            ThemeManager.Instance.ThemeChanged -= ThemeManager_ThemeChanged;
        }

        public int Id => _userSubscription.Id;
        public DateTime PurchaseDate => _userSubscription.PurchaseDate;
        public DateTime ExpiryDate => _userSubscription.ExpiryDate;
        public Subscription Subscription => _userSubscription.Subscription;
        public bool IsCanceled => _userSubscription.IsCanceled;

        public bool IsExpired => ExpiryDate < DateTime.Now;
        public string StatusText
        {
            get
            {
                if (IsCanceled) 
                    return (string)System.Windows.Application.Current.Resources["StatusCancelled"];
                if (IsExpired) 
                    return (string)System.Windows.Application.Current.Resources["StatusExpired"];
                if (PurchaseDate > DateTime.Now) 
                    return (string)System.Windows.Application.Current.Resources["StatusPending"];
                
                return (string)System.Windows.Application.Current.Resources["StatusActive"];
            }
        }

        public Brush StatusColor
        {
            get
            {
                bool isDarkTheme = ThemeManager.Instance.CurrentTheme == ThemeManager.AppTheme.Dark;
                
                if (IsCanceled)
                {
                    // Красный цвет для отмененного абонемента
                    return isDarkTheme 
                        ? new SolidColorBrush(Color.FromRgb(229, 115, 115)) // Светло-красный для темной темы
                        : new SolidColorBrush(Color.FromRgb(183, 28, 28));  // Темно-красный для светлой темы
                }
                
                if (IsExpired)
                {
                    // Серый цвет
                    return isDarkTheme 
                        ? new SolidColorBrush(Color.FromRgb(180, 180, 180)) // Светло-серый для темной темы
                        : Brushes.Gray;
                }
                
                if (PurchaseDate > DateTime.Now)
                {
                    // Оранжевый цвет
                    return isDarkTheme 
                        ? new SolidColorBrush(Color.FromRgb(255, 167, 38)) // Ярко-оранжевый для темной темы
                        : Brushes.DarkOrange;
                }
                
                // Зеленый цвет
                return isDarkTheme 
                    ? new SolidColorBrush(Color.FromRgb(76, 175, 80)) // Более яркий зеленый для темной темы
                    : Brushes.Green;
            }
        }

    }
} 