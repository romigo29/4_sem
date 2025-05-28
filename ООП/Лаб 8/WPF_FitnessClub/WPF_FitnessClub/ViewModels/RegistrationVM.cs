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
using static WPF_FitnessClub.Commands;
using WPF_FitnessClub.ViewModels;
using System.Text.RegularExpressions;
using WPF_FitnessClub.Data;

namespace WPF_FitnessClub.ViewModels
{
	public class RegistrationVM : ViewModelBase
	{

		#region Приватные поля

		string _login;
		string _password;
		string _fullname;
		string _email;
		string _regLogin;
		string _regPassword;
		string _confirmPassword;


		private readonly string usersFilePath = "users.js";
		readonly List<User> _users;

		#endregion

		#region Свойства для привязки комманд

		public string Login
		{
			get => _login;
			set
			{
				if (_login != value)
				{
					_login = value;
					OnPropertyChanged(nameof(Login));
				}
			}
		}

		public string Password
		{
			get => _password;
			set
			{
				if (_password != value)
				{
					_password = value;
					OnPropertyChanged(nameof(Password));
				}
			}
		}

		public string FullName
		{
			get => _fullname;
			set
			{
				if (_fullname != value)
				{
					_fullname = value;
					OnPropertyChanged(nameof(FullName));
				}	
			}
		}

		public string Email
		{
			get => _email;
			set
			{
				if (_email != value)
				{
					_email = value;
					OnPropertyChanged(nameof(Email));
				}
			}
		}

		public string RegLogin
		{
			get => _regLogin;
			set
			{
				if (_regLogin != value)	
				{
					_regLogin = value;
					OnPropertyChanged(nameof(RegLogin));
				}
			}
		}

		public string RegPassword	
		{
			get => _regPassword;
			set
			{
				if (_regPassword != value)
				{
					_regPassword = value;
					OnPropertyChanged(nameof(RegPassword));
				}
			}
		}
		
		public string ConfirmPassword
		{
			get => _confirmPassword;
			set
			{
				if (_confirmPassword != value)
				{
					_confirmPassword = value;
					OnPropertyChanged(nameof(ConfirmPassword));
				}
			}	
		}

		
		#endregion


		#region Комманды

		public ICommand EnterCommand { get; set; }
		public ICommand RegisterCommand {  get; set; }

		#endregion

		#region События
		public event EventHandler RequestClose;
		#endregion

		#region Методы команд

		private void RaiseRequestClose()
		{
			RequestClose?.Invoke(this, EventArgs.Empty);
		}

		private void OpenMainWindow(User user)
		{
			// Создание основного окна программы
			MainWindow mainWindow = new MainWindow(user);
			mainWindow.Show();
			
			// Закрываем текущее окно
			RaiseRequestClose();
		}

		#endregion

		public RegistrationVM()
		{
			_users = LoadUsers(usersFilePath);
			SaveUsers();

			EnterCommand = new RelayCommand(ExecuteEnterCommand);
			RegisterCommand = new RelayCommand(ExecuteRegisterCommand);
		}


		public List<User> LoadUsers(string usersFilePath)
		{
			try
			{
				// Попытка загрузить данные из базы данных
				DataAccess dataAccess = new DataAccess();
				List<User> usersFromDb = dataAccess.GetAllUsers();
				
				// Если из БД получены пользователи, возвращаем их
				if (usersFromDb != null && usersFromDb.Count > 0)
				{
					return usersFromDb;
				}
				
				// Если в БД нет данных, пробуем загрузить из JSON
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
						return CreateDefaultUsers();
					}
				}
				else
				{
					// Если JSON-файл не существует, создаем пользователей по умолчанию
					return CreateDefaultUsers();
				}
			}
			catch (Exception ex)
			{
				// Обработка общих ошибок
				MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorLoadingData"], ex.Message));
				return CreateDefaultUsers();
			}
		}
		
		private List<User> CreateDefaultUsers()
		{
			return new List<User>
			{
				new User("Бибизяна", "test@gmail.com", "", "", UserRole.Admin),
				new User("Тренер", "test@gmail.com", "1", "1", UserRole.Coach),
				new User("Клиент", "test@gmail.com", "2", "2", UserRole.Client)
			};
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

		private void ExecuteEnterCommand(object parameter)
		{
			// Удаляем прямые обращения к UI элементам
			string EnteredLogin = Login?.Trim();
			string EnteredPassword = Password;

			if (string.IsNullOrEmpty(EnteredLogin) || string.IsNullOrEmpty(EnteredPassword))
			{
				MessageBox.Show($"{Application.Current.Resources["FailedLogin"]}");
				return;
			}

			try
			{
				// Попытка аутентификации через базу данных
				DataAccess dataAccess = new DataAccess();
				User user = dataAccess.GetUserByLogin(EnteredLogin);
				
				if (user != null && user.Password == EnteredPassword)
				{
					// Успешная аутентификация
					string enteredMessage = "";
					
					switch (user.Role)
					{
						case UserRole.Admin:
							enteredMessage = "EnteredAdmin";
							break;
						case UserRole.Coach:
							enteredMessage = "EnteredCoach";
							break;
						case UserRole.Client:
							enteredMessage = "EnteredClient";
							break;
					}
					
					MessageBox.Show($"{Application.Current.Resources[enteredMessage]}");
					OpenMainWindow(user);
					return;
				}
				
				// Если аутентификация через БД не удалась, проверяем локальный список
				bool loginSuccessful = false;
				
				foreach (var localUser in _users)
				{
					if (EnteredLogin == localUser.Login && EnteredPassword == localUser.Password)
					{
						loginSuccessful = true;

						switch (localUser.Role)
						{
							case UserRole.Admin:
								MessageBox.Show($"{Application.Current.Resources["EnteredAdmin"]}");
								OpenMainWindow(localUser);
								break;
							case UserRole.Coach:
								MessageBox.Show($"{Application.Current.Resources["EnteredCoach"]}");
								OpenMainWindow(localUser);
								break;
							case UserRole.Client:
								MessageBox.Show($"{Application.Current.Resources["EnteredClient"]}");
								OpenMainWindow(localUser);
								break;
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
			catch (Exception ex)
			{
				// Если произошла ошибка при работе с БД, пробуем аутентификацию по локальному списку
				bool loginSuccessful = false;

				foreach (var user in _users)
				{
					if (EnteredLogin == user.Login && EnteredPassword == user.Password)
					{
						loginSuccessful = true;

						switch (user.Role)
						{
							case UserRole.Admin:
								MessageBox.Show($"{Application.Current.Resources["EnteredAdmin"]}");
								OpenMainWindow(user);
								break;
							case UserRole.Coach:
								MessageBox.Show($"{Application.Current.Resources["EnteredCoach"]}");
								OpenMainWindow(user);
								break;
							case UserRole.Client:
								MessageBox.Show($"{Application.Current.Resources["EnteredClient"]}");
								OpenMainWindow(user);
								break;
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
		}

		private void ExecuteRegisterCommand(object parameter)
		{
			// Используем свойства вместо прямого обращения к UI-элементам
			string fullName = FullName?.Trim();
			string email = Email?.Trim();
			string login = RegLogin?.Trim();
			string password = RegPassword;
			string confirmPass = ConfirmPassword;

			// Проверка на пустые значения
			if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || 
				string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || 
				string.IsNullOrEmpty(confirmPass))
			{
				ShowWarning("FillAllFields");
				return;
			}

			// Валидация данных
			if (!IsValid(fullName, @"^([A-Za-zА-Яа-яЁё]+(-[A-Za-zА-Яа-яЁё]+)?\s){2}[A-Za-zА-Яа-яЁё]+(-[A-Za-zА-Яа-яЁё]+)?$", "InvalidFullName")) return;
			if (!IsValid(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "InvalidEmail")) return;
			if (!IsValid(login, @"^[a-zA-ZА-Яа-я0-9]+$", "InvalidLogin")) return;
			if (!IsValid(password, @"^.{6,}$", "PasswordTooShort")) return;

			// Проверка паролей
			if (password != confirmPass)
			{
				ShowWarning("PasswordMismatch");
				return;
			}

			try
			{
				// Создание нового пользователя
				var newUser = new User(fullName, email, login, password, UserRole.Client);
				
				// Проверка, существует ли пользователь в БД
				DataAccess dataAccess = new DataAccess();
				User existingUser = dataAccess.GetUserByLogin(login);
				
				if (existingUser != null)
				{
					ShowWarning("LoginExists");
					return;
				}
				
				// Пробуем добавить пользователя в БД
				try
				{
					int userId = dataAccess.AddUser(newUser);
					if (userId > 0)
					{
						// Пользователь успешно добавлен в БД
						MessageBox.Show(
							(string)Application.Current.Resources["RegistrationSuccess"],
							(string)Application.Current.Resources["SuccessTitle"],
							MessageBoxButton.OK,
							MessageBoxImage.Information);
							
						// После успешной регистрации выполняем вход
						Login = login;
						Password = password;
						OpenMainWindow(newUser);
						return;
					}
				}
				catch
				{
					// Если не удалось добавить в БД, добавляем в локальный список
					if (_users.Any(u => u.Login == login))
					{
						ShowWarning("LoginExists");
						return;
					}
					
					_users.Add(newUser);
					SaveUsers();
					
					MessageBox.Show(
						(string)Application.Current.Resources["RegistrationSuccess"],
						(string)Application.Current.Resources["SuccessTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Information);
						
					// После успешной регистрации выполняем вход
					Login = login;
					Password = password;
					OpenMainWindow(newUser);
				}
			}
			catch (Exception ex)
			{
				// Если произошла ошибка, пробуем использовать локальный список
				// Проверка на существующий логин
				if (_users.Any(u => u.Login == login))
				{
					ShowWarning("LoginExists");
					return;
				}
			
				// Создание нового пользователя
				var newUser = new User(fullName, email, login, password, UserRole.Client);
				_users.Add(newUser);
				SaveUsers();
			
				MessageBox.Show(
					(string)Application.Current.Resources["RegistrationSuccess"],
					(string)Application.Current.Resources["SuccessTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Information);
			
				// После успешной регистрации выполняем вход
				Login = login;
				Password = password;
				OpenMainWindow(newUser);
			}
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
