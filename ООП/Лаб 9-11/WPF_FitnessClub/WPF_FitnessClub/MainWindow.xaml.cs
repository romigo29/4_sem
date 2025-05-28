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
using System.Windows.Navigation;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Channels;
using WPF_FitnessClub;
using System.ComponentModel;
using WPF_FitnessClub.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WPF_FitnessClub.View;
using WPF_FitnessClub.Data;
using WPF_FitnessClub.Data.Services;
using WPF_FitnessClub.Data.Services.Interfaces;
using WPF_FitnessClub.ViewModels;

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged, IDisposable
	{
		public User _user;
		public readonly UserRole currentUserRole;
		public List<Subscription> subscriptions = new List<Subscription>();
		private SubscriptionService _subscriptionService;
		private UserService _userService;
		private IWorkoutPlanService _workoutPlanService;
		private INutritionPlanService _nutritionPlanService;
		private bool _disposed = false;
		
		// Список открытых окон с деталями абонементов
		private List<SubscriptionDetailsView> _openSubscriptionDetailsViews = new List<SubscriptionDetailsView>();
		
		private Visibility editModeVisible = Visibility.Collapsed;
		private Visibility adminRoleVisible = Visibility.Collapsed;
		private Visibility coachRoleVisible = Visibility.Collapsed;
		private Visibility clientRoleVisible = Visibility.Collapsed;
		private Visibility addSubscriptionVisible = Visibility.Collapsed;

		public User CurrentUser => _user;
		
		public Visibility EditModeVisible
		{
			get => editModeVisible;
			set
			{
				editModeVisible = value;
				OnPropertyChanged(nameof(EditModeVisible));
			}
		}

		public Visibility AdminRoleVisible
		{
			get => adminRoleVisible;
			set
			{
				adminRoleVisible = value;
				OnPropertyChanged(nameof(AdminRoleVisible));
			}
		}

		public Visibility CoachRoleVisible
		{
			get => coachRoleVisible;
			set
			{
				coachRoleVisible = value;
				OnPropertyChanged(nameof(CoachRoleVisible));
			}
		}

		public Visibility ClientRoleVisible
		{
			get => clientRoleVisible;
			set
			{
				clientRoleVisible = value;
				OnPropertyChanged(nameof(ClientRoleVisible));
			}
		}

		public Visibility AddSubscriptionVisible
		{
			get => addSubscriptionVisible;
			set
			{
				addSubscriptionVisible = value;
				OnPropertyChanged(nameof(AddSubscriptionVisible));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private SubscriptionsView _homePageView;

		public MainWindow(User user)
		{
			_user = user;
			currentUserRole = user.Role;

			_subscriptionService = new SubscriptionService();
			_userService = new UserService();
			_workoutPlanService = new WorkoutPlanService();
			_nutritionPlanService = new NutritionPlanService();
			
			// Установка текущего пользователя
			UserService.SetCurrentUser(user);

			InitializeComponent();
			DataContext = this;

			// Установка полноэкранного режима
			this.WindowState = WindowState.Maximized;

			// Инициализация сервиса навигации
			NavigationManager.Instance.Initialize(MainContent);

			LoadSubscriptions();

			// Создание и отображение домашней страницы
			ShowHomePage();

			// Установка видимости элементов в зависимости от роли пользователя
			switch (currentUserRole)
			{
				case UserRole.Admin:
					EditModeVisible = Visibility.Visible;
					AdminRoleVisible = Visibility.Visible;
					CoachRoleVisible = Visibility.Collapsed;
					ClientRoleVisible = Visibility.Collapsed;
					AddSubscriptionVisible = Visibility.Visible;
					break;
				case UserRole.Coach:
					EditModeVisible = Visibility.Visible;
					AdminRoleVisible = Visibility.Collapsed;
					CoachRoleVisible = Visibility.Visible;
					ClientRoleVisible = Visibility.Collapsed;
					AddSubscriptionVisible = Visibility.Visible;
					break;
				case UserRole.Client:
					EditModeVisible = Visibility.Collapsed;
					AdminRoleVisible = Visibility.Collapsed;
					CoachRoleVisible = Visibility.Collapsed;
					ClientRoleVisible = Visibility.Visible;
					AddSubscriptionVisible = Visibility.Collapsed;
					break;
				default:
					EditModeVisible = Visibility.Collapsed;
					AdminRoleVisible = Visibility.Collapsed;
					CoachRoleVisible = Visibility.Collapsed;
					ClientRoleVisible = Visibility.Collapsed;
					AddSubscriptionVisible = Visibility.Collapsed;
					break;
			}

			ThemeManager.Instance.ThemeChanged += OnThemeChanged;
		}

		public void LoadSubscriptions()
		{
			try
			{
				System.Diagnostics.Debug.WriteLine("Начинаем загрузку абонементов в MainWindow");
				
				subscriptions = _subscriptionService.GetAll().ToList();
				
				// Проверка на успешную загрузку абонементов
				if (subscriptions == null)
				{
					System.Diagnostics.Debug.WriteLine("Получен null при загрузке абонементов");
					subscriptions = new List<Subscription>();
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"Загружено {subscriptions.Count} абонементов в MainWindow");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Ошибка при загрузке абонементов: {ex.Message}");
				MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorLoadingData"], ex.Message));
				subscriptions = new List<Subscription>();
			}
		}

		public void SaveSubscriptions()
		{
			try
			{
				// Данные теперь сохраняются непосредственно в базе данных через сервис
				// поэтому сохранение в JSON файл больше не требуется
				System.Diagnostics.Debug.WriteLine("Сохранение абонементов происходит автоматически через сервис");
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorSavingData"], ex.Message));
			}
		}

		private void ShowHomePage()
		{
			// Создаем представление домашней страницы, если его еще нет
			if (_homePageView == null)
			{
				_homePageView = new SubscriptionsView(this, subscriptions);
				_homePageView.SubscriptionSelected += OpenSubscriptionDetails;
			}
			else
			{
				_homePageView.UpdateSubscriptions(subscriptions);
			}

			NavigationManager.Instance.NavigateTo(_homePageView);
		}

		private void OpenSubscriptionDetails(Subscription subscription)
		{
			var detailsWindow = new SubscriptionDetailsView(this, subscriptions, subscription, currentUserRole);
			
			// Добавляем окно в список открытых окон с деталями
			_openSubscriptionDetailsViews.Add(detailsWindow);
			
			// Подписываемся на событие закрытия окна для удаления из списка
			detailsWindow.Closed += (sender, e) => 
			{
				_openSubscriptionDetailsViews.Remove(detailsWindow);
			};
			
			detailsWindow.ShowDialog();
		}

		public void UpdateUIWithSubscriptions(List<Subscription> updatedSubscriptions)
		{
			System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Обновление UI с {updatedSubscriptions?.Count ?? 0} абонементами");
			
			// Обновляем локальную коллекцию абонементов
			subscriptions = new List<Subscription>(updatedSubscriptions ?? new List<Subscription>());
			
			// Обновляем домашнюю страницу, если она загружена
			if (_homePageView != null)
			{
				System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Обновление домашней страницы");
				// Принудительно сбрасываем фильтры при удалении элементов
				bool resetFilters = subscriptions.Count < _homePageView._viewModel.FilteredSubscriptions.Count;
				_homePageView.UpdateSubscriptions(subscriptions, resetFilters);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Домашняя страница не загружена");
			}
			
			// Обновляем все открытые окна деталей абонементов
			foreach (var detailsView in _openSubscriptionDetailsViews.ToList())
			{
				try
				{
					// Получаем ID текущего отображаемого абонемента для сохранения выбора
					var vm = detailsView.DataContext as SubscriptionDetailsVM;
					if (vm != null)
					{
						int currentSubscriptionId = vm.CurrentSubscriptionId;
						System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Обработка окна с абонементом ID={currentSubscriptionId}");
						
						// Обновляем список абонементов во viewmodel
						// Важно обновить список абонементов и текущий абонемент
						var updatedCurrent = updatedSubscriptions.FirstOrDefault(s => s.Id == currentSubscriptionId);
						if (updatedCurrent != null)
						{
							System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Обновление данных абонемента ID={currentSubscriptionId}");
							vm.UpdateSubscriptionDetails(updatedCurrent, updatedSubscriptions);
						}
						else
						{
							// Если текущий абонемент был удален, закрываем окно
							System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Абонемент ID={currentSubscriptionId} не найден, закрываем окно");
							detailsView.Close();
						}
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении окна деталей абонемента: {ex.Message}");
				}
			}
			
			// Проверяем и обновляем представление администратора, если оно открыто
			if (MainContent.Content is AdminPanelView adminView)
			{
				// Обновляем данные в панели администратора
				try
				{
					// Обновляем контекст данных, если это возможно
					if (adminView.DataContext is ViewModelBase viewModel)
					{
						// Уведомляем об изменении данных
						viewModel.OnPropertyChanged("");
						System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Обновлены данные в AdminPanelView");
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении панели администратора: {ex.Message}");
				}
			}
			
			// Принудительное обновление UI
			System.Diagnostics.Debug.WriteLine($"UpdateUIWithSubscriptions: Принудительное обновление UI");
			Application.Current.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Background,
				new Action(() => { }));
		}

		public void UpdateSubscriptions()
		{
			// Перезагружаем данные из БД
			LoadSubscriptions();
			
			SaveSubscriptions();

			// Обновляем домашнюю страницу с новыми данными
			if (_homePageView != null)
			{
				_homePageView.UpdateSubscriptions(subscriptions);
			}
			
			// Обновляем все открытые окна деталей абонементов
			UpdateAllSubscriptionDetailsViews();
		}

		/// <summary>
		/// Обновляет все открытые окна с деталями абонементов
		/// </summary>
		private void UpdateAllSubscriptionDetailsViews()
		{
			// Перебираем все открытые окна деталей абонементов
			foreach (var detailsView in _openSubscriptionDetailsViews.ToList())
			{
				try
				{
					// Получаем текущий абонемент из окна
					var viewModel = detailsView.DataContext as ViewModels.SubscriptionDetailsVM;
					if (viewModel != null)
					{
						// Находим обновленную версию этого же абонемента
						int subscriptionId = viewModel.CurrentSubscriptionId;
						var updatedSubscription = subscriptions.FirstOrDefault(s => s.Id == subscriptionId);
						
						// Если нашли абонемент, обновляем представление
						if (updatedSubscription != null)
						{
							viewModel.UpdateSubscriptionDetails(updatedSubscription, subscriptions);
						}
						else
						{
							// Если абонемент был удален, закрываем окно
							detailsView.Close();
						}
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении окна деталей абонемента: {ex.Message}");
				}
			}
		}

		private void PersonalAccountButon_Click(object sender, RoutedEventArgs e)
		{
			// Используем сервис навигации для перехода к личному кабинету
			var personalAccountView = new PersonalAccountView(_user);
			NavigationManager.Instance.NavigateTo(personalAccountView);
		}

		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			// Возвращаемся на главную страницу с абонементами
			ShowHomePage();
		}

		private void AddSubscriptionButton_Click(object sender, RoutedEventArgs e)
		{
			// Проверяем, что пользователь имеет роль тренера или администратора
			if (currentUserRole != UserRole.Coach && currentUserRole != UserRole.Admin)
			{
				MessageBox.Show(
					(string)Application.Current.Resources["OnlyCoachesCanAddSubscription"],
					(string)Application.Current.Resources["AccessDenied"],
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
			}
			
			// Используем UserControl для добавления абонемента
			UseAddSubscriptionView();
		}

		private void OnThemeChanged(object sender, ThemeManager.AppTheme e)
		{
			// Обновляем UI при смене темы
			if (_homePageView != null)
			{
				_homePageView.UpdateSubscriptions(subscriptions);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			ThemeManager.Instance.ThemeChanged -= OnThemeChanged;
			Dispose();
		}

		private void UseAddSubscriptionView()
		{
			// Пример, как можно использовать контрол AddSubscriptionView в диалоговом окне
			var dialog = new Window
			{
				Title = (string)Application.Current.Resources["AddSubscriptionTitle"],
				SizeToContent = SizeToContent.WidthAndHeight,
				WindowStartupLocation = WindowStartupLocation.CenterScreen,
				Style = (Style)Application.Current.Resources["WindowStyle"]
			};

			var subscriptionView = new View.AddSubscriptionView();

			// Обработка события закрытия
			subscriptionView.CloseRequested += (success, subscription) =>
			{
				dialog.DialogResult = success;
				dialog.Close();

				// Если успешно добавлена подписка, обновляем коллекцию
				if (success && subscription != null)
				{
					// Добавляем новый абонемент в локальную коллекцию
					subscriptions.Add(subscription);
					
					// Обновляем данные в MainWindow
					if (_homePageView != null)
					{
						_homePageView.UpdateSubscriptions(subscriptions, true);
					}
					
					// Обновляем все открытые окна деталей абонементов
					UpdateAllSubscriptionDetailsViews();
				}
			};

			dialog.Content = subscriptionView;
			dialog.ShowDialog();
		}

		private void DataTableButton_Click(object sender, RoutedEventArgs e)
		{
			// Создаем новое окно для панели администратора
			var adminPanelWindow = new Window
			{
				Title = "Панель администратора",
				Width = 1200,
				Height = 700,
				WindowStartupLocation = WindowStartupLocation.CenterScreen,
				Content = new View.AdminPanelView()
			};

			// Показываем окно
			adminPanelWindow.Show();
		}

		private void CoachClientsButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Создаем окно с клиентами тренера
				var coachClientsView = new View.CoachClientsView(_user);
				
				// Показываем окно
				coachClientsView.Show();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Ошибка при открытии окна клиентов тренера: {ex.Message}");
				MessageBox.Show(
					$"Ошибка при открытии окна клиентов: {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			// Подтверждение выхода из системы
			MessageBoxResult result = MessageBox.Show(
				(string)Application.Current.Resources["ConfirmLogout"],
				(string)Application.Current.Resources["ConfirmTitle"],
				MessageBoxButton.YesNo,
				MessageBoxImage.Question);
				
			if (result == MessageBoxResult.Yes)
			{
				System.Diagnostics.Debug.WriteLine("Выход из системы для пользователя: " + _user.Login);
				
				try
				{
					// Создаем и показываем окно авторизации/регистрации
					RegistrationView registrationView = new RegistrationView();
					registrationView.Show();
					
					// Освобождаем ресурсы перед закрытием
					Dispose();
					
					// Очищаем данные пользователя
					_user = null;
					
					// Закрываем текущее окно
					this.Close();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Ошибка при выходе из системы: {ex.Message}");
					MessageBox.Show(
						$"{(string)Application.Current.Resources["ErrorLoggingOut"]}: {ex.Message}",
						(string)Application.Current.Resources["ErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Error);
				}
			}
		}

		/// <summary>
		/// Обновляет данные о абонементах пользователя в личном кабинете
		/// </summary>
		public void RefreshUserSubscriptions()
		{
			// Находим открытый личный кабинет
			var personalAccountView = NavigationManager.Instance.CurrentView as PersonalAccountView;
			if (personalAccountView != null)
			{
				// Обновляем данные о абонементах пользователя
				var viewModel = personalAccountView.DataContext as ViewModels.PersonalAccountVM;
				if (viewModel != null)
				{
					// Перезагружаем абонементы пользователя
					viewModel.LoadUserSubscriptions();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_subscriptionService.Dispose();
				}
				
				_disposed = true;
			}
		}

	}
}
