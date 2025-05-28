using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.ViewModels;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WPF_FitnessClub.View
{
	/// <summary>
	/// Логика взаимодействия для RegistrationView.xaml
	/// </summary>
	public partial class RegistrationView : Window
	{
		private RegistrationVM _viewModel;
		private const int MaxPasswordLength = 30;

		public RegistrationView()
		{
			InitializeComponent();

			_viewModel = new RegistrationVM();
			DataContext = _viewModel;

			// Подписываемся на изменения PasswordBox, так как они не поддерживают привязку данных
			PasswordInput.PasswordChanged += PasswordInput_PasswordChanged;
			RegPasswordInput.PasswordChanged += RegPasswordInput_PasswordChanged;
			ConfirmPasswordInput.PasswordChanged += ConfirmPasswordInput_PasswordChanged;

			// Подписываемся на RequestClose
			_viewModel.RequestClose += (s, e) => this.Close();
            
            // Подписываемся на изменение языка
            LanguageManager.Instance.LanguageChanged += LanguageManager_LanguageChanged;
            
            // Обновляем внешний вид кнопок языка при запуске
            UpdateLanguageButtonsAppearance();
		}
        
        private void LanguageManager_LanguageChanged(object sender, string cultureName)
        {
            UpdateLanguageButtonsAppearance();
        }
        
        private void UpdateLanguageButtonsAppearance()
        {
            string currentCulture = CultureInfo.CurrentUICulture.Name.ToLower();
            
            // Сбрасываем стили обеих кнопок
            RussianButton.Background = new SolidColorBrush(Colors.Transparent);
            EnglishButton.Background = new SolidColorBrush(Colors.Transparent);
            
            // Выделяем активную кнопку
            if (currentCulture.StartsWith("ru"))
            {
                RussianButton.BorderBrush = new SolidColorBrush(Colors.White);
                RussianButton.BorderThickness = new Thickness(2);
                EnglishButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                EnglishButton.BorderThickness = new Thickness(1);
                
                // Устанавливаем тег для специального стиля
                RussianButton.Tag = "Active";
                EnglishButton.Tag = "Normal";
            }
            else
            {
                EnglishButton.BorderBrush = new SolidColorBrush(Colors.White);
                EnglishButton.BorderThickness = new Thickness(2);
                RussianButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                RussianButton.BorderThickness = new Thickness(1);
                
                // Устанавливаем тег для специального стиля
                EnglishButton.Tag = "Active";
                RussianButton.Tag = "Normal";
            }
        }

		private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
			{
				_viewModel.Password = PasswordInput.Password;
			}
		}

		private void RegPasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
			{
				_viewModel.RegPassword = RegPasswordInput.Password;
			}
		}

		private void ConfirmPasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
			{
				_viewModel.ConfirmPassword = ConfirmPasswordInput.Password;
			}
		}

		private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			var passwordBox = sender as PasswordBox;
			if (passwordBox != null)
			{
				// Если длина пароля достигла максимума и нажата клавиша, добавляющая символ
				if (passwordBox.Password.Length >= MaxPasswordLength && 
					e.Key != Key.Back && e.Key != Key.Delete && 
					e.Key != Key.Left && e.Key != Key.Right && 
					e.Key != Key.Tab && e.Key != Key.Home && 
					e.Key != Key.End && !Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
				{
					e.Handled = true; // Блокируем ввод
				}
			}
		}

		private void RussianButton_Click(object sender, RoutedEventArgs e)
		{
			LanguageManager.Instance.ChangeLanguage("ru-RU");
		}

		private void EnglishButton_Click(object sender, RoutedEventArgs e)
		{
			LanguageManager.Instance.ChangeLanguage("en-US");
		}

		private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MainTabControl.SelectedIndex == 0)
			{
				MainBorder.Height = 300;
			}
			else if (MainTabControl.SelectedIndex == 1)
			{
				MainBorder.Height = 420;
			}
		}
        
        protected override void OnClosed(EventArgs e)
        {
            // Отписываемся от события при закрытии окна
            LanguageManager.Instance.LanguageChanged -= LanguageManager_LanguageChanged;
            base.OnClosed(e);
        }
	}
}