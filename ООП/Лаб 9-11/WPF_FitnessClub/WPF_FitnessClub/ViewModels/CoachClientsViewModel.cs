using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Linq;
using WPF_FitnessClub.Data.Services;
using WPF_FitnessClub.Models;
using System.Collections.Generic;

namespace WPF_FitnessClub.ViewModels
{
    public class CoachClientsViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private CoachClientService _coachClientService;

        private User _currentCoach;
        private ObservableCollection<User> _clients;
        private ObservableCollection<User> _availableClients;
        private User _selectedClient;
        private User _selectedAvailableClient;
        private int _selectedTabIndex;
        
        // Словарь для хранения дат добавления клиентов к тренеру
        private Dictionary<int, DateTime> _clientAssignmentDates;
        
        // Свойство для текущего типа сортировки
        private SortType _currentSortType = SortType.ByName;
        
        // Перечисление для типов сортировки
        public enum SortType
        {
            ByName,
            ByEmail,
            ByDate
        }

        // Свойство для хранения текущего тренера
        public User CurrentCoach
        {
            get => _currentCoach;
            set
            {
                _currentCoach = value;
                OnPropertyChanged(nameof(CurrentCoach));
                
                // При изменении текущего тренера загружаем его клиентов
                if (_currentCoach != null)
                {
                    LoadClients();
                    LoadAvailableClients();
                }
            }
        }

        // Свойство для хранения списка клиентов
        public ObservableCollection<User> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }
        
        // Свойство для хранения списка доступных клиентов (без тренера)
        public ObservableCollection<User> AvailableClients
        {
            get => _availableClients;
            set
            {
                _availableClients = value;
                OnPropertyChanged(nameof(AvailableClients));
            }
        }

        // Свойство для хранения выбранного клиента
        public User SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }
        
        // Свойство для хранения выбранного доступного клиента
        public User SelectedAvailableClient
        {
            get => _selectedAvailableClient;
            set
            {
                _selectedAvailableClient = value;
                OnPropertyChanged(nameof(SelectedAvailableClient));
            }
        }
        
        // Свойство для выбранной вкладки
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                
                // При переключении на вкладку "Все клиенты" загружаем список доступных клиентов
                if (_selectedTabIndex == 1)
                {
                    LoadAvailableClients();
                }
            }
        }
        
        // Свойство для получения даты добавления клиента
        public DateTime GetClientAssignmentDate(int clientId)
        {
            if (_clientAssignmentDates != null && _clientAssignmentDates.ContainsKey(clientId))
            {
                return _clientAssignmentDates[clientId];
            }
            return DateTime.MinValue;
        }
        
        // Команды

        // Конструктор
        public CoachClientsViewModel(User coach)
        {
            _userService = new UserService();
            _coachClientService = new CoachClientService();
            
            Clients = new ObservableCollection<User>();
            AvailableClients = new ObservableCollection<User>();
            _clientAssignmentDates = new Dictionary<int, DateTime>();
            SelectedTabIndex = 0; // По умолчанию выбрана первая вкладка
            CurrentCoach = coach;
        }

        // Метод загрузки клиентов текущего тренера
        public void LoadClients()
        {
            try
            {
                var coachClients = _coachClientService.GetCoachClients(CurrentCoach.Id);
                
                _clientAssignmentDates = _coachClientService.GetClientAssignmentDates(CurrentCoach.Id);
                
                var sortedClients = SortClients(coachClients);
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Clear();
                    foreach (var client in sortedClients)
                    {
                        Clients.Add(client);
                    }
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        string.Format((string)Application.Current.FindResource("ErrorRefreshingLists"), ex.Message),
                        (string)Application.Current.FindResource("ErrorTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }
        
        // Метод сортировки клиентов
        private List<User> SortClients(List<User> clients)
        {
            switch (_currentSortType)
            {
                case SortType.ByName:
                    return clients.OrderBy(c => c.FullName).ToList();
                case SortType.ByEmail:
                    return clients.OrderBy(c => c.Email).ToList();
                case SortType.ByDate:
                    return clients.OrderByDescending(c => GetClientAssignmentDate(c.Id)).ToList();
                default:
                    return clients;
            }
        }
        
        // Метод для изменения типа сортировки
        public void SortClientsList(SortType sortType)
        {
            _currentSortType = sortType;
            
            if (Clients != null && Clients.Count > 0)
            {
                var sortedClients = SortClients(Clients.ToList());
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Clear();
                    foreach (var client in sortedClients)
                    {
                        Clients.Add(client);
                    }
                });
            }
        }
        
        // Метод загрузки всех доступных клиентов без тренера
        public void LoadAvailableClients()
        {
            try
            {
                if (CurrentCoach == null)
                {
                    return;
                }
                
                var clientsWithoutCoach = _coachClientService.GetClientsWithoutCoach();
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AvailableClients.Clear();
                    foreach (var client in clientsWithoutCoach)
                    {
                        AvailableClients.Add(client);
                    }
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        string.Format((string)Application.Current.FindResource("ErrorRefreshingLists"), ex.Message),
                        (string)Application.Current.FindResource("ErrorTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }
        
        // Метод для добавления клиента к текущему тренеру
        public void AddClientToCoach(User client)
        {
            if (client == null || CurrentCoach == null)
            {
                MessageBox.Show(
                    (string)Application.Current.FindResource("ErrorGettingClientData"),
                    (string)Application.Current.FindResource("ErrorTitle"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            if (_coachClientService == null)
            {
                _coachClientService = new CoachClientService();
            }
            
            if (client.Id <= 0 || CurrentCoach.Id <= 0)
            {
                MessageBox.Show(
                    (string)Application.Current.FindResource("ErrorGettingClientData"),
                    (string)Application.Current.FindResource("ErrorTitle"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            try
            {
                int clientId = client.Id;
                int coachId = CurrentCoach.Id;
                
                bool result = _coachClientService.AssignClientToCoach(clientId, coachId);
                
                if (result)
                {
                    LoadClients();
                    LoadAvailableClients();
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            string.Format((string)Application.Current.FindResource("ClientAddedSuccessfully"), client.FullName),
                            (string)Application.Current.FindResource("SuccessTitle"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            string.Format((string)Application.Current.FindResource("FailedToAddClient"), client.FullName),
                            (string)Application.Current.FindResource("ErrorTitle"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    });
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        string.Format((string)Application.Current.FindResource("ErrorAddingClient"), ex.Message),
                        (string)Application.Current.FindResource("ErrorTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }

        // Метод для удаления клиента у текущего тренера
        public void RemoveClientFromCoach(User client)
        {
            if (client == null || CurrentCoach == null)
            {
                MessageBox.Show(
                    (string)Application.Current.FindResource("ErrorGettingClientData"),
                    (string)Application.Current.FindResource("ErrorTitle"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            if (_coachClientService == null)
            {
                _coachClientService = new CoachClientService();
            }
            
            try
            {
                int clientId = client.Id;
                int coachId = CurrentCoach.Id;
                
                bool result = _coachClientService.RemoveClientFromCoach(clientId, coachId);
                
                if (result)
                {
                    LoadClients();
                    LoadAvailableClients();
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            string.Format((string)Application.Current.FindResource("ClientRemovedSuccessfully"), client.FullName),
                            (string)Application.Current.FindResource("SuccessTitle"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            string.Format((string)Application.Current.FindResource("FailedToRemoveClient"), client.FullName),
                            (string)Application.Current.FindResource("ErrorTitle"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    });
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        string.Format((string)Application.Current.FindResource("ErrorRemovingClient"), ex.Message),
                        (string)Application.Current.FindResource("ErrorTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }
    }
} 