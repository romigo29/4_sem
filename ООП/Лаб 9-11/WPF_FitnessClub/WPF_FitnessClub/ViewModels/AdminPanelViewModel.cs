using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static WPF_FitnessClub.Commands;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.Data;
using WPF_FitnessClub.Data.Services;
using System.IO;
using System.Text;

namespace WPF_FitnessClub.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly DatabaseBackupService _backupService;
        private bool _isLoading;
        private ObservableCollection<User> _usersTable;
        private User _selectedUser;

        public AdminPanelViewModel()
        {
            try
            {
                _isLoading = true;
                OnPropertyChanged(nameof(IsLoading));

                _context = new AppDbContext();
                _userService = new UserService();
                _backupService = new DatabaseBackupService();

                RefreshCommand = new RelayCommand(ExecuteRefreshCommand);
                BlockUserCommand = new RelayCommand(ExecuteBlockUserCommand, (p) => p is User);
                DeleteUserCommand = new RelayCommand(ExecuteDeleteUserCommand, (p) => p is User);
                AddUserCommand = new RelayCommand(ExecuteAddUserCommand);

                LoadUsersData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelInitError"]}: {ex.Message}",
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _isLoading = false;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        #region Свойства

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public ObservableCollection<User> UsersTable
        {
            get => _usersTable;
            set
            {
                if (_usersTable != value)
                {
                    _usersTable = value;
                    OnPropertyChanged(nameof(UsersTable));
                }
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        #endregion

        #region Команды

        public ICommand RefreshCommand { get; private set; }
        public ICommand BlockUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand AddUserCommand { get; private set; }
        public ICommand ExportToJsonCommand { get; private set; }

        #endregion

        #region Методы команд

        private void ExecuteRefreshCommand(object parameter)
        {
            try
            {
                IsLoading = true;
                LoadUsersData();
                MessageBox.Show((string)Application.Current.Resources["AdminPanelDataUpdated"], 
                    (string)Application.Current.Resources["AdminPanelSuccess"], MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelLoadUsersError"]}: {ex.Message}",
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ExecuteBlockUserCommand(object parameter)
        {
            try
            {
                if (parameter is User user)
                {
                    // Проверка на главного администратора (ID=1)
                    if (user.Id == 1)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelBlockAdminError"],
                            (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    // Проверка на текущего пользователя
                    User currentUser = _userService.GetCurrentUser();
                    if (currentUser != null && currentUser.Id == user.Id)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelBlockSelfError"],
                            (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    // Получаем свежую версию пользователя из базы данных
                    var freshUser = _userService.GetById(user.Id);
                    if (freshUser == null)
                    {
                        MessageBox.Show(string.Format((string)Application.Current.Resources["AdminPanelUserNotFound"], user.Id),
                            (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    // Инвертируем состояние блокировки
                    freshUser.IsBlocked = !freshUser.IsBlocked;

                    // Обновляем пользователя в БД
                    bool success = _userService.Update(freshUser);
                    
                    if (success)
                    {
                        // Обновляем статус в модели представления
                        user.IsBlocked = freshUser.IsBlocked;
                        
                        string message = freshUser.IsBlocked
                            ? (string)Application.Current.Resources["AdminPanelUserBlocked"]
                            : (string)Application.Current.Resources["AdminPanelUserUnblocked"];

                        MessageBox.Show(message, (string)Application.Current.Resources["AdminPanelSuccess"], MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // Обновляем данные
                        LoadUsersData();
                    }
                    else
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelUpdateUserError"],
                            (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelBlockUserError"]}: {ex.Message}",
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteUserCommand(object parameter)
        {
            try
            {
                if (parameter is User user)
                {
                    // Проверка на главного администратора (ID=1)
                    if (user.Id == 1)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelDeleteAdminError"],
                            (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    // Проверка на текущего пользователя
                    User currentUser = _userService.GetCurrentUser();
                    if (currentUser != null && currentUser.Id == user.Id)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelBlockSelfError"],
                            (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    // Запрашиваем подтверждение удаления
                    var result = MessageBox.Show(string.Format((string)Application.Current.Resources["AdminPanelConfirmDeleteUser"], user.Login),
                        (string)Application.Current.Resources["DeleteConfirmTitle"], MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        IsLoading = true;
                        // Получаем свежую версию пользователя из базы данных
                        var freshUser = _userService.GetById(user.Id);
                        if (freshUser == null)
                        {
                            MessageBox.Show(string.Format((string)Application.Current.Resources["AdminPanelUserNotFound"], user.Id),
                                (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                            IsLoading = false;
                            return;
                        }
                        
                        // Удаляем пользователя
                        bool success = _userService.Delete(freshUser.Id);
                        
                        if (success)
                        {
                            MessageBox.Show((string)Application.Current.Resources["AdminPanelUserDeleted"],
                                (string)Application.Current.Resources["AdminPanelSuccess"], MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            // Обновляем данные
                            LoadUsersData();
                        }
                        else
                        {
                            MessageBox.Show((string)Application.Current.Resources["AdminPanelDeleteUserError"],
                                (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                            
                            // Автоматически обновляем данные для очистки отслеживания объектов
                            LoadUsersData();
                        }
                        IsLoading = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelDeleteUserError"]}: {ex.Message}",
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                
                // В случае ошибки все равно обновляем данные
                LoadUsersData();
            }
        }

        private void ExecuteAddUserCommand(object parameter)
        {
            try
            {
                // Создаем и отображаем диалоговое окно
                var addUserDialog = new View.AddUserDialog();
                
                // Получаем текущее окно приложения для установки владельца диалога
                var mainWindow = Application.Current.MainWindow;
                addUserDialog.Owner = mainWindow;
                
                // Отображаем диалог и проверяем результат
                bool? result = addUserDialog.ShowDialog();
                
                if (result == true && addUserDialog.NewUser != null)
                {
                    IsLoading = true;
                    
                    // Список ошибок валидации
                    List<string> validationErrors = new List<string>();
                    
                    // Проверяем уникальность логина и email
                    if (!_userService.IsLoginUnique(addUserDialog.NewUser.Login))
                    {
                        validationErrors.Add((string)Application.Current.Resources["LoginAlreadyTaken"]);
                    }
                    
                    if (!_userService.IsEmailUnique(addUserDialog.NewUser.Email))
                    {
                        validationErrors.Add((string)Application.Current.Resources["EmailAlreadyExists"]);
                    }
                    
                    // Если есть ошибки валидации, показываем их и прерываем выполнение
                    if (validationErrors.Count > 0)
                    {
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
                        IsLoading = false;
                        return;
                    }
                    
                    // Добавляем пользователя в базу данных
                    int userId = _userService.Add(addUserDialog.NewUser);
                    
                    if (userId > 0)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelUserAdded"],
                            (string)Application.Current.Resources["AdminPanelSuccess"], MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // Полностью обновляем список пользователей, чтобы избежать проблем с отслеживанием сущностей
                        // Детач всех отслеживаемых записей перед загрузкой новых
                        _context.ChangeTracker.Entries()
                            .Where(e => e.State != EntityState.Detached)
                            .ToList()
                            .ForEach(e => e.State = EntityState.Detached);
                        
                        LoadUsersData();
                    }
                    else
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelAddUserError"],
                            (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelAddUserError"]}: {ex.Message}",
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Вспомогательные методы

        private void LoadUsersData()
        {
            try
            {
                // Загружаем список пользователей
                var users = _userService.GetAll();
                
                if (users == null || users.Count == 0)
                {
                    MessageBox.Show((string)Application.Current.Resources["AdminPanelNoUsersFound"],
                        (string)Application.Current.Resources["AdminPanelWarning"], MessageBoxButton.OK, MessageBoxImage.Warning);
                    UsersTable = new ObservableCollection<User>();
                    return;
                }
                
                // Проверяем каждого пользователя на корректность данных
                foreach (var user in users)
                {
                    if (user == null)
                    {
                        continue;
                    }
                    
                    // Проверяем, что роль имеет правильное значение
                    if (!Enum.IsDefined(typeof(UserRole), user.Role))
                    {
                        user.Role = UserRole.Client;
                    }
                }
                
                // Создаем новую коллекцию и присваиваем ее свойству UsersTable
                UsersTable = new ObservableCollection<User>(users);
                
                // Дополнительно вызываем событие изменения свойства
                OnPropertyChanged(nameof(UsersTable));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelLoadUsersError"]}: {ex.Message}",
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                UsersTable = new ObservableCollection<User>();
            }
        }


        public bool IsLoginUnique(string login)
        {
            return _userService.IsLoginUnique(login);
        }


        public bool IsEmailUnique(string email)
        {
            return _userService.IsEmailUnique(email);
        }

        public bool SaveUserChanges(User user)
        {
            try
            {
                // Очищаем контекст перед операцией обновления
                _context.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Detached)
                    .ToList()
                    .ForEach(e => e.State = EntityState.Detached);
                
                // Получаем свежую версию пользователя из базы данных
                var freshUser = _userService.GetById(user.Id);
                if (freshUser == null)
                {
                    MessageBox.Show(string.Format((string)Application.Current.Resources["AdminPanelUserNotFound"], user.Id),
                        (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                
                // Проверяем, не пытаемся ли мы изменить главного администратора
                if (user.Id == 1)
                {
                    // Для главного администратора разрешаем изменять только ФИО и email
                    if (freshUser.Login != user.Login)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelDeleteAdminError"],
                            (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                        
                        // Восстанавливаем оригинальный логин
                        user.Login = freshUser.Login;
                    }
                    
                    // Запрещаем изменять роль и статус блокировки
                    if (freshUser.Role != user.Role)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelDeleteAdminError"],
                            (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                        user.Role = freshUser.Role;
                    }
                    
                    if (user.IsBlocked)
                    {
                        MessageBox.Show((string)Application.Current.Resources["AdminPanelBlockAdminError"],
                            (string)Application.Current.Resources["AdminPanelLimitationTitle"], MessageBoxButton.OK, MessageBoxImage.Warning);
                        user.IsBlocked = false;
                    }
                }

                // Обновляем свойства свежего пользователя
                freshUser.FullName = user.FullName;
                freshUser.Email = user.Email;
                freshUser.Login = user.Login;
                freshUser.Role = user.Role;
                freshUser.IsBlocked = user.IsBlocked;

                // Сохраняем изменения в базе данных
                bool result = _userService.Update(freshUser);
                
                if (result)
                {
                    // После успешного обновления, обновляем данные в представлении
                    // Это гарантирует, что все изменения будут корректно отображены
                    int index = UsersTable.IndexOf(user);
                    if (index >= 0)
                    {
                        UsersTable[index] = freshUser;
                    }
                    
                    // Обновляем UI
                    OnPropertyChanged(nameof(UsersTable));
                }
                else
                {
                    MessageBox.Show((string)Application.Current.Resources["AdminPanelUpdateUserError"],
                        (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(string)Application.Current.Resources["AdminPanelUpdateUserError"]}: {ex.Message}",
                    (string)Application.Current.Resources["AdminPanelError"], MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public User GetCurrentUser()
        {
            return _userService.GetCurrentUser();
        }

        #endregion
    }
} 