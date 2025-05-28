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

namespace WPF_FitnessClub.View
{
	/// <summary>
	/// Логика взаимодействия для RegistrationView.xaml
	/// </summary>
	public partial class RegistrationView : Window
	{
		private RegistrationVM _viewModel;

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
	}
}