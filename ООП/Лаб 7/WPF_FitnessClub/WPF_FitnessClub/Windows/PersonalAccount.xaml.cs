using System;
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
using System.ComponentModel;
using static WPF_FitnessClub.Commands;
using static WPF_FitnessClub.LanguageManager;
using WPF_FitnessClub.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Text.RegularExpressions;

namespace WPF_FitnessClub.Windows
{
	/// <summary>
	/// Логика взаимодействия для PersonalAccount.xaml
	/// </summary>
	/// 
	
	public partial class PersonalAccount : Window, INotifyPropertyChanged
	{
		private bool _isEditMode;
		User _user;
		private string _originalUsername;
		private string _originalEmail;
		private string _originalPassword;
		private int _selectedTabIndex;
		List<User> _users;
		string usersFilePath = "users.js";

		public event EventHandler<string> LanguageChanged;

		public int SelectedTabIndex
		{
			get => _selectedTabIndex;
			set
			{
				_selectedTabIndex = value;
				OnPropertyChanged(nameof(SelectedTabIndex));
			}
		}

		public Visibility ViewModeVisible => IsEditMode ? Visibility.Collapsed : Visibility.Visible;
		public Visibility EditModeVisible => IsEditMode ? Visibility.Visible : Visibility.Collapsed;

		public ICommand EditCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }
		public ICommand ChangePasswordCommand { get; private set; }

		public bool IsEditMode
		{
			get => _isEditMode;
			set
			{
				_isEditMode = value;
				OnPropertyChanged(nameof(IsEditMode));
				OnPropertyChanged(nameof(ViewModeVisible));
				OnPropertyChanged(nameof(EditModeVisible));
				if (value)
				{
					// Инициализация может происходить при необходимости
					// Обновляем команду Save
					(SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
				}
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

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public PersonalAccount(User user)
		{
			InitializeComponent();

			_user = user;
			_originalUsername = user.Login;
			_originalEmail = user.Email;
			_originalPassword = user.Password;
			SelectedTabIndex = 0;

			_users = LoadUsers(usersFilePath);

			EditCommand = new RelayCommand(ExecuteEdit);
			SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
			CancelCommand = new RelayCommand(ExecuteCancel);
			CloseCommand = new RelayCommand(ExecuteClose);
			ChangePasswordCommand = new RelayCommand(ExecuteChangePassword, CanExecuteChangePassword);

			DataContext = this;
			IsEditMode = false;
            
            // Инициализируем переключатели темы в соответствии с текущей темой
            InitializeThemeControls();
            
            // Подписываемся на изменение темы
            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
		}
        
        private void InitializeThemeControls()
        {
 
            switch (ThemeManager.Instance.CurrentTheme)
            {
                case ThemeManager.AppTheme.Light:
                    LightThemeRadioButton.IsChecked = true;
                    break;
                case ThemeManager.AppTheme.Dark:
                    DarkThemeRadioButton.IsChecked = true;
                    break;
            }
        }
        
        private void LightThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded && LightThemeRadioButton.IsChecked == true)
            {
                ThemeManager.Instance.ChangeTheme(ThemeManager.AppTheme.Light);
            }
        }

        private void DarkThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded && DarkThemeRadioButton.IsChecked == true)
            {
                ThemeManager.Instance.ChangeTheme(ThemeManager.AppTheme.Dark);
            }
        }

		private void ExecuteEdit(object parameter)
		{
			IsEditMode = true;
		}

		private bool CanExecuteSave(object parameter)
		{
			if (!IsEditMode) return false;

			if (string.IsNullOrWhiteSpace(EditUsernameInput.Text) || 
				string.IsNullOrWhiteSpace(EditEmailInput.Text))
				return false;

			if (!IsValidEmail(EditEmailInput.Text))
				return false;

			return true;
		}

		private void ExecuteSave(object parameter)
		{
			FullName = FullNameViewInput.Text;
			Username = EditUsernameInput.Text;
			Email = EditEmailInput.Text;

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
			if (IsEditMode)
			{
				var result = MessageBox.Show(
					Application.Current.Resources["UnsavedChangesMessage"].ToString(),
					Application.Current.Resources["Warning"].ToString(),
					MessageBoxButton.YesNoCancel);

				switch (result)
				{
					case MessageBoxResult.Yes:
						if (CanExecuteSave(null))
						{
							ExecuteSave(null);
							Close();
						}
						break;
					case MessageBoxResult.No:
						Close();
						break;
					case MessageBoxResult.Cancel:
						return;
				}
			}
			else
			{
				Close();
			}
		}

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

		private void LanguageSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem selectedItem = LanguageSelectionComboBox.SelectedItem as ComboBoxItem;
			string languageCode = selectedItem.Tag.ToString();
			LanguageManager.Instance.ChangeLanguage(languageCode);
			LanguageChanged?.Invoke(this, languageCode);
		}

		private void OnThemeChanged(object sender, ThemeManager.AppTheme e)
		{
			// При изменении темы принудительно обновляем стили и элементы интерфейса
			if (IsLoaded)
			{
				// Принудительное обновление стилей TabControl
				var currentIndex = SelectedTabIndex;
				SelectedTabIndex = -1;
				SelectedTabIndex = currentIndex;
			}
		}
		
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			ThemeManager.Instance.ThemeChanged -= OnThemeChanged;
		}

		public List<User> LoadUsers(string filePath)
		{
			if (System.IO.File.Exists(filePath))
			{
				try
				{
					string json = System.IO.File.ReadAllText(filePath);
					return Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
				}
				catch (Exception ex)
				{
					MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorLoadingData"], ex.Message));
					return new List<User>();
				}
			}
			else
			{
				return new List<User>();
			}
		}

		public void SaveUsers()
		{
			try
			{
				string json = Newtonsoft.Json.JsonConvert.SerializeObject(_users, Newtonsoft.Json.Formatting.Indented);
				System.IO.File.WriteAllText(usersFilePath, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorSavingData"], ex.Message));
			}
		}

		private void UpdateUserInList()
		{
			var userIndex = _users.FindIndex(u => u.Login == _originalUsername);
			if (userIndex != -1)
			{
				_users[userIndex] = _user;
			}
			else
			{
				// Если пользователь не найден (что маловероятно), добавляем его
				_users.Add(_user);
			}
		}

		private bool CanExecuteChangePassword(object parameter)
		{
			if (string.IsNullOrWhiteSpace(SecurityCurrentPasswordInput.Text) ||
				string.IsNullOrWhiteSpace(SecurityNewPasswordInput.Password) ||
				string.IsNullOrWhiteSpace(SecurityConfirmPasswordInput.Password))
				return false;

			if (SecurityCurrentPasswordInput.Text == SecurityNewPasswordInput.Password)
				return false;

			if (SecurityNewPasswordInput.Password != SecurityConfirmPasswordInput.Password)
				return false;

			if (SecurityNewPasswordInput.Password.Length < 6)
				return false;

			return true;
		}

		private void ExecuteChangePassword(object parameter)
		{
			if (SecurityCurrentPasswordInput.Text != _originalPassword)
			{
				MessageBox.Show(
					Application.Current.Resources["InvalidCurrentPassword"].ToString(),
					Application.Current.Resources["Error"].ToString(),
					MessageBoxButton.OK,
					MessageBoxImage.Error);
				return;
			}

			try
			{
				Password = SecurityNewPasswordInput.Password;
				_originalPassword = Password;

				UpdateUserInList();
				SaveUsers();

				MessageBox.Show(
					Application.Current.Resources["PasswordChangedSuccess"].ToString(),
					Application.Current.Resources["Success"].ToString(),
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				ClearPasswordFields();
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"{Application.Current.Resources["PasswordChangeFailed"]}: {ex.Message}",
					Application.Current.Resources["Error"].ToString(),
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}

		private void ClearPasswordFields_Click(object sender, RoutedEventArgs e)
		{
			ClearPasswordFields();
		}

		private void ClearPasswordFields()
		{
			SecurityNewPasswordInput.Clear();
			SecurityConfirmPasswordInput.Clear();
		}

	}
}
