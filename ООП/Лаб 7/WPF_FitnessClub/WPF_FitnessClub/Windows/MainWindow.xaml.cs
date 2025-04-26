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
using WPF_FitnessClub.Windows;
using WPF_FitnessClub.Models;
using static WPF_FitnessClub.Commands;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

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
		List<Subscription> filteredSubscriptions = new List<Subscription>();
		private Visibility filterPanelVisibility = Visibility.Visible;

		public UndoManager UndoManager { get; } = new UndoManager();
		private Visibility editModeVisible = Visibility.Collapsed;

		public Visibility FilterPanelVisibility
		{
			get => filterPanelVisibility;
			set
			{
				filterPanelVisibility = value;
				OnPropertyChanged(nameof(FilterPanelVisibility));
			}
		}

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

		public MainWindow(User user)
		{
			_user = user;
			currentUserRole = user.Role;
			
			
			InitializeComponent();
			DataContext = this;
			
			UndoManager = new UndoManager(this);
			
			LoadSubscriptions();
			CreateSubscriptionsPanel(subscriptions);
			ConfigureUIBasedOnRole();
						
			if (currentUserRole == UserRole.Admin || currentUserRole == UserRole.Coach)
			{
				EditModeVisible = Visibility.Visible;
			}
			
			ThemeManager.Instance.ThemeChanged += OnThemeChanged;

			// Подписка на туннельные события
			RootPanel.AddHandler(
				UIElement.PreviewMouseLeftButtonDownEvent,
				new MouseButtonEventHandler(Control_Preview),
				true);
			TestControlsButton.AddHandler(
				UIElement.PreviewMouseLeftButtonDownEvent,
				new MouseButtonEventHandler(Control_Preview),
				true);
			TestEllipse.AddHandler(
				UIElement.PreviewMouseLeftButtonDownEvent,
				new MouseButtonEventHandler(Control_Preview),
				true);

			//// Подписка на пузырьковые события
			//RootPanel.AddHandler(
			//	UIElement.MouseLeftButtonDownEvent,
			//	new MouseButtonEventHandler(Control_MouseDown),
			//	true);
			//TestControlsButton.AddHandler(
			//	UIElement.MouseLeftButtonDownEvent,
			//	new MouseButtonEventHandler(Control_MouseDown),
			//	true);
			//TestEllipse.AddHandler(
			//	UIElement.MouseLeftButtonDownEvent,
			//	new MouseButtonEventHandler(Control_MouseDown),
			//	true);

		}
	

		//TO-DO
		private void ConfigureUIBasedOnRole()
		{
			switch (currentUserRole)
			{
				case UserRole.Admin:
					break;
				case UserRole.Coach:
					break;
				case UserRole.Client:
					break;
			}
		}

		private void LoadSubscriptions()
		{
			if (File.Exists(subscriptionsFilePath))
			{
				try
				{
					string json = File.ReadAllText(subscriptionsFilePath);
					subscriptions = JsonConvert.DeserializeObject <List<Subscription>>(json) ?? new List<Subscription>();
				}
				catch (Exception ex)
				{
					MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorLoadingData"], ex.Message));
					subscriptions = new List<Subscription>();
				}
			}

			else
			{
				subscriptions = new List<Subscription>
{
				new Subscription("Тренажерный зал", 85.00, "Абонемент на месяц с доступом к тренажерному залу", "/Images/iron1.jpg", "1 месяц", "Безлимит",
								new List<Review> {
								new Review("Иван", 5, "Отличный зал, много оборудования!"),
								new Review("Мария", 4, "Чисто и комфортно, но дороговато.")
				}),

				new Subscription("Тренажерный зал - премиум", 850.00, "Годовой абонемент с неограниченным доступом ко всем зонам клуба", "/Images/iron1.jpg", "12 месяцев", "Обычный",
							new List<Review> {
							new Review("Алексей", 5, "Лучший клуб в городе!"),
							new Review("Ольга", 3, "Много людей в вечернее время.")
				}),

				new Subscription("Тренажерный зал  - 1 смена", 60.00, "Абонемент на утренние тренировки с 6:00 до 12:00", "/Images/gym3.jpg", "1 месяц", "Обычный",
									new List<Review> {
									new Review("Дмитрий", 4, "Отличное время для занятий, мало людей.")
				}),

				new Subscription("Тренажерный зал – Персональные тренировки", 270.00, "Абонемент на 10 индивидуальных занятий с тренером", "/Images/gym2.jpg", "10 занятий", "Обычный",
									new List<Review> {
									new Review("Светлана", 5, "Тренер профессионал, занятия очень полезные.")
				}),

				new Subscription("Тренажерный зал - групповые тренировки", 100.00, "Абонемент на месяц с доступом к групповым тренировкам (йога, пилатес, зумба)", "/Images/yoga.jpg", "1 месяц", "Безлимит",
							new List<Review> {
								new Review("Елена", 5, "Люблю групповые тренировки, тренеры супер!"),
								new Review("Сергей", 4, "Йога – класс!")
				})
			};

				SaveSubscriptions();
	
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

		public void CreateSubscriptionsPanel(List<Subscription> _subscriptions)
		{
			SubscriptionsPanel.Children.Clear();

			foreach (Subscription subscription in _subscriptions)
			{
				StackPanel subscriptionCard = new StackPanel
				{
					Style = (Style)FindResource("ResponsiveSubscriptionCardStyle")
				};

				Image subscriptionImage = new Image
				{
					Stretch = Stretch.UniformToFill,
					Height = 250,
					Margin = new Thickness(5)
				};

				try
				{
					string imagePath = subscription.ImagePath.StartsWith("/")
						? subscription.ImagePath
						: "/Images/" + subscription.ImagePath;

					subscriptionImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
				}
				catch (Exception ex)
				{
					MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorLoadingImage"], ex.Message));
				}

				TextBlock subscriptionName = new TextBlock
				{
					Text = subscription.Name,
					FontWeight = FontWeights.Bold,
					FontSize = 16,
					Margin = new Thickness(5),
					TextWrapping = TextWrapping.Wrap,
					Foreground = (SolidColorBrush)Application.Current.Resources["TextBrush"]
				};

				TextBlock subscriptionPrice = new TextBlock
				{
					Text = $"{string.Format((string)Application.Current.Resources["Price"])}: {subscription.Price:C}",
					FontSize = 14,
					Margin = new Thickness(5),
					Foreground = (SolidColorBrush)Application.Current.Resources["TextBrush"]
				};

				TextBlock subscriptionDescription = new TextBlock
				{
					Text = subscription.Description,
					TextWrapping = TextWrapping.Wrap,
					Margin = new Thickness(5),
					FontSize = 12,
					Foreground = (SolidColorBrush)Application.Current.Resources["TextBrush"]
				};

				StackPanel ratingPanel = new StackPanel
				{
					Margin = new Thickness(5),
					Orientation = Orientation.Horizontal,
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Center
				};

				TextBlock ratingText = new TextBlock
				{
					Text = subscription.Rating.ToString(),
					FontSize = 14,
					Foreground = (SolidColorBrush)Application.Current.Resources["TextBrush"]
				};

				TextBlock star = new TextBlock
				{
					Text = subscription.Rating > 0 ? "★" : "☆",
					FontSize = 16,
					Foreground = subscription.Rating > 0 ? Brushes.Gold : Brushes.Gray,
					Margin = new Thickness(1)
				};

				ratingPanel.Children.Add(ratingText);
				ratingPanel.Children.Add(star);

				subscriptionCard.Children.Add(subscriptionImage);
				subscriptionCard.Children.Add(subscriptionName);
				subscriptionCard.Children.Add(subscriptionPrice);
				subscriptionCard.Children.Add(subscriptionDescription);
				subscriptionCard.Children.Add(ratingPanel);

				subscriptionCard.MouseDown += (sender, e) => OpenSubscriptionDetails(subscription);

				SubscriptionsPanel.Children.Add(subscriptionCard);
			}
		}

		private void AddButon_Click(object sender, RoutedEventArgs e)
		{
			AddSubscription addSubscription = new AddSubscription();

			if (addSubscription.ShowDialog() == true)
			{
				Subscription newSubscription = addSubscription.NewSubscription;
				
				var cmd = new AddSubscriptionCommand(new ObservableCollection<Subscription>(subscriptions), newSubscription);
				UndoManager.ExecuteAction(cmd);
			}
		}

		private void OpenSubscriptionDetails(Subscription subscription)
		{
			var detailsWindow = new SubscriptionDetails(this, subscriptions, subscription, currentUserRole, UndoManager);
			detailsWindow.ShowDialog();
		}

		public void UpdateSubscriptions()
		{
			SaveSubscriptions();
			CreateSubscriptionsPanel(subscriptions);
		}

		private void ApplyFilters()
		{
			filteredSubscriptions = subscriptions.Where(s =>
			{
	
				bool matchesName = string.IsNullOrWhiteSpace(SearchSubscription.Text) || 
								  s.Name.ToLower().Contains(SearchSubscription.Text.ToLower());

				bool matchesPrice = true;
				if (!string.IsNullOrWhiteSpace(MinCost.Text) && double.TryParse(MinCost.Text, out double minCostValue))
				{
					matchesPrice = matchesPrice && s.Price >= minCostValue;
				}
				if (!string.IsNullOrWhiteSpace(MaxCost.Text) && double.TryParse(MaxCost.Text, out double maxCostValue))
				{
					matchesPrice = matchesPrice && s.Price <= maxCostValue;
				}

				string chosenType = (FilterType.SelectedItem as ComboBoxItem)?.Content.ToString();
				bool matchesType = string.IsNullOrEmpty(chosenType) || 
								  chosenType == s.SubscriptionType;

	
				string chosenDuration = (FilterDuration.SelectedItem as ComboBoxItem)?.Content.ToString();
				bool matchesDuration = string.IsNullOrEmpty(chosenDuration) ||
									  chosenDuration == s.Duration;


				return matchesName && matchesPrice && matchesType && matchesDuration;
			}).ToList();

			CreateSubscriptionsPanel(filteredSubscriptions);
		}

		private void FilterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ApplyFilters();
		}

		private void FilterDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ApplyFilters();
		}

		private void SearchSubscription_TextChanged(object sender, TextChangedEventArgs e)
		{
			ApplyFilters();
		}

		private void MinCost_TextChanged(object sender, TextChangedEventArgs e)
		{
			ApplyFilters();
		}

		private void MaxCost_TextChanged(object sender, TextChangedEventArgs e)
		{
			ApplyFilters();
		}

		private void ManipulateFilterPanel_Click(object sender, RoutedEventArgs e)
		{
			if (FilterPanelVisibility == Visibility.Visible)
			{
				FilterPanelVisibility = Visibility.Collapsed;
				ManipulateFilterPanel.Content = "▶";
			}
			else
			{
				FilterPanelVisibility = Visibility.Visible;
				ManipulateFilterPanel.Content = "◀";
			}
		}

		private void PersonalAccountButon_Click(object sender, RoutedEventArgs e)
		{
			Windows.PersonalAccount personalAccount = new Windows.PersonalAccount(_user);
			personalAccount.ShowDialog();
		}

		private void UndoButton_Click(object sender, RoutedEventArgs e)
		{
			UndoManager.Undo();
		}

		private void RedoButton_Click(object sender, RoutedEventArgs e)
		{
			UndoManager.Redo();
		}
		public void UpdateUIWithSubscriptions(List<Subscription> updatedSubscriptions)
		{
			subscriptions = updatedSubscriptions;
			CreateSubscriptionsPanel(subscriptions);
			SaveSubscriptions();
		}

		private void OnThemeChanged(object sender, ThemeManager.AppTheme e)
		{
			CreateSubscriptionsPanel(subscriptions);
		}
		
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			ThemeManager.Instance.ThemeChanged -= OnThemeChanged;
		}
		private void Statistics_Click(object sender, RoutedEventArgs e)
		{
			ControlTestWindow testWindow = new ControlTestWindow();
			testWindow.Owner = this;
			testWindow.ShowDialog();
		}
        private void LogoutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите выйти из системы?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
    
                SaveSubscriptions();
                Registration registrationWindow = new Registration();
                registrationWindow.Show();
                
                Close();
            }
        }

        private void LogoutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

		private void Control_Preview(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show(
				$"ТУННЕЛЬНАЯ маршрутизация\n" +
				$"Событие идёт сверху вниз\n\n" +
				$"Sender: {((FrameworkElement)sender).Name}\n" +
				$"Source: {((FrameworkElement)e.Source).Name}",
				"PreviewMouseLeftButtonDown",
				MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void Control_MouseDown(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show(
				$"ПУЗЫРЬКОВАЯ маршрутизация\n" +
				$"Событие идёт снизу вверх\n\n" +
				$"Sender: {((FrameworkElement)sender).Name}\n" +
				$"Source: {((FrameworkElement)e.Source).Name}",
				"MouseLeftButtonDown",
				MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void HomeButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show(
				$"ПРЯМАЯ маршрутизация\n" +
				$"Событие произошло ТОЛЬКО в {((FrameworkElement)sender).Name}",
				"TextChanged (Direct Event)",
				MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
