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
using System.Text.RegularExpressions;

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для Registration.xaml
	/// </summary>
	public partial class Registration : Window
	{

		private readonly string usersFilePath = "users.js";
		List<User> _users;

		public Registration()
		{
			InitializeComponent();


			_users = LoadUsers(usersFilePath);
			SaveUsers();

		}

		public List<User> LoadUsers(string usersFilePath)
		{
			if (File.Exists(usersFilePath))
			{
				try
				{
					string json = File.ReadAllText(usersFilePath);
					return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
				}
				catch (Exception ex)
				{
					MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorLoadingData"], ex.Message));
					return new List<User>();
				}
			}
			else
			{
				return new List<User>
			{
				new User("Бибизяна", "test@gmail.com", "", "", UserRole.Admin),
				new User("Тренер", "test@gmail.com", "1", "1", UserRole.Coach),
				new User("Клиент", "test@gmail.com", "2", "2", UserRole.Client)
			};
			}
		}

		public void SaveUsers()
		{
			try
			{
				string json = JsonConvert.SerializeObject(_users, Formatting.Indented);
				File.WriteAllText(usersFilePath, json);
			}

			catch (Exception ex)
			{
				MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorSavingData"], ex.Message));
			}
		}

		private void OpenMainWindow(User user)
		{
			MainWindow mainWindow = new MainWindow(user);
			mainWindow.Show();
			Close();
		}

		private void EnterButton_Click(object sender, RoutedEventArgs e)
		{
			string EnteredLogin = LoginInput.Text.Trim();
			string EnteredPassword = PasswordInput.Password;

			bool loginSuccessful = false;

			foreach (var user in _users)
			{
				if (EnteredLogin == user.Login && EnteredPassword == user.Password)
				{
					loginSuccessful = true;
					
					switch (user.Role)
					{
						case UserRole.Admin:
							{
								MessageBox.Show($"{Application.Current.Resources["EnteredAdmin"]}");
								OpenMainWindow(user);
								break;
							}
						case UserRole.Coach:
							{
								MessageBox.Show($"{Application.Current.Resources["EnteredCoach"]}");
								OpenMainWindow(user);
								break;
							}
						case UserRole.Client:
							{
								MessageBox.Show($"{Application.Current.Resources["EnteredClient"]}");
								OpenMainWindow(user);
								break;
							}
					}
					
					break; 
				}
			}

			// Проверяем результат после перебора всех пользователей
			if (!loginSuccessful)
			{
				MessageBox.Show($"{Application.Current.Resources["FailedLogin"]}");
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

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			string fullName = FullNameInput.Text.Trim();
			string email = EmailInput.Text.Trim();
			string login = RegLoginInput.Text.Trim();
			string password = RegPasswordInput.Password.Trim();
			string confirmPassword = ConfirmPasswordInput.Password.Trim();

			if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
				string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) ||
				string.IsNullOrEmpty(confirmPassword))
			{
				ShowWarning("EmptyFields");
				return;
			}

			if (!IsValid(fullName, @"^([A-Za-zА-Яа-яЁё]+(-[A-Za-zА-Яа-яЁё]+)?\s){2}[A-Za-zА-Яа-яЁё]+(-[A-Za-zА-Яа-яЁё]+)?$", "InvalidFullName")) return;
			if (!IsValid(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "InvalidEmail")) return;
			if (!IsValid(login, @"^[a-zA-Z0-9]+$", "InvalidLogin")) return;
			if (!IsValid(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", "InvalidPassword")) return;

			if (password != confirmPassword)
			{
				ShowWarning("PasswordMismatch");
				return;
			}

			if (_users.Any(u => u.Email == email))
			{
				ShowWarning("ExistingUser", "ErrorTitle");
				return;
			}
			User _newUser = new User(fullName, email, login, password, UserRole.Client);
			_users.Add(_newUser);
			SaveUsers();

			MessageBox.Show($"{Application.Current.Resources["SuccessSignIn"]}");
			OpenMainWindow(_newUser);
		}

		private bool IsValid(string input, string pattern, string errorKey)
		{
			if (!Regex.IsMatch(input, pattern))
			{
				ShowWarning(errorKey);
				return false;
			}
			return true;
		}

		private void ShowWarning(string messageKey, string titleKey = "ValidationErrorTitle")
		{
			MessageBox.Show(
				(string)Application.Current.Resources[messageKey],
				(string)Application.Current.Resources[titleKey],
				MessageBoxButton.OK,
				MessageBoxImage.Warning);
		}


	}
}