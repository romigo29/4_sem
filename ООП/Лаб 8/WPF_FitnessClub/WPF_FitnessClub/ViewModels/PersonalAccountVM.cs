using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.IO;
using WPF_FitnessClub.Models;
using Newtonsoft.Json;
using static WPF_FitnessClub.Commands;


namespace WPF_FitnessClub.ViewModels
{
	public class PersonalAccountVM : ViewModelBase
	{
		#region Конструктор и инициализация
		public PersonalAccountVM(User user)
		{
			// Команды
			EditCommand = new RelayCommand(ExecuteEdit);
			SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
			CancelCommand = new RelayCommand(ExecuteCancel);
			CloseCommand = new RelayCommand(ExecuteClose);
			ChangePasswordCommand = new RelayCommand(ExecuteChangePassword, CanExecuteChangePassword);
			ApplyLanguageCommand = new RelayCommand(ExecuteApplyLanguage);
			ApplyThemeCommand = new RelayCommand(ExecuteApplyTheme);

			_user = user;
			_originalUsername = user.Login;
			_originalEmail = user.Email;
			_originalPassword = user.Password;
			SelectedTabIndex = 0;

			_users = LoadUsers(usersFilePath);

			// Начальный режим - просмотр
			IsEditMode = false;

			// Темы и языки
			CurrentTheme = ThemeManager.Instance.CurrentTheme == ThemeManager.AppTheme.Light 
				? 0 : 1;

			// Подписка на изменение темы
			ThemeManager.Instance.ThemeChanged += OnThemeChanged;
		}
		#endregion

		#region Приватные поля
		private bool _isEditMode;
		private readonly User _user;
		private string _originalUsername;
		private string _originalEmail;
		private string _originalPassword;
		private int _selectedTabIndex;
		private List<User> _users;
		private string usersFilePath = "users.js";
		private string _currentPassword;
		private string _newPassword;
		private string _confirmPassword;
		private int _currentTheme;
		private int _currentLanguage;
		#endregion

		#region События
		public event EventHandler<string> LanguageChanged;
		public event EventHandler RequestClose;
		#endregion

		#region Свойства для привязки Visibility
		public Visibility ViewModeVisible => IsEditMode ? Visibility.Collapsed : Visibility.Visible;
		public Visibility EditModeVisible => IsEditMode ? Visibility.Visible : Visibility.Collapsed;
		#endregion

		#region Команды
		public ICommand EditCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }
		public ICommand ChangePasswordCommand { get; private set; }
		public ICommand ApplyLanguageCommand { get; private set; }
		public ICommand ApplyThemeCommand { get; private set; }
		#endregion

		#region Свойства для привязки данных
		public bool IsEditMode
		{
			get => _isEditMode;
			set
			{
				_isEditMode = value;
				OnPropertyChanged(nameof(IsEditMode));
				OnPropertyChanged(nameof(ViewModeVisible));
				OnPropertyChanged(nameof(EditModeVisible));
			}
		}

		public string Username
		{
			get => _user.Login;
			set
			{
				if (_user.Login != value)
				{
					_user.Login = value;
					OnPropertyChanged(nameof(Username));
				}
			}
		}

		public string Email
		{
			get => _user.Email;
			set
			{
				if (_user.Email != value)
				{
					_user.Email = value;
					OnPropertyChanged(nameof(Email));
				}
			}
		}

		public string FullName
		{
			get => _user.FullName;
			set
			{
				if (_user.FullName != value)
				{
					_user.FullName = value;
					OnPropertyChanged(nameof(FullName));
				}
			}
		}

		public string Password
		{
			get => _user.Password;
			set
			{
				if (_user.Password != value)
				{
					_user.Password = value;
					OnPropertyChanged(nameof(Password));
				}
			}
		}

		public string CurrentPassword
		{
			get => _currentPassword;
			set
			{
				_currentPassword = value;
				OnPropertyChanged(nameof(CurrentPassword));
			}
		}

		public string NewPassword
		{
			get => _newPassword;
			set
			{
				_newPassword = value;
				OnPropertyChanged(nameof(NewPassword));
			}
		}

		public string ConfirmPassword
		{
			get => _confirmPassword;
			set
			{
				_confirmPassword = value;
				OnPropertyChanged(nameof(ConfirmPassword));
			}
		}

		public int SelectedTabIndex
		{
			get => _selectedTabIndex;
			set
			{
				_selectedTabIndex = value;
				OnPropertyChanged(nameof(SelectedTabIndex));
			}
		}

		public int CurrentTheme
		{
			get => _currentTheme;
			set
			{
				_currentTheme = value;
				OnPropertyChanged(nameof(CurrentTheme));
			}
		}

		public int CurrentLanguage
		{
			get => _currentLanguage;
			set
			{
				_currentLanguage = value;
				OnPropertyChanged(nameof(CurrentLanguage));
			}
		}

		#endregion

		#region Методы команд

		private void ExecuteEdit(object parameter)
		{
			IsEditMode = true;
		}

		private bool CanExecuteSave(object parameter)
		{
			if (!IsEditMode) return false;

			if (string.IsNullOrWhiteSpace(Username) ||
				string.IsNullOrWhiteSpace(Email) ||
				string.IsNullOrWhiteSpace(FullName))
				return false;

			if (!IsValidEmail(Email))
				return false;

			return true;
		}

		private void ExecuteSave(object parameter)
		{
			if (!IsValid(FullName, @"^([A-Za-zА-Яа-яЁё]+(-[A-Za-zА-Яа-яЁё]+)?\s){2}[A-Za-zА-Яа-яЁё]+(-[A-Za-zА-Яа-яЁё]+)?$", "InvalidFullName")) return;
			if (!IsValid(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "InvalidEmail")) return;
			if (!IsValid(Username, @"^[a-zA-ZА-Яа-я0-9]+$", "InvalidLogin")) return;

			UpdateUserInList();
			SaveUsers();

			_originalUsername = Username;
			_originalEmail = Email;

			MessageBox.Show(Application.Current.Resources["SaveSuccessful"].ToString());
			IsEditMode = false;
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

		private void ExecuteCancel(object parameter)
		{
			Username = _originalUsername;
			Email = _originalEmail;
			IsEditMode = false;
		}

		private void ExecuteClose(object parameter)
		{
			RequestClose?.Invoke(this, EventArgs.Empty);
		}

		private bool CanExecuteChangePassword(object parameter)
		{
			if (string.IsNullOrEmpty(CurrentPassword) ||
				string.IsNullOrEmpty(NewPassword) ||
				string.IsNullOrEmpty(ConfirmPassword))
				return false;

			if (CurrentPassword != _user.Password)
				return false;

			if (NewPassword != ConfirmPassword)
				return false;

			return true;
		}

		private void ExecuteChangePassword(object parameter)
		{
			if (CurrentPassword != _user.Password)
			{
				ShowWarning("IncorrectPassword");
				return;
			}

			if (NewPassword != ConfirmPassword)
			{
				ShowWarning("PasswordMismatch");
				return;
			}

			if (NewPassword.Length < 6)
			{
				ShowWarning("PasswordTooShort");
				return;
			}

			_user.Password = NewPassword;
			_originalPassword = NewPassword;
			UpdateUserInList();
			SaveUsers();

			MessageBox.Show(
				(string)Application.Current.Resources["PasswordChangedSuccess"],
				(string)Application.Current.Resources["SuccessTitle"],
				MessageBoxButton.OK,
				MessageBoxImage.Information);

			ClearPasswordFields();
		}

		private void ExecuteApplyLanguage(object parameter)
		{
			string language = CurrentLanguage == 0 ? "ru-RU" : "en-US";
			LanguageManager.Instance.ChangeLanguage(language);
			LanguageChanged?.Invoke(this, language);
		}

		private void ExecuteApplyTheme(object parameter)
		{
			ThemeManager.AppTheme theme = CurrentTheme == 0 
				? ThemeManager.AppTheme.Light 
				: ThemeManager.AppTheme.Dark;
			ThemeManager.Instance.ChangeTheme(theme);
		}
		#endregion

		#region Вспомогательные методы
		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		private void OnThemeChanged(object sender, ThemeManager.AppTheme theme)
		{
			CurrentTheme = theme == ThemeManager.AppTheme.Light ? 0 : 1;
		}

		private void ClearPasswordFields()
		{
			CurrentPassword = string.Empty;
			NewPassword = string.Empty;
			ConfirmPassword = string.Empty;
		}

		public List<User> LoadUsers(string filePath)
		{
			try
			{
				if (File.Exists(filePath))
				{
					string json = File.ReadAllText(filePath);
					return JsonConvert.DeserializeObject<List<User>>(json);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}");
			}
			return new List<User>();
		}

		public void SaveUsers()
		{
			try
			{
				string json = JsonConvert.SerializeObject(_users);
				File.WriteAllText(usersFilePath, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при сохранении пользователей: {ex.Message}");
			}
		}

		private void UpdateUserInList()
		{
			var existingUser = _users.Find(u => u.Login == _originalUsername);
			if (existingUser != null)
			{
				int index = _users.IndexOf(existingUser);
				_users[index] = _user;
			}
		}
		#endregion
	}
}
