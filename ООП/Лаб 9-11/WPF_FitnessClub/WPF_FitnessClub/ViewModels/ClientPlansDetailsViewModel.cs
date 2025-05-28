using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPF_FitnessClub.Data.Services;
using WPF_FitnessClub.Models;
using static WPF_FitnessClub.Commands;
using System.Collections.Generic;
using WPF_FitnessClub.View;
using System.Configuration;
using System.Windows.Controls;

namespace WPF_FitnessClub.ViewModels
{
    public class ClientPlansDetailsViewModel : ViewModelBase
    {
        private readonly WorkoutPlanService _workoutPlanService;
        private readonly NutritionPlanService _nutritionPlanService;
        private readonly UserService _userService;
        
        private User _client;
        private ObservableCollection<WorkoutPlan> _workoutPlans;
        private ObservableCollection<NutritionPlan> _nutritionPlans;
        private WorkoutPlan _selectedWorkoutPlan;
        private NutritionPlan _selectedNutritionPlan;
        private bool _isLoading;
        private int _selectedTabIndex;
        
        private string _newNutritionPlanTitle;
        private string _newNutritionPlanDescription;
        private DateTime _newNutritionPlanStartDate = DateTime.Today;
        private DateTime _newNutritionPlanEndDate = DateTime.Today.AddMonths(1);
        
        private string _newWorkoutPlanTitle;
        private string _newWorkoutPlanDescription;
        private DateTime _newWorkoutPlanStartDate = DateTime.Today;
        private DateTime _newWorkoutPlanEndDate = DateTime.Today.AddMonths(1);
        
        private DateTime _currentDate = DateTime.Today;
        
        // Команды для работы с планами питания
        public ICommand AddNutritionPlanCommand { get; private set; }
        public ICommand EditNutritionPlanCommand { get; private set; }
        public ICommand DeleteNutritionPlanCommand { get; private set; }
        public ICommand SaveNutritionPlanCommand { get; private set; }
        public ICommand CancelNutritionPlanEditCommand { get; private set; }
        
        // Команды для работы с планами тренировок
        public ICommand AddWorkoutPlanCommand { get; private set; }
        public ICommand EditWorkoutPlanCommand { get; private set; }
        public ICommand DeleteWorkoutPlanCommand { get; private set; }
        public ICommand SaveWorkoutPlanCommand { get; private set; }
        public ICommand CancelWorkoutPlanEditCommand { get; private set; }

        // Свойство для хранения информации о клиенте
        public User Client
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged("Client");
                
                // При изменении клиента загружаем его планы
                if (_client != null)
                {
                    LoadClientPlans();
                }
            }
        }

        // Свойство для хранения планов тренировок
        public ObservableCollection<WorkoutPlan> WorkoutPlans
        {
            get => _workoutPlans;
            set
            {
                _workoutPlans = value;
                OnPropertyChanged("WorkoutPlans");
            }
        }

        // Свойство для хранения планов питания
        public ObservableCollection<NutritionPlan> NutritionPlans
        {
            get => _nutritionPlans;
            set
            {
                _nutritionPlans = value;
                OnPropertyChanged("NutritionPlans");
            }
        }

        // Свойство для хранения выбранного плана тренировок
        public WorkoutPlan SelectedWorkoutPlan
        {
            get => _selectedWorkoutPlan;
            set
            {
                _selectedWorkoutPlan = value;
                OnPropertyChanged("SelectedWorkoutPlan");
            }
        }

        // Свойство для хранения выбранного плана питания
        public NutritionPlan SelectedNutritionPlan
        {
            get => _selectedNutritionPlan;
            set
            {
                _selectedNutritionPlan = value;
                OnPropertyChanged("SelectedNutritionPlan");
            }
        }
     
        // Свойство для выбранной вкладки
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged("SelectedTabIndex");
            }
        }

        // Новые свойства для редактирования плана питания
        public string NewNutritionPlanTitle
        {
            get => _newNutritionPlanTitle;
            set
            {
                _newNutritionPlanTitle = value;
                OnPropertyChanged("NewNutritionPlanTitle");
            }
        }
        
        public string NewNutritionPlanDescription
        {
            get => _newNutritionPlanDescription;
            set
            {
                _newNutritionPlanDescription = value;
                OnPropertyChanged("NewNutritionPlanDescription");
            }
        }
        
        public DateTime NewNutritionPlanStartDate
        {
            get => _newNutritionPlanStartDate;
            set
            {
                _newNutritionPlanStartDate = value;
                
                // Если дата начала позже даты окончания, сдвигаем дату окончания
                if (_newNutritionPlanStartDate > _newNutritionPlanEndDate)
                {
                    NewNutritionPlanEndDate = _newNutritionPlanStartDate.AddMonths(1);
                }
                
                OnPropertyChanged("NewNutritionPlanStartDate");
            }
        }
        
        public DateTime NewNutritionPlanEndDate
        {
            get => _newNutritionPlanEndDate;
            set
            {
                _newNutritionPlanEndDate = value;
                OnPropertyChanged("NewNutritionPlanEndDate");
            }
        }
        
        // Режим редактирования для плана питания
        private bool _isNutritionPlanEditMode;
        public bool IsNutritionPlanEditMode
        {
            get => _isNutritionPlanEditMode;
            set
            {
                _isNutritionPlanEditMode = value;
                OnPropertyChanged("IsNutritionPlanEditMode");
                OnPropertyChanged("IsNutritionPlanViewMode");
            }
        }
        
        // Вспомогательное свойство для режима просмотра
        public bool IsNutritionPlanViewMode => !_isNutritionPlanEditMode;
        
        // Флаг, указывающий, редактируем ли существующий план или создаем новый
        private bool _isEditingExistingNutritionPlan;
        public bool IsEditingExistingNutritionPlan
        {
            get => _isEditingExistingNutritionPlan;
            set
            {
                _isEditingExistingNutritionPlan = value;
                OnPropertyChanged("IsEditingExistingNutritionPlan");
            }
        }

        // Свойства для редактирования плана тренировок
        public string NewWorkoutPlanTitle
        {
            get => _newWorkoutPlanTitle;
            set
            {
                _newWorkoutPlanTitle = value;
                OnPropertyChanged("NewWorkoutPlanTitle");
            }
        }
        
        public string NewWorkoutPlanDescription
        {
            get => _newWorkoutPlanDescription;
            set
            {
                _newWorkoutPlanDescription = value;
                OnPropertyChanged("NewWorkoutPlanDescription");
            }
        }
        
        public DateTime NewWorkoutPlanStartDate
        {
            get => _newWorkoutPlanStartDate;
            set
            {
                _newWorkoutPlanStartDate = value;
                
                // Если дата начала позже даты окончания, сдвигаем дату окончания
                if (_newWorkoutPlanStartDate > _newWorkoutPlanEndDate)
                {
                    NewWorkoutPlanEndDate = _newWorkoutPlanStartDate.AddMonths(1);
                }
                
                OnPropertyChanged("NewWorkoutPlanStartDate");
            }
        }
        
        public DateTime NewWorkoutPlanEndDate
        {
            get => _newWorkoutPlanEndDate;
            set
            {
                _newWorkoutPlanEndDate = value;
                OnPropertyChanged("NewWorkoutPlanEndDate");
            }
        }
        
        // Режим редактирования для плана тренировок
        private bool _isWorkoutPlanEditMode;
        public bool IsWorkoutPlanEditMode
        {
            get => _isWorkoutPlanEditMode;
            set
            {
                _isWorkoutPlanEditMode = value;
                OnPropertyChanged("IsWorkoutPlanEditMode");
                OnPropertyChanged("IsWorkoutPlanViewMode");
            }
        }
        
        // Вспомогательное свойство для режима просмотра плана тренировок
        public bool IsWorkoutPlanViewMode => !_isWorkoutPlanEditMode;
        
        // Флаг, указывающий, редактируем ли существующий план тренировок или создаем новый
        private bool _isEditingExistingWorkoutPlan;
        public bool IsEditingExistingWorkoutPlan
        {
            get => _isEditingExistingWorkoutPlan;
            set
            {
                _isEditingExistingWorkoutPlan = value;
                OnPropertyChanged("IsEditingExistingWorkoutPlan");
            }
        }

        // Свойство для текущей даты
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged("CurrentDate");
            }
        }

        // Конструктор
        public ClientPlansDetailsViewModel(User client)
        {
            _workoutPlanService = new WorkoutPlanService();
            _nutritionPlanService = new NutritionPlanService();
            _userService = new UserService();
            
            WorkoutPlans = new ObservableCollection<WorkoutPlan>();
            NutritionPlans = new ObservableCollection<NutritionPlan>();
            
            SelectedTabIndex = 0; // По умолчанию выбрана первая вкладка
            
            // Инициализация команд для работы с планами питания
            AddNutritionPlanCommand = new RelayCommand(ExecuteAddNutritionPlan);
            EditNutritionPlanCommand = new RelayCommand(ExecuteEditNutritionPlan, CanExecuteEditNutritionPlan);
            DeleteNutritionPlanCommand = new RelayCommand(ExecuteDeleteNutritionPlan, CanExecuteDeleteNutritionPlan);
            SaveNutritionPlanCommand = new RelayCommand(ExecuteSaveNutritionPlan, CanExecuteSaveNutritionPlan);
            CancelNutritionPlanEditCommand = new RelayCommand(ExecuteCancelNutritionPlanEdit);
            
            // Инициализация команд для работы с планами тренировок
            AddWorkoutPlanCommand = new RelayCommand(ExecuteAddWorkoutPlan);
            EditWorkoutPlanCommand = new RelayCommand(ExecuteEditWorkoutPlan, CanExecuteEditWorkoutPlan);
            DeleteWorkoutPlanCommand = new RelayCommand(ExecuteDeleteWorkoutPlan, CanExecuteDeleteWorkoutPlan);
            SaveWorkoutPlanCommand = new RelayCommand(ExecuteSaveWorkoutPlan, CanExecuteSaveWorkoutPlan);
            CancelWorkoutPlanEditCommand = new RelayCommand(ExecuteCancelWorkoutPlanEdit);
            
            // Инициализация дат по умолчанию
            ResetNewNutritionPlanFields();
            ResetNewWorkoutPlanFields();
            
            Client = client;
        }

        // Сбросить поля для нового плана питания
        private void ResetNewNutritionPlanFields()
        {
            NewNutritionPlanTitle = string.Empty;
            NewNutritionPlanDescription = string.Empty;
            NewNutritionPlanStartDate = DateTime.Today;
            NewNutritionPlanEndDate = DateTime.Today.AddMonths(1);
        }

        // Методы для команд работы с планами питания
        
        // Добавление нового плана питания
        private void ExecuteAddNutritionPlan(object parameter)
        {
            IsEditingExistingNutritionPlan = false;
            ResetNewNutritionPlanFields();
            IsNutritionPlanEditMode = true;
        }
        
        // Редактирование существующего плана питания
        private void ExecuteEditNutritionPlan(object parameter)
        {
            if (SelectedNutritionPlan != null)
            {
                IsEditingExistingNutritionPlan = true;
                
                // Копируем данные выбранного плана в поля редактирования
                NewNutritionPlanTitle = SelectedNutritionPlan.Title;
                NewNutritionPlanDescription = SelectedNutritionPlan.Description;
                NewNutritionPlanStartDate = SelectedNutritionPlan.StartDate;
                NewNutritionPlanEndDate = SelectedNutritionPlan.EndDate;
                
                IsNutritionPlanEditMode = true;
            }
        }
        
        private void ExecuteDeleteNutritionPlan(object parameter)
        {
            if (SelectedNutritionPlan != null)
            {
                var result = MessageBox.Show(
                    string.Format(Application.Current.FindResource("ConfirmDeleteNutritionPlan") as string, SelectedNutritionPlan.Title),
                    Application.Current.FindResource("ConfirmationDelete") as string,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        
                        int planId = SelectedNutritionPlan.Id;
                        var planToRemove = SelectedNutritionPlan;
                        
                        SelectedNutritionPlan = null;
                        
                        _nutritionPlanService.Delete(planId);
                        
                        NutritionPlans.Remove(planToRemove);
                        
                        MessageBox.Show(
                            Application.Current.FindResource("NutritionPlanDeleteSuccess") as string,
                            Application.Current.FindResource("Success") as string,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            string.Format(Application.Current.FindResource("NutritionPlanDeleteError") as string, 
                                ex.Message, ex.InnerException?.Message),
                            Application.Current.FindResource("Error") as string,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        
                        LoadClientPlans();
                    }
                }
            }
        }
        
        private void ExecuteSaveNutritionPlan(object parameter)
        {
            // Валидация дат
            if (NewNutritionPlanEndDate < NewNutritionPlanStartDate)
            {
                MessageBox.Show(
                    Application.Current.FindResource("DateEndBeforeDateStart") as string,
                    Application.Current.FindResource("ValidationErrorDate") as string,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                
                if (IsEditingExistingNutritionPlan)
                {
                    // Обновляем существующий план
                    SelectedNutritionPlan.Title = NewNutritionPlanTitle;
                    SelectedNutritionPlan.Description = NewNutritionPlanDescription;
                    SelectedNutritionPlan.StartDate = NewNutritionPlanStartDate;
                    SelectedNutritionPlan.EndDate = NewNutritionPlanEndDate;
                    SelectedNutritionPlan.UpdatedDate = DateTime.Now;
                    
                    // Сохраняем изменения в БД
                    _nutritionPlanService.Update(SelectedNutritionPlan);
                    
                    MessageBox.Show(
                        Application.Current.FindResource("NutritionPlanUpdateSuccess") as string,
                        Application.Current.FindResource("Success") as string,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    // Создаем новый план
                    var coach = _userService.GetCurrentUser(); // Предполагается, что текущий пользователь - тренер
                    
                    NutritionPlan newPlan = new NutritionPlan
                    {
                        Title = NewNutritionPlanTitle,
                        Description = NewNutritionPlanDescription,
                        ClientId = Client.Id,
                        CoachId = coach.Id,
                        StartDate = NewNutritionPlanStartDate,
                        EndDate = NewNutritionPlanEndDate,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    
                    // Сохраняем в БД
                    var createdPlan = _nutritionPlanService.Create(newPlan);
                    
                    // Добавляем в коллекцию
                    NutritionPlans.Add(createdPlan);
                    
                    MessageBox.Show(
                        Application.Current.FindResource("NutritionPlanCreateSuccess") as string,
                        Application.Current.FindResource("Success") as string,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                
                // Выходим из режима редактирования
                IsNutritionPlanEditMode = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Application.Current.FindResource("NutritionPlanSaveError") as string, ex.Message),
                    Application.Current.FindResource("Error") as string,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }
        
        // Отмена редактирования плана питания
        private void ExecuteCancelNutritionPlanEdit(object parameter)
        {
            IsNutritionPlanEditMode = false;
            ResetNewNutritionPlanFields();
        }
        
        // Проверки возможности выполнения команд
        private bool CanExecuteEditNutritionPlan(object parameter)
        {
            return SelectedNutritionPlan != null && !IsNutritionPlanEditMode;
        }
        
        private bool CanExecuteDeleteNutritionPlan(object parameter)
        {
            return SelectedNutritionPlan != null && !IsNutritionPlanEditMode;
        }
        
        private bool CanExecuteSaveNutritionPlan(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewNutritionPlanTitle) &&
                   NewNutritionPlanEndDate > NewNutritionPlanStartDate;
        }

        public void LoadClientPlans()
        {
            try
            {
                
                // Загружаем планы тренировок
                var workoutPlans = _workoutPlanService.GetWorkoutPlansByClientId(Client.Id);
                // Загружаем планы питания
                var nutritionPlans = _nutritionPlanService.GetNutritionPlansByClientId(Client.Id);
                
                // Обновляем коллекции в UI потоке
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkoutPlans.Clear();
                    foreach (var plan in workoutPlans)
                    {
                        WorkoutPlans.Add(plan);
                    }
                    
                    NutritionPlans.Clear();
                    foreach (var plan in nutritionPlans)
                    {
                        NutritionPlans.Add(plan);
                    }
                });
                
                System.Diagnostics.Debug.WriteLine($"Загружено {WorkoutPlans.Count} планов тренировок и {NutritionPlans.Count} планов питания для клиента {Client.FullName} (ID: {Client.Id})");
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        string.Format(Application.Current.FindResource("LoadClientPlansError") as string, ex.Message),
                        Application.Current.FindResource("Error") as string,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
                
                System.Diagnostics.Debug.WriteLine($"Ошибка при загрузке планов клиента: {ex.Message}");
            }
        }

        private void ExecuteAddWorkoutPlan(object parameter)
        {
            IsEditingExistingWorkoutPlan = false;
            ResetNewWorkoutPlanFields();
            IsWorkoutPlanEditMode = true;
        }
        
        private void ExecuteEditWorkoutPlan(object parameter)
        {
            if (SelectedWorkoutPlan != null)
            {
                IsEditingExistingWorkoutPlan = true;
                
                // Копируем данные выбранного плана в поля редактирования
                NewWorkoutPlanTitle = SelectedWorkoutPlan.Title;
                NewWorkoutPlanDescription = SelectedWorkoutPlan.Description;
                NewWorkoutPlanStartDate = SelectedWorkoutPlan.StartDate;
                NewWorkoutPlanEndDate = SelectedWorkoutPlan.EndDate;
                
                IsWorkoutPlanEditMode = true;
            }
        }
        
        private void ExecuteDeleteWorkoutPlan(object parameter)
        {
            if (SelectedWorkoutPlan != null)
            {
                var result = MessageBox.Show(
                    string.Format(Application.Current.FindResource("ConfirmDeleteWorkoutPlan") as string, SelectedWorkoutPlan.Title),
                    Application.Current.FindResource("ConfirmationDelete") as string,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        
                        // Сохраняем ID и другие данные перед удалением
                        int planId = SelectedWorkoutPlan.Id;
                        var planToRemove = SelectedWorkoutPlan;
                        
                        // Отменяем выбор плана перед удалением
                        SelectedWorkoutPlan = null;
                        
                        // Удаляем план из базы данных
                        _workoutPlanService.Delete(planId);
                        
                        // Удаляем план из коллекции
                        WorkoutPlans.Remove(planToRemove);
                        
                        MessageBox.Show(
                            Application.Current.FindResource("WorkoutPlanDeleteSuccess") as string,
                            Application.Current.FindResource("Success") as string,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            string.Format(Application.Current.FindResource("WorkoutPlanDeleteError") as string, 
                                ex.Message, ex.InnerException?.Message),
                            Application.Current.FindResource("Error") as string,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        
                        // Перезагружаем данные после ошибки
                        LoadClientPlans();
                    }
                }
            }
        }
        
        private void ExecuteSaveWorkoutPlan(object parameter)
        {
            // Валидация дат
            if (NewWorkoutPlanEndDate < NewWorkoutPlanStartDate)
            {
                MessageBox.Show(
                    Application.Current.FindResource("DateEndBeforeDateStart") as string,
                    Application.Current.FindResource("ValidationErrorDate") as string,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                
                if (IsEditingExistingWorkoutPlan)
                {
                    // Обновляем существующий план
                    SelectedWorkoutPlan.Title = NewWorkoutPlanTitle;
                    SelectedWorkoutPlan.Description = NewWorkoutPlanDescription;
                    SelectedWorkoutPlan.StartDate = NewWorkoutPlanStartDate;
                    SelectedWorkoutPlan.EndDate = NewWorkoutPlanEndDate;
                    SelectedWorkoutPlan.UpdatedDate = DateTime.Now;
                    
                    // Сохраняем изменения в БД
                    _workoutPlanService.Update(SelectedWorkoutPlan);
                    
                    MessageBox.Show(
                        Application.Current.FindResource("WorkoutPlanUpdateSuccess") as string,
                        Application.Current.FindResource("Success") as string,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    // Создаем новый план
                    var coach = _userService.GetCurrentUser(); // Предполагается, что текущий пользователь - тренер
                    
                    WorkoutPlan newPlan = new WorkoutPlan
                    {
                        Title = NewWorkoutPlanTitle,
                        Description = NewWorkoutPlanDescription,
                        ClientId = Client.Id,
                        CoachId = coach.Id,
                        StartDate = NewWorkoutPlanStartDate,
                        EndDate = NewWorkoutPlanEndDate,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    
                    // Сохраняем в БД
                    var createdPlan = _workoutPlanService.Create(newPlan);
                    
                    // Добавляем в коллекцию
                    WorkoutPlans.Add(createdPlan);
                    
                    MessageBox.Show(
                        Application.Current.FindResource("WorkoutPlanCreateSuccess") as string,
                        Application.Current.FindResource("Success") as string,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                
                // Выходим из режима редактирования
                IsWorkoutPlanEditMode = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Application.Current.FindResource("WorkoutPlanSaveError") as string, ex.Message),
                    Application.Current.FindResource("Error") as string,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }
        
        private void ExecuteCancelWorkoutPlanEdit(object parameter)
        {
            IsWorkoutPlanEditMode = false;
            ResetNewWorkoutPlanFields();
        }
        
        // Проверки возможности выполнения команд для планов тренировок
        private bool CanExecuteEditWorkoutPlan(object parameter)
        {
            return SelectedWorkoutPlan != null && !IsWorkoutPlanEditMode;
        }
        
        private bool CanExecuteDeleteWorkoutPlan(object parameter)
        {
            return SelectedWorkoutPlan != null && !IsWorkoutPlanEditMode;
        }
        
        private bool CanExecuteSaveWorkoutPlan(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewWorkoutPlanTitle) &&
                   NewWorkoutPlanEndDate > NewWorkoutPlanStartDate;
        }
        
        private void ResetNewWorkoutPlanFields()
        {
            NewWorkoutPlanTitle = string.Empty;
            NewWorkoutPlanDescription = string.Empty;
            NewWorkoutPlanStartDate = DateTime.Today;
            NewWorkoutPlanEndDate = DateTime.Today.AddMonths(1);
        }
    }
} 