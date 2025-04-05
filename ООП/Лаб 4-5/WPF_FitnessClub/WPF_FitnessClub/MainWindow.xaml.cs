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

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public readonly UserRole currentUserRole;
		readonly string filePath = "subscriptions.js";
		List<Subscription> subscriptions = new List<Subscription>();
		List<Subscription> filteredSubscriptions = new List<Subscription>();

		public MainWindow(UserRole role)
		{
			currentUserRole = role;
			InitializeComponent();
			LoadSubscriptions();
			CreateSubscriptionsPanel(subscriptions);
			ConfigureUIBasedOnRole();
			
			LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
		}

		private void ConfigureUIBasedOnRole()
		{
			switch (currentUserRole)
			{
				case UserRole.Admin:
					AddSubscriptionButon.Visibility = Visibility.Visible;
			
					break;
				case UserRole.Coach:
		
					AddSubscriptionButon.Visibility = Visibility.Collapsed;
	
					break;
				case UserRole.Client:

					AddSubscriptionButon.Visibility = Visibility.Collapsed;
					break;
			}
		}

		private void LoadSubscriptions()
		{
			if (File.Exists(filePath))
			{
				try
				{
					string json = File.ReadAllText(filePath);
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
					File.WriteAllText(filePath, json);

			}

			catch (Exception ex)
            {
				MessageBox.Show(string.Format((string)Application.Current.Resources["ErrorSavingData"], ex.Message));
			}
		}

		public void CreateSubscriptionsPanel(List<Subscription> _subscriptions)
		{
			SubscriptionsPanel.Children.Clear();

			const double CARD_WIDTH = 360;
			const double CARD_HEIGHT = 500;

			foreach (Subscription subscription in _subscriptions)
			{
				StackPanel subscriptionCard = new StackPanel
				{
					Width = CARD_WIDTH,
					Height = CARD_HEIGHT,
					Margin = new Thickness(5)
				};

				Image subscriptionImage = new Image
				{
					Stretch = Stretch.UniformToFill,
					Width = CARD_WIDTH - 20,
					Height = CARD_HEIGHT * 0.5,
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
					Margin = new Thickness(5)
				};

				TextBlock subscriptionPrice = new TextBlock
				{
					Text = $"{string.Format((string)Application.Current.Resources["Price"])}: {subscription.Price:C}",
					Margin = new Thickness(5)
				};

				TextBlock subscriptionDescription = new TextBlock
				{
					Text = subscription.Description,
					TextWrapping = TextWrapping.Wrap,
					Margin = new Thickness(5)
				};

				StackPanel ratingPanel = new StackPanel
				{
					Margin = new Thickness(5),
					Orientation = Orientation.Horizontal,
					HorizontalAlignment	= HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Center

				};

				TextBlock ratingText = new TextBlock
				{
					Text = subscription.Rating.ToString()
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

				subscriptions.Add(newSubscription);
				SaveSubscriptions();
				CreateSubscriptionsPanel(subscriptions);
			}
		}

		private void OpenSubscriptionDetails(Subscription subscription)
		{
			var detailsWindow = new SubscriptionDetails(this, subscriptions, subscription, currentUserRole);
			detailsWindow.ShowDialog();
		}

		public void UpdateSubscriptions()
		{
			SaveSubscriptions();
			CreateSubscriptionsPanel(subscriptions);
		}

		private void OnLanguageChanged(object sender, string e)
		{
			// Обновляем все тексты в окне
			UpdateAllTexts();
		}

		private void UpdateAllTexts()
		{
			// Обновляем заголовки и тексты в зависимости от выбранного языка
			// Например:
			// Title = (string)Application.Current.Resources["MainWindowTitle"];
			// btnAddSubscription.Content = (string)Application.Current.Resources["AddSubscriptionButton"];
			// и т.д.
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
			if (SearchFilterPanel.Visibility == Visibility.Visible)
			{
				SearchFilterPanel.Visibility = Visibility.Collapsed;
				ManipulateFilterPanel.Content = string.Format((string)Application.Current.Resources["ShowPanelButton"]);

			}

			else if (SearchFilterPanel.Visibility == Visibility.Collapsed)
			{
				SearchFilterPanel.Visibility = Visibility.Visible;
				ManipulateFilterPanel.Content = "HidePanelButton";
			}
		}
	}
}
