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

		public PersonalAccountView(User user)
		{
			InitializeComponent();
			
			_viewModel = new PersonalAccountVM(user);
			_viewModel.LanguageChanged += OnLanguageChanged;
			//_viewModel.RequestClose += OnRequestClose;
			
			DataContext = _viewModel;
			
			SecurityCurrentPasswordInput.TextChanged += CurrentPasswordBox_PasswordChanged;
			SecurityNewPasswordInput.PasswordChanged += NewPasswordBox_PasswordChanged;
			SecurityConfirmPasswordInput.PasswordChanged += ConfirmPasswordBox_PasswordChanged;

			_user = user;

			// Установка выбранного языка в ComboBox при инициализации
			string currentLanguage = Thread.CurrentThread.CurrentUICulture.Name;
			if (currentLanguage.StartsWith("ru"))
			{
				LanguageSelectionComboBox.SelectedIndex = 0; // Russian
			}
			else
			{
				LanguageSelectionComboBox.SelectedIndex = 1; // English
			}

			// Проверка текущей темы
			if (ThemeManager.Instance.CurrentTheme == ThemeManager.AppTheme.Light)
			{
				LightThemeRadioButton.IsChecked = true;
			}
			else
			{
				DarkThemeRadioButton.IsChecked = true;
			}
		}

		private void OnLanguageChanged(object sender, string language)
		{
			// Обработка смены языка
			// Можно добавить дополнительные действия при необходимости
		}
		
		//private void OnRequestClose(object sender, EventArgs e)
		//{
		//	// Уведомляем родительский контейнер о необходимости закрытия
		//	Window.GetWindow(this)?.Close();
		//}

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

		private void LightThemeRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			ThemeManager.Instance.ChangeTheme(ThemeManager.AppTheme.Light);
		}

		private void DarkThemeRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			ThemeManager.Instance.ChangeTheme(ThemeManager.AppTheme.Dark);
		}

		private void ClearPasswordFields_Click(object sender, RoutedEventArgs e)
		{
			SecurityCurrentPasswordInput.Clear();
			SecurityNewPasswordInput.Clear();
			SecurityConfirmPasswordInput.Clear();
		}
	}
}
