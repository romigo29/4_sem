using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_FitnessClub.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace WPF_FitnessClub.View
{
    /// <summary>
    /// Логика взаимодействия для AddUserDialog.xaml
    /// </summary>
    public partial class AddUserDialog : Window
    {
        public User NewUser { get; private set; }
        private const int MaxPasswordLength = 30;

        public AddUserDialog()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> validationErrors = new List<string>();
            
            // Проверка ФИО
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                validationErrors.Add((string)Application.Current.Resources["FullNameRequired"]);
                FullNameTextBox.Focus();
            }
            else if (FullNameTextBox.Text.Length < 3)
            {
                validationErrors.Add((string)Application.Current.Resources["FullNameTooShort"]);
                FullNameTextBox.Focus();
            }
            else if (!Regex.IsMatch(FullNameTextBox.Text, @"^[а-яА-Яa-zA-ZёЁ\s]+$"))
            {
                validationErrors.Add((string)Application.Current.Resources["FullNameOnlyLetters"]);
                FullNameTextBox.Focus();
            }
            else
            {
                string[] nameParts = FullNameTextBox.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length != 3)
                {
                    validationErrors.Add((string)Application.Current.Resources["FullNameRequireThreeWords"]);
                    FullNameTextBox.Focus();
                }
            }
            
            // Проверка Email
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                validationErrors.Add((string)Application.Current.Resources["EmailRequired"]);
                if (validationErrors.Count == 1) EmailTextBox.Focus();
            }
            else if (!IsValidEmail(EmailTextBox.Text))
            {
                validationErrors.Add((string)Application.Current.Resources["InvalidEmail"]);
                if (validationErrors.Count == 1) EmailTextBox.Focus();
            }
            
            // Проверка логина
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                validationErrors.Add((string)Application.Current.Resources["UsernameRequired"]);
                if (validationErrors.Count == 1) LoginTextBox.Focus();
            }
            else if (LoginTextBox.Text.Length < 3)
            {
                validationErrors.Add((string)Application.Current.Resources["UsernameTooShort"]);
                if (validationErrors.Count == 1) LoginTextBox.Focus();
            }
            else if (!Regex.IsMatch(LoginTextBox.Text, @"^[a-zA-Z0-9_]{3,20}$"))
            {
                validationErrors.Add((string)Application.Current.Resources["UsernameInvalidFormat"]);
                if (validationErrors.Count == 1) LoginTextBox.Focus();
            }
            
            // Проверка пароля
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                validationErrors.Add((string)Application.Current.Resources["PasswordRequired"]);
                if (validationErrors.Count == 1) PasswordBox.Focus();
            }
            else if (PasswordBox.Password.Length < 6)
            {
                validationErrors.Add((string)Application.Current.Resources["PasswordTooShort"]);
                if (validationErrors.Count == 1) PasswordBox.Focus();
            }
            else if (!Regex.IsMatch(PasswordBox.Password, @"[A-Za-z]"))
            {
                validationErrors.Add((string)Application.Current.Resources["PasswordRequireLetter"]);
                if (validationErrors.Count == 1) PasswordBox.Focus();
            }
            else if (!Regex.IsMatch(PasswordBox.Password, @"\d"))
            {
                validationErrors.Add((string)Application.Current.Resources["PasswordRequireDigit"]);
                if (validationErrors.Count == 1) PasswordBox.Focus();
            }
            
            // Если есть ошибки валидации, показываем сообщение и прерываем выполнение
            if (validationErrors.Count > 0)
            {
                // Создаем строку сообщения с ошибками
                StringBuilder errorMessageBuilder = new StringBuilder();
                errorMessageBuilder.AppendLine((string)Application.Current.Resources["ValidationErrorsHeader"]);
                
                foreach (var error in validationErrors)
                {
                    errorMessageBuilder.AppendLine("- " + error);
                }
                
                string message = errorMessageBuilder.ToString();
                
                MessageBox.Show(
                    message,
                    (string)Application.Current.Resources["ValidationErrorTitle"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Определение выбранной роли
            UserRole selectedRole = UserRole.Client;
            if (RoleComboBox.SelectedItem is ComboBoxItem selectedItem && 
                int.TryParse(selectedItem.Tag?.ToString(), out int roleValue))
            {
                selectedRole = (UserRole)roleValue;
            }

            // Создание нового пользователя
            NewUser = new User(
                FullNameTextBox.Text.Trim(),
                EmailTextBox.Text.Trim(),
                LoginTextBox.Text.Trim(),
                PasswordBox.Password,
                selectedRole)
            {
                IsBlocked = IsBlockedCheckBox.IsChecked ?? false
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
                
            try 
            {
                // Проверка на формат email
                if (!email.Contains("@"))
                {
                    return false;
                }

                string[] parts = email.Split('@');
                if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                {
                    return false;
                }

                string localPart = parts[0];
                string domainPart = parts[1];
                
                // Проверяем локальную часть (до @)
                if (!Regex.IsMatch(localPart, @"^[a-zA-Z0-9._\-]+$"))
                {
                    return false;
                }
                
                // Проверяем, что в локальной части нет двух точек подряд
                if (localPart.Contains(".."))
                {
                    return false;
                }
                
                // Проверяем, что домен содержит хотя бы одну точку
                if (!domainPart.Contains("."))
                {
                    return false;
                }
                
                // Проверяем, что в домене нет двух точек подряд
                if (domainPart.Contains(".."))
                {
                    return false;
                }
                
                // Проверяем, что домен не начинается и не заканчивается точкой
                if (domainPart.StartsWith(".") || domainPart.EndsWith("."))
                {
                    return false;
                }
                
                // Проверяем, что домен состоит только из разрешенных символов (буквы, цифры, точки, дефисы)
                if (!Regex.IsMatch(domainPart, @"^[a-zA-Z0-9\.\-]+$"))
                {
                    return false;
                }

                return true;
            }
            catch 
            {
                return false;
            }
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                if (passwordBox.Password.Length >= MaxPasswordLength && 
                    e.Key != Key.Back && e.Key != Key.Delete && 
                    e.Key != Key.Left && e.Key != Key.Right && 
                    e.Key != Key.Tab && e.Key != Key.Home && 
                    e.Key != Key.End && !Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    e.Handled = true;
                }
            }
        }
    }
} 