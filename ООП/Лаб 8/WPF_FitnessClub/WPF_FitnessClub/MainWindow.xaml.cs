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
using static WPF_FitnessClub.Commands;
using WPF_FitnessClub.SQL;
using WPF_FitnessClub.Data;

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public User _user;
		public readonly UserRole currentUserRole;
		readonly string subscriptionsFilePath = "subscriptions.js";
		List<Subscription> subscriptions = new List<Subscription>();
		DataAccess dataAccess = new DataAccess();
		
		public UndoManager UndoManager { get; } = new UndoManager();
		private Visibility editModeVisible = Visibility.Collapsed;

		public Visibility EditModeVisible
		{
			get => editModeVisible;
			set
			{
				editModeVisible = value;
				OnPropertyChanged(nameof(EditModeVisible));
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


			InitializeComponent();
			DataContext = this;

			UndoManager = new UndoManager(this);

			// Инициализация сервиса навигации
			NavigationManager.Instance.Initialize(MainContent);

			LoadSubscriptions();

			// Создание и отображение домашней страницы
			ShowHomePage();

			if (currentUserRole == UserRole.Admin || currentUserRole == UserRole.Coach)
			{
				EditModeVisible = Visibility.Visible;
			}

			ThemeManager.Instance.ThemeChanged += OnThemeChanged;
		}

		private void LoadSubscriptions()
		{

			try
			{
				subscriptions = dataAccess.GetAllSubscriptions();
					
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorLoadingData"], ex.Message));
				subscriptions = new List<Subscription>();
			}

		}

		public void SaveSubscriptions()
		{

			try
			{
				string json = JsonConvert.SerializeObject(subscriptions, Formatting.Indented);
				File.WriteAllText(subscriptionsFilePath, json);

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
			var detailsWindow = new SubscriptionDetailsView(this, subscriptions, subscription, currentUserRole, UndoManager);
			detailsWindow.ShowDialog();
		}

		public void UpdateUIWithSubscriptions(List<Subscription> updatedSubscriptions)
		{
			subscriptions = updatedSubscriptions;
			SaveSubscriptions();

			// Обновляем домашнюю страницу с новыми данными
			if (_homePageView != null)
			{
				_homePageView.UpdateSubscriptions(subscriptions, true);
			}
		}

		public void UpdateSubscriptions()
		{
			SaveSubscriptions();

			// Обновляем домашнюю страницу с новыми данными
			if (_homePageView != null)
			{
				_homePageView.UpdateSubscriptions(subscriptions);
			}
		}

		private void PersonalAccountButon_Click(object sender, RoutedEventArgs e)
		{
			// Используем сервис навигации для перехода к личному кабинету
			var personalAccountView = new PersonalAccountView(_user);
			NavigationManager.Instance.NavigateTo(personalAccountView);
		}

		private void UndoButton_Click(object sender, RoutedEventArgs e)
		{
			UndoManager.Undo();
		}

		private void RedoButton_Click(object sender, RoutedEventArgs e)
		{
			UndoManager.Redo();
		}

		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			// Возвращаемся на главную страницу с абонементами
			ShowHomePage();
		}

		private void AddSubscriptionButton_Click(object sender, RoutedEventArgs e)
		{
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
					var cmd = new AddSubscriptionCommand(new ObservableCollection<Subscription>(subscriptions), subscription);
					UndoManager.ExecuteAction(cmd);
				}
			};

			dialog.Content = subscriptionView;
			dialog.ShowDialog();
		}

		private void DataTableButton_Click(object sender, RoutedEventArgs e)
		{
			var dataTableView = new DataTableView();
			NavigationManager.Instance.NavigateTo(dataTableView);
		}

	}
}
