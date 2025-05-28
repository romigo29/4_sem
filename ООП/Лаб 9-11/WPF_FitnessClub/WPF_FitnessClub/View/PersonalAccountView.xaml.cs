using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using WPF_FitnessClub.Data.Services.Interfaces;
using WPF_FitnessClub.Data.Services;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.ViewModels;

namespace WPF_FitnessClub.View
{
	/// <summary>
	/// Логика взаимодействия для PersonalAccountView.xaml
	/// </summary>
	public partial class PersonalAccountView : UserControl
	{
		private PersonalAccountVM _viewModel;
		private User _user;
		private const int MaxPasswordLength = 30;

		public PersonalAccountView(User user)
		{
			InitializeComponent();
			
			// Получаем сервисы из DI или создаем их
			var workoutPlanService = new WorkoutPlanService();
			var nutritionPlanService = new NutritionPlanService();
			
			_viewModel = new PersonalAccountVM(user, workoutPlanService, nutritionPlanService);
			_viewModel.LanguageChanged += OnLanguageChanged;
			//_viewModel.RequestClose += OnRequestClose;
			
			DataContext = _viewModel;
			
			SecurityCurrentPasswordInput.TextChanged += CurrentPasswordBox_PasswordChanged;
			SecurityNewPasswordInput.PasswordChanged += NewPasswordBox_PasswordChanged;
			SecurityConfirmPasswordInput.PasswordChanged += ConfirmPasswordBox_PasswordChanged;

			_user = user;

			// Подписываемся на изменение темы
			ThemeManager.Instance.PropertyChanged += ThemeManager_PropertyChanged;
			
			// Подписываемся на событие выгрузки контрола
			this.Unloaded += PersonalAccountView_Unloaded;

			// Установка выбранного языка в ComboBox при инициализации
			string currentLanguage = LanguageManager.Instance.CurrentLanguage;
			if (currentLanguage.StartsWith("ru"))
			{
				LanguageSelectionComboBox.SelectedIndex = 0; // Russian
			}
			else
			{
				LanguageSelectionComboBox.SelectedIndex = 1; // English
			}

			// Установка выбранной темы в ComboBox при инициализации
			if (ThemeManager.Instance.CurrentTheme == ThemeManager.AppTheme.Light)
			{
				ThemeSelectionComboBox.SelectedIndex = 0; // Light theme
			}
			else
			{
				ThemeSelectionComboBox.SelectedIndex = 1; // Dark theme
			}
		}

		private void EmailTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (sender is TextBox textBox)
			{
				// Получаем полный текст, который будет после вставки
				string fullText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

				// Если в тексте есть символ '@'
				if (fullText.Contains("@"))
				{
					int atIndex = fullText.IndexOf('@');
					
					// Если курсор находится после символа '@' (в доменной части)
					if (textBox.CaretIndex > atIndex)
					{
						// Проверяем, что после '@' вводятся только буквы, точки или дефисы
						if (!Regex.IsMatch(e.Text, @"^[a-zA-Z.\-]$"))
						{
							e.Handled = true; // Блокируем ввод
							return;
						}
						
						// Проверка, что между точками есть хотя бы одна буква
						string domainPart = fullText.Substring(atIndex + 1) + e.Text;
						
						// Проверяем, что не будет двух точек подряд
						if (domainPart.Contains(".."))
						{
							e.Handled = true;
							return;
						}
					}
					// Если курсор находится до символа '@' (в локальной части)
					else
					{
						// Проверяем, что в локальной части вводятся только буквы, цифры, точки, дефисы и символы подчеркивания
						if (!Regex.IsMatch(e.Text, @"^[a-zA-Z0-9._\-]$"))
						{
							e.Handled = true; // Блокируем ввод
							return;
						}
						
						// Проверяем, что не будет двух точек подряд в локальной части
						string localPart = fullText.Substring(0, atIndex);
						if (localPart.Contains(".."))
						{
							e.Handled = true;
							return;
						}
					}
				}
				// Если символа '@' еще нет в тексте
				else
				{
					// Проверяем, что вводится допустимый символ для локальной части
					if (!Regex.IsMatch(e.Text, @"^[a-zA-Z0-9._\-@]$"))
					{
						e.Handled = true; // Блокируем ввод
						return;
					}
					
					// Проверяем, что не будет двух точек подряд
					if (fullText.Contains(".."))
					{
						e.Handled = true;
						return;
					}
				}
			}
		}

		private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null && sender is TextBox textBox)
			{
				_viewModel.ValidateEmail(textBox.Text);
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

		private void OnLanguageChanged(object sender, string language)
		{
			// Обработка смены языка
			// Можно добавить дополнительные действия при необходимости
		}
		

		private void CurrentPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
				_viewModel.CurrentPassword = SecurityCurrentPasswordInput.Text;
		}

		private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
				_viewModel.NewPassword = SecurityNewPasswordInput.Password;
		}

		private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
				_viewModel.ConfirmPassword = SecurityConfirmPasswordInput.Password;
		}

		private void LanguageSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (LanguageSelectionComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
			{
				string languageCode = selectedItem.Tag.ToString();
				LanguageManager.Instance.ChangeLanguage(languageCode);
				
				// Обновляем культуру для правильного отображения валюты
				Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
				CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
				customCulture.NumberFormat.CurrencySymbol = "Br";
				customCulture.NumberFormat.CurrencyDecimalSeparator = ",";
				customCulture.NumberFormat.CurrencyGroupSeparator = " ";
				Thread.CurrentThread.CurrentCulture = customCulture;
			}
		}

		private void ThemeSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ThemeSelectionComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
			{
				string themeCode = selectedItem.Tag.ToString();
				ThemeManager.Instance.ChangeTheme(themeCode == "Light" ? ThemeManager.AppTheme.Light : ThemeManager.AppTheme.Dark);
			}
		}

		private void ClearPasswordFields_Click(object sender, RoutedEventArgs e)
		{
			SecurityCurrentPasswordInput.Clear();
			SecurityNewPasswordInput.Clear();
			SecurityConfirmPasswordInput.Clear();
		}

		private void ThemeManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentThemeString")
			{
				// Обновляем выбранный элемент в ComboBox при изменении темы
				if (ThemeManager.Instance.CurrentTheme == ThemeManager.AppTheme.Light)
				{
					ThemeSelectionComboBox.SelectedIndex = 0; // Light theme
				}
				else
				{
					ThemeSelectionComboBox.SelectedIndex = 1; // Dark theme
				}
			}
		}
		
		private void PersonalAccountView_Unloaded(object sender, RoutedEventArgs e)
		{
			// Автоматически отписываемся от событий при выгрузке контрола
			Unload();
		}

		public void Unload()
		{
			// Отписываемся от событий при выгрузке контрола
			ThemeManager.Instance.PropertyChanged -= ThemeManager_PropertyChanged;
			_viewModel.LanguageChanged -= OnLanguageChanged;
		}
	}
}
