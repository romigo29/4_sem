using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_FitnessClub.ViewModels;
using WPF_FitnessClub.Models;

namespace WPF_FitnessClub.View
{
    /// <summary>
    /// Логика взаимодействия для AdminPanelView.xaml
    /// </summary>
    public partial class AdminPanelView : UserControl
    {
        private AdminPanelViewModel viewModel;
        private bool isViewModelInitialized = false;

        public AdminPanelView()
        {
            InitializeComponent();
            
            // Инициализируем ViewModel сразу при создании представления
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                viewModel = new AdminPanelViewModel();
                DataContext = viewModel;
                isViewModelInitialized = true;
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelInitError"]}: {ex.Message}", 
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                Mouse.OverrideCursor = null;
            }
            
            Loaded += AdminPanelView_Loaded;
            Unloaded += AdminPanelView_Unloaded;
        }

        private void AdminPanelView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Если ViewModel не была инициализирована, делаем это сейчас
                if (!isViewModelInitialized)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    viewModel = new AdminPanelViewModel();
                    DataContext = viewModel;
                    isViewModelInitialized = true;
                    Mouse.OverrideCursor = null;
                }
                else
                {
                    // Если ViewModel уже инициализирована, просто обновляем данные
                    Mouse.OverrideCursor = Cursors.Wait;
                    viewModel.RefreshCommand.Execute(null);
                    Mouse.OverrideCursor = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelLoadError"]}: {ex.Message}", 
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                Mouse.OverrideCursor = null;
            }
        }

        private void AdminPanelView_Unloaded(object sender, RoutedEventArgs e)
        {
            // Освобождаем ресурсы при выгрузке контрола
            if (viewModel != null)
            {
                viewModel = null;
                isViewModelInitialized = false;
            }
        }
        
        /// <summary>
        /// Обработчик изменения выбранной роли в ComboBox
        /// </summary>
        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is User user)
            {
                try
                {
                    // Получаем выбранный элемент
                    if (comboBox.SelectedItem is ComboBoxItem selectedItem && 
                        int.TryParse(selectedItem.Tag?.ToString(), out int roleValue))
                    {
                        // Преобразуем значение тега в UserRole
                        UserRole selectedRole = (UserRole)roleValue;
                        
                        // Запоминаем текущую роль
                        UserRole oldRole = user.Role;
                        
                        // Если роль не изменилась, ничего не делаем
                        if (oldRole == selectedRole)
                            return;
                            
                        // Проверяем ограничения для главного администратора
                        if (user.Id == 1 && selectedRole != UserRole.Admin)
                        {
                            MessageBox.Show((string)Application.Current.Resources["AdminPanelDeleteAdminError"],
                                (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                                
                            // Отменяем обработчик событий
                            comboBox.SelectionChanged -= RoleComboBox_SelectionChanged;
                            
                            // Восстанавливаем выбранный элемент
                            foreach (ComboBoxItem item in comboBox.Items)
                            {
                                if (int.TryParse(item.Tag?.ToString(), out int tag) && tag == (int)oldRole)
                                {
                                    comboBox.SelectedItem = item;
                                    break;
                                }
                            }
                            
                            // Восстанавливаем обработчик событий
                            comboBox.SelectionChanged += RoleComboBox_SelectionChanged;
                            return;
                        }
                        
                        // Проверяем, не снимает ли администратор роль с самого себя
                        User currentUser = viewModel.GetCurrentUser();
                        if (currentUser != null && currentUser.Id == user.Id && selectedRole != UserRole.Admin && oldRole == UserRole.Admin)
                        {
                            MessageBox.Show((string)Application.Current.Resources["AdminPanelBlockSelfError"],
                                (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                                
                            // Отменяем обработчик событий
                            comboBox.SelectionChanged -= RoleComboBox_SelectionChanged;
                            
                            // Восстанавливаем выбранный элемент
                            foreach (ComboBoxItem item in comboBox.Items)
                            {
                                if (int.TryParse(item.Tag?.ToString(), out int tag) && tag == (int)oldRole)
                                {
                                    comboBox.SelectedItem = item;
                                    break;
                                }
                            }
                            
                            // Восстанавливаем обработчик событий
                            comboBox.SelectionChanged += RoleComboBox_SelectionChanged;
                            return;
                        }
                        
                        // Устанавливаем новую роль
                        user.Role = selectedRole;
                        
                        // Передаем обработку изменения роли во ViewModel
                        Mouse.OverrideCursor = Cursors.Wait;
                        bool success = viewModel.SaveUserChanges(user);
                        Mouse.OverrideCursor = null;
                        
                        // Если изменение роли не удалось, возвращаем предыдущее значение
                        if (!success)
                        {
                            // Отменяем обработчик событий
                            comboBox.SelectionChanged -= RoleComboBox_SelectionChanged;
                            
                            // Восстанавливаем предыдущую роль
                            user.Role = oldRole;
                            
                            // Восстанавливаем выбранный элемент
                            foreach (ComboBoxItem item in comboBox.Items)
                            {
                                if (int.TryParse(item.Tag?.ToString(), out int tag) && tag == (int)oldRole)
                                {
                                    comboBox.SelectedItem = item;
                                    break;
                                }
                            }
                            
                            // Восстанавливаем обработчик событий
                            comboBox.SelectionChanged += RoleComboBox_SelectionChanged;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{(string)Application.Current.Resources["RoleSelectionError"]}: {ex.Message}", 
                        (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                    Mouse.OverrideCursor = null;
                }
            }
        }

        /// <summary>
        /// Обработчик загрузки ComboBox для роли
        /// </summary>
        private void RoleComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is User user)
            {
                // Установка выбранного значения на основе роли пользователя
                int roleValue = (int)user.Role;
                
                // Перебираем элементы ComboBox и выбираем соответствующий
                foreach (ComboBoxItem item in comboBox.Items)
                {
                    if (int.TryParse(item.Tag?.ToString(), out int tag) && tag == roleValue)
                    {
                        // Отключаем обработчик событий на время установки значения
                        comboBox.SelectionChanged -= RoleComboBox_SelectionChanged;
                        
                        comboBox.SelectedItem = item;
                        
                        // Восстанавливаем обработчик
                        comboBox.SelectionChanged += RoleComboBox_SelectionChanged;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик окончания редактирования ячейки в DataGrid
        /// </summary>
        private void UsersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Получаем редактируемого пользователя
                if (e.Row.Item is User user)
                {
                    try
                    {
                        // Получаем новое значение из редактора
                        string newValue = string.Empty;
                        bool? isBlocked = null;
                        
                        if (e.EditingElement is TextBox textBox)
                        {
                            newValue = textBox.Text;
                        }
                        else if (e.EditingElement is CheckBox checkBox)
                        {
                            isBlocked = checkBox.IsChecked;
                        }
                        
                        // Определяем, какое поле было изменено
                        string columnHeader = e.Column.Header.ToString();
                        
                        // Проверяем валидность данных перед сохранением
                        bool isValid = true;
                        string errorMessage = string.Empty;
                        
                        switch (columnHeader)
                        {
                            case "ФИО":
                                if (string.IsNullOrWhiteSpace(newValue))
                                {
                                    isValid = false;
                                    errorMessage = (string)Application.Current.Resources["AddUserValidationEmptyFullName"];
                                }
                                else
                                {
                                    user.FullName = newValue;
                                }
                                break;
                                
                            case "Email":
                                if (string.IsNullOrWhiteSpace(newValue) || !IsValidEmail(newValue))
                                {
                                    isValid = false;
                                    errorMessage = (string)Application.Current.Resources["AddUserValidationInvalidEmail"];
                                }
                                else if (newValue != user.Email && !viewModel.IsEmailUnique(newValue))
                                {
                                    isValid = false;
                                    errorMessage = (string)Application.Current.Resources["AddUserValidationEmailExists"];
                                }
                                else
                                {
                                    user.Email = newValue;
                                }
                                break;
                                
                            case "Логин":
                                if (string.IsNullOrWhiteSpace(newValue) || newValue.Length < 3)
                                {
                                    isValid = false;
                                    errorMessage = (string)Application.Current.Resources["AddUserValidationShortLogin"];
                                }
                                else if (newValue != user.Login && !viewModel.IsLoginUnique(newValue))
                                {
                                    isValid = false;
                                    errorMessage = (string)Application.Current.Resources["AddUserValidationLoginExists"];
                                }
                                else
                                {
                                    user.Login = newValue;
                                }
                                break;
                                
                            case "Заблокирован":
                                // Проверяем, не пытаемся ли заблокировать главного администратора
                                if (user.Id == 1 && isBlocked == true)
                                {
                                    isValid = false;
                                    errorMessage = (string)Application.Current.Resources["AdminPanelBlockAdminError"];
                                }
                                
                                // Проверяем, не блокирует ли администратор сам себя
                                User currentUser = viewModel.GetCurrentUser();
                                if (currentUser != null && currentUser.Id == user.Id && isBlocked == true)
                                {
                                    isValid = false;
                                    errorMessage = (string)Application.Current.Resources["AdminPanelBlockSelfError"];
                                }
                                
                                // Если пользователь пытается изменить статус блокировки через DataGrid,
                                // то применяем изменения напрямую
                                if (isValid && isBlocked.HasValue)
                                {
                                    user.IsBlocked = isBlocked.Value;
                                }
                                break;
                        }
                        
                        // Если данные невалидны, отменяем изменение и показываем сообщение
                        if (!isValid)
                        {
                            MessageBox.Show(errorMessage, (string)Application.Current.Resources["AdminPanelWarning"], MessageBoxButton.OK, MessageBoxImage.Warning);
                            e.Cancel = true;
                            return;
                        }
                        
                        // Сохраняем изменения в базе данных, если это не редактирование роли
                        // (роль обрабатывается отдельно в RoleComboBox_SelectionChanged)
                        if (columnHeader != "Роль")
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            bool success = viewModel.SaveUserChanges(user);
                            Mouse.OverrideCursor = null;
                            
                            // Если сохранение не удалось, сообщаем об этом
                            if (!success)
                            {
                                MessageBox.Show(string.Format((string)Application.Current.Resources["AdminPanelUpdateUserError"], user.Login), 
                                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                                e.Cancel = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{(string)Application.Current.Resources["CellEditError"]}: {ex.Message}", 
                            (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                        Mouse.OverrideCursor = null;
                    }
                }
            }
        }
        
        /// <summary>
        /// Проверка корректности email
        /// </summary>
        private bool IsValidEmail(string email)
        {
            // Простая проверка формата email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }
    }
} 