using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static WPF_FitnessClub.Commands;

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для SubscriptionDetails.xaml
	/// </summary>
	public partial class SubscriptionDetails : Window, INotifyPropertyChanged
	{
		private MainWindow _mainWindow;
		private Subscription currentSubscription;
		private List<Subscription> subscriptions;
		UserRole currentUserRole;

		private string _name;
		private string _imagePath;
		private double _price;
		private string _type;
		private string _duration;
		private string _description;
		private bool _isEditMode;

		public Visibility ViewModeVisible => IsEditMode ? Visibility.Collapsed : Visibility.Visible;
		public Visibility EditModeVisible => IsEditMode ? Visibility.Visible : Visibility.Collapsed;

		public Visibility DeleteReviewVisible => currentUserRole == UserRole.Admin ? Visibility.Visible : Visibility.Collapsed;

		public Visibility EditPermissionsVisibility => currentUserRole != UserRole.Client ? Visibility.Visible : Visibility.Collapsed;
		public Visibility WriteReviewVisible => currentUserRole == UserRole.Client? Visibility.Visible : Visibility.Collapsed;

		public ICommand ChooseImageCommand {  get; private set; }
		public ICommand EditCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand DeleteCommand {  get; private set; }
		public ICommand CancelCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }
		public ICommand DeleteReviewCommand {  get; private set; }

		public ICommand AddReviewCommand { get; private set; }

		public string SubscrName
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged();
			}
		}

		public string ImagePath
		{
			get { return _imagePath; }
			set		
			{
				_imagePath = value;
				OnPropertyChanged();
			}
		}

		public double Price
		{
			get { return _price; }
			set
			{
				_price = value;
				OnPropertyChanged();
			}
		}
	
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				OnPropertyChanged();
			}
		}

		public string Duration
		{
			get { return _duration; }
			set
			{
				_duration = value;
				OnPropertyChanged();
			}
		}

		public string Type
		{
			get { return _type; }
			set
			{
				_type = value;
				OnPropertyChanged();
			}
		}

		public bool IsEditMode
		{
			get { return _isEditMode; }
			set
			{
				_isEditMode = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ViewModeVisible));
				OnPropertyChanged(nameof(EditModeVisible));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public SubscriptionDetails(MainWindow mainWindow, List<Subscription> subscriptions, Subscription subscription, UserRole role)
		{
			InitializeComponent();

			_mainWindow = mainWindow;
			this.subscriptions = subscriptions;
			currentSubscription = subscription;
			currentUserRole = role;

			ChooseImageCommand = new RelayCommand(ExecuteChooseImage);
			EditCommand = new RelayCommand(ExecuteEdit);
			SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
			DeleteCommand = new RelayCommand(ExecuteDelete);
			CancelCommand = new RelayCommand(ExecuteCancel);
			CloseCommand = new RelayCommand(ExecuteClose);
			DeleteReviewCommand = new RelayCommand(ExecuteDeleteReview);
			AddReviewCommand = new RelayCommand(ExecuteAddReview);

			DataContext = this;
			IsEditMode = false; // По умолчанию режим просмотра
			LoadDetails();
			LoadReviews();

			this.Title = string.Format((string)Application.Current.Resources["SubscriptionDetailsFormat"], subscription.Name);
		}

		private void ExecuteEdit(object parameter)
		{
			IsEditMode = true;
		}

		private void ExecuteSave(object parameter)
		{
			try
			{
				// Сохраняем изменения в текущем объекте
				currentSubscription.Name = SubscrName;
				
				// Обработка пути к изображению при сохранении
				if (!string.IsNullOrEmpty(ImagePath))
				{
					// Если путь начинается с "/Images/", сохраняем только имя файла
					if (ImagePath.StartsWith("/Images/"))
					{
						currentSubscription.ImagePath = ImagePath;
					}
					else
					{
						// Если путь не начинается с "/Images/", добавляем префикс
						currentSubscription.ImagePath = "/Images/" + System.IO.Path.GetFileName(ImagePath);
					}
				}
				else
				{
					currentSubscription.ImagePath = null;
				}
				
				currentSubscription.Price = Price;
				currentSubscription.SubscriptionType = Type;
				currentSubscription.Duration = Duration;
				currentSubscription.Description = Description;

				this.Title = string.Format((string)Application.Current.Resources["SubscriptionDetailsFormat"], currentSubscription.Name);

				MessageBox.Show(
					(string)Application.Current.Resources["SavedSuccessfully"],
					(string)Application.Current.Resources["SavedTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				IsEditMode = false;

				_mainWindow?.UpdateSubscriptions();
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format((string)Application.Current.Resources["ErrorSaving"], ex.Message),
					(string)Application.Current.Resources["ErrorTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}

		private bool CanExecuteSave(object parameter)
		{
			return !string.IsNullOrWhiteSpace(SubscrName);
		}

		private void ExecuteDelete(object parameter)
		{
			var answer = MessageBox.Show(
				(string)Application.Current.Resources["DeleteSubscriptionConfirm"],
				(string)Application.Current.Resources["DeleteConfirmTitle"],
				MessageBoxButton.YesNo,
				MessageBoxImage.Question);

			if (answer is MessageBoxResult.Yes)
			{
				subscriptions.Remove(currentSubscription);
				LoadReviews();
				_isEditMode = false;
				_mainWindow.CreateSubscriptionsPanel(subscriptions);
				_mainWindow.SaveSubscriptions();
				MessageBox.Show((string)Application.Current.Resources["SubscriptionDeleted"]);
				Close();
			}
		}

		private void ExecuteCancel(object parameter)
		{
			SubscrName = currentSubscription.Name;
			ImagePath = currentSubscription.ImagePath;
			Price = currentSubscription.Price;
			Type = currentSubscription.SubscriptionType;
			Duration = currentSubscription.Duration;
			Description = currentSubscription.Description;

			IsEditMode = false;
		}

		private void ExecuteClose(object parameter)
		{
			this.Close();
		}

		private void ExecuteDeleteReview(object parameter)
		{
			var reviewToDelte = parameter as Review;

			if (reviewToDelte == null)
			{
				return;
			}

			var answer = MessageBox.Show(
				(string)Application.Current.Resources["DeleteReviewConfirm"],
				(string)Application.Current.Resources["DeleteConfirmTitle"],
				MessageBoxButton.YesNo,
				MessageBoxImage.Question);

			if (answer is MessageBoxResult.Yes)
			{
				currentSubscription.Reviews.Remove(reviewToDelte);
				currentSubscription.Rating = currentSubscription.CalculateRating();
				LoadReviews();
				_mainWindow.CreateSubscriptionsPanel(subscriptions);
				_mainWindow.SaveSubscriptions();
			}
		}

		private void ExecuteAddReview(object sender)
		{
			string name = ReviewNameBox.Text;
			string namePattern = @"^[a-zA-Zа-яА-Я\s]+$";

			if(string.IsNullOrEmpty(name))
			{
				MessageBox.Show(
					(string)Application.Current.Resources["PleaseEnterName"],
					(string)Application.Current.Resources["ValidationErrorTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
			}

			bool isRatingSelected = RatingRadioBox.Children.OfType<RadioButton>()
				.Any(rb => rb.IsChecked == true);

			if (!isRatingSelected)
			{
				MessageBox.Show(
					(string)Application.Current.Resources["PleaseRateSubscription"],
					(string)Application.Current.Resources["ValidationErrorTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
			}

			if (!Regex.IsMatch(name, namePattern))
			{
				MessageBox.Show(
					(string)Application.Current.Resources["PleaseEnterValidName"],
					(string)Application.Current.Resources["ValidationErrorTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
			}

			int score = RatingRadioBox.Children.OfType<RadioButton>()
						.Where(r => r.IsChecked == true)
						.Select(r => Convert.ToInt32(r.Content))
						.FirstOrDefault();
			string comment = CommentBox.Text;

			Review _review = new Review(name, score, comment);

			currentSubscription.Reviews.Add(_review);
			currentSubscription.Rating = currentSubscription.CalculateRating();
			LoadDetails();
			LoadReviews();
			_mainWindow.UpdateSubscriptions();
			_mainWindow.SaveSubscriptions();
		}

		private void LoadDetails()
		{
			SubscrName = currentSubscription.Name;
			
			// Форматируем путь к изображению так же, как в MainWindow
			string imagePath = currentSubscription.ImagePath;
			if (!string.IsNullOrEmpty(imagePath))
			{
				// Проверяем, начинается ли путь с "/", если нет - добавляем префикс "/Images/"
				ImagePath = imagePath.StartsWith("/") 
					? imagePath 
					: "/Images/" + imagePath;
			}
			else
			{
				ImagePath = null;
			}
			
			Price = currentSubscription.Price;
			Type = currentSubscription.SubscriptionType;
			Duration = currentSubscription.Duration;
			Description = currentSubscription.Description;
		}

		private void LoadReviews()
		{
			ReviewWrapPanel.Children.Clear();

			foreach(var review in currentSubscription.Reviews)
			{
				var border = new Border
				{
					BorderBrush = Brushes.LightGray,
					BorderThickness = new Thickness(1),
					CornerRadius = new CornerRadius(8),
					Margin = new Thickness(10),
					Padding = new Thickness(10),
					MaxWidth = 300,
					MinWidth = 100
				};

				var stackPanel = new StackPanel();

				var userNameTextBlock = new TextBlock
				{
					Text = review.User,
					FontWeight = FontWeights.Bold,
					Margin = new Thickness(0, 0, 0, 5)
				};
				stackPanel.Children.Add(userNameTextBlock);

				var ratingPanel = new StackPanel
				{
					Orientation = Orientation.Horizontal,
					Margin = new Thickness(0, 0, 0, 5)
				};


				for (int i = 0; i < 5; i++)
				{
					var star = new TextBlock
					{
						Text = i < review.Score ? "★" : "☆",
						FontSize = 16,
						Foreground = i < review.Score ? Brushes.Gold : Brushes.Gray
					};
					ratingPanel.Children.Add(star);
				}

				stackPanel.Children.Add(ratingPanel);
				

				var commentTextBlock = new TextBlock
				{
					Text = review.Comment,
					TextWrapping = TextWrapping.Wrap,
					Margin = new Thickness(0, 0, 0, 5)
				};
				stackPanel.Children.Add(commentTextBlock);

				var deleteReviewButton = new Button
				{
    				Content = (string)Application.Current.Resources["DeleteReviewButton"],
    				Command = DeleteReviewCommand,
    				CommandParameter = review, // Добавляем параметр команды
    				Visibility = DeleteReviewVisible,
					HorizontalAlignment = HorizontalAlignment.Center, // Изменяем выравнивание
					Margin = new Thickness(5,0,0,0) // Добавляем отступ слева
				};
				// Применяем стиль из ресурсов приложения
				deleteReviewButton.Style = Application.Current.Resources["DefaultButton"] as Style;
				stackPanel.Children.Add(deleteReviewButton);

				border.Child = stackPanel;
				ReviewWrapPanel.Children.Add(border);
			}
		}

		private void ExecuteChooseImage(object sender)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
				Title = (string)Application.Current.Resources["SelectImageTitle"]
			};

			if (openFileDialog.ShowDialog() == true)
			{
				string selectedPath = openFileDialog.FileName;
				string fileName = System.IO.Path.GetFileName(selectedPath);
				
				// Копируем файл в папку Images
				try
				{
					string appDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
					string imagesDirectory = System.IO.Path.Combine(appDirectory, "Images");
					
					// Создаем директорию, если она не существует
					if (!System.IO.Directory.Exists(imagesDirectory))
					{
						System.IO.Directory.CreateDirectory(imagesDirectory);
					}
					
					string targetPath = System.IO.Path.Combine(imagesDirectory, fileName);
					if (!System.IO.File.Exists(targetPath))
					{
						System.IO.File.Copy(selectedPath, targetPath);
					}
					
					// Используем относительный путь для WPF
					ImagePath = "/Images/" + fileName;
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						"Ошибка при копировании изображения: " + ex.Message, 
						"Ошибка", 
						MessageBoxButton.OK, 
						MessageBoxImage.Error);
					
					// Если копирование не удалось, всё равно пытаемся установить путь
					ImagePath = "/Images/" + fileName;
				}
			}
		}
	}
}

