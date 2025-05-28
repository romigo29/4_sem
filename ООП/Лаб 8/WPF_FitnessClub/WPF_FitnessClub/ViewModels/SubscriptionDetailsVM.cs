using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_FitnessClub.Models;
using static WPF_FitnessClub.Commands;

namespace WPF_FitnessClub.ViewModels
{
	public class SubscriptionDetailsVM : ViewModelBase
	{
		#region Приватные поля
		private Subscription _currentSubscription;
		private ObservableCollection<Subscription> _subscriptions;
		private UndoManager _undoManager;
		private UserRole _currentUserRole;
		private MainWindow _mainWindow;

		private string _name;
		private string _imagePath;
		private double _price;
		private string _type;
		private string _duration;
		private string _description;
		private bool _isEditMode;
		private string _reviewName;
		private string _reviewComment;
		private int _reviewRating;
		private ObservableCollection<Review> _reviews;
		#endregion

		#region Свойства
		public string SubscrName
		{
			get => _name;
			set
			{
				_name = value;
				OnPropertyChanged("SubscrName");
			}
		}

		public string ImagePath
		{
			get => _imagePath;
			set
			{
				_imagePath = value;
				OnPropertyChanged("ImagePath");
			}
		}

		public double Price
		{
			get => _price;
			set
			{
				_price = value;
				OnPropertyChanged("Price");
			}
		}

		public string Description
		{
			get => _description;
			set
			{
				_description = value;
				OnPropertyChanged("Description");
			}
		}

		public string Duration
		{
			get => _duration;
			set
			{
				_duration = value;
				OnPropertyChanged("Duration");
			}
		}

		public string Type
		{
			get => _type;
			set
			{
				_type = value;
				OnPropertyChanged("Type");
			}
		}

		public bool IsEditMode
		{
			get => _isEditMode;
			set
			{
				_isEditMode = value;
				OnPropertyChanged("IsEditMode");
				OnPropertyChanged(nameof(ViewModeVisible));
				OnPropertyChanged(nameof(EditModeVisible));
			}
		}

		public string ReviewName
		{
			get => _reviewName;
			set
			{
				_reviewName = value;
				OnPropertyChanged("ReviewName");
			}
		}

		public string ReviewComment
		{
			get => _reviewComment;
			set
			{
				_reviewComment = value;
				OnPropertyChanged("ReviewComment");
			}
		}

		public int ReviewRating
		{
			get => _reviewRating;
			set
			{
				_reviewRating = value;
				OnPropertyChanged("ReviewRating");
			}
		}

		public ObservableCollection<Review> Reviews
		{
			get => _reviews;
			set
			{
				_reviews = value;
				OnPropertyChanged("Reviews");
			}
		}

		public Visibility ViewModeVisible => IsEditMode ? Visibility.Collapsed : Visibility.Visible;
		public Visibility EditModeVisible => IsEditMode ? Visibility.Visible : Visibility.Collapsed;
		public Visibility DeleteReviewVisible => _currentUserRole == UserRole.Admin ? Visibility.Visible : Visibility.Collapsed;
		public Visibility WriteReviewVisible => _currentUserRole == UserRole.Client ? Visibility.Visible : Visibility.Collapsed;
		#endregion

		#region Команды
		public ICommand ChooseImageCommand { get; private set; }
		public ICommand EditCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand DeleteCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }
		public ICommand DeleteReviewCommand { get; private set; }
		public ICommand AddReviewCommand { get; private set; }
		#endregion

		#region События
		public event EventHandler RequestClose;
		public event EventHandler<Review> ReviewAdded;
		public event EventHandler<Review> ReviewDeleted;
		#endregion

		#region Конструктор
		public SubscriptionDetailsVM(MainWindow mainWindow, List<Subscription> subscriptions, Subscription subscription, UserRole role, UndoManager undoManager)
		{
			_mainWindow = mainWindow;
			_subscriptions = new ObservableCollection<Subscription>(subscriptions);
			_currentSubscription = subscription;
			_currentUserRole = role;
			_undoManager = undoManager;
			_reviews = new ObservableCollection<Review>(subscription.Reviews ?? new List<Review>());

			// Инициализация команд
			ChooseImageCommand = new RelayCommand(ExecuteChooseImage);
			EditCommand = new RelayCommand(ExecuteEdit);
			SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
			DeleteCommand = new RelayCommand(ExecuteDelete);
			CancelCommand = new RelayCommand(ExecuteCancel);
			CloseCommand = new RelayCommand(ExecuteClose);
			DeleteReviewCommand = new RelayCommand(ExecuteDeleteReview);
			AddReviewCommand = new RelayCommand(ExecuteAddReview, CanExecuteAddReview);

			// Загрузка данных
			LoadDetails();
		}
		#endregion

		#region Методы команд
		private void ExecuteEdit(object parameter)
		{
			IsEditMode = true;
		}

		private void ExecuteSave(object parameter)
		{
			try
			{
				// Создаем копию объекта для сохранения оригинальных данных
				Subscription originalSubscription = new Subscription(
					_currentSubscription.Name,
					_currentSubscription.Price,
					_currentSubscription.Description,
					_currentSubscription.ImagePath,
					_currentSubscription.Duration,
					_currentSubscription.SubscriptionType,
					new List<Review>(_currentSubscription.Reviews ?? new List<Review>())
				);
				originalSubscription.Id = _currentSubscription.Id;
				
				// Сохраняем изменения в текущем объекте
				_currentSubscription.Name = SubscrName;

				// Обработка пути к изображению при сохранении
				if (!string.IsNullOrEmpty(ImagePath))
				{
					// Если путь начинается с "/Images/", сохраняем только имя файла
					if (ImagePath.StartsWith("/Images/"))
					{
						_currentSubscription.ImagePath = ImagePath;
					}
					else
					{
						// Если путь не начинается с "/Images/", добавляем префикс
						_currentSubscription.ImagePath = "/Images/" + System.IO.Path.GetFileName(ImagePath);
					}
				}
				else
				{
					_currentSubscription.ImagePath = null;
				}

				_currentSubscription.Price = Price;
				_currentSubscription.SubscriptionType = Type;
				_currentSubscription.Duration = Duration;
				_currentSubscription.Description = Description;
				
				// Создаем и выполняем команду редактирования через UndoManager
				var cmd = new EditSubscriptionCommand(_subscriptions, _currentSubscription, originalSubscription);
				_undoManager.ExecuteAction(cmd);

				MessageBox.Show(
					(string)Application.Current.Resources["SavedSuccessfully"],
					(string)Application.Current.Resources["SavedTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				IsEditMode = false;
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

			if (answer == MessageBoxResult.Yes)
			{
				// Создаем команду удаления и выполняем ее через UndoManager
				var cmd = new DeleteSubscriptionCommand(_subscriptions, _currentSubscription);
				_undoManager.ExecuteAction(cmd);
				
				MessageBox.Show((string)Application.Current.Resources["SubscriptionDeleted"]);
				CloseWindow();
			}
		}

		private void ExecuteCancel(object parameter)
		{
			// Отменяем изменения, восстанавливая данные из объекта подписки
			LoadDetails();
			IsEditMode = false;
		}

		private void ExecuteClose(object parameter)
		{
			if (IsEditMode)
			{
				var result = MessageBox.Show(
					(string)Application.Current.Resources["UnsavedChangesMessage"],
					(string)Application.Current.Resources["WarningTitle"],
					MessageBoxButton.YesNoCancel,
					MessageBoxImage.Warning);

				switch (result)
				{
					case MessageBoxResult.Yes:
						if (CanExecuteSave(null))
						{
							ExecuteSave(null);
							CloseWindow();
						}
						break;
					case MessageBoxResult.No:
						CloseWindow();
						break;
					case MessageBoxResult.Cancel:
						return;
				}
			}
			else
			{
				CloseWindow();
			}
		}

		private void CloseWindow()
		{
			RequestClose?.Invoke(this, EventArgs.Empty);
		}

		private void ExecuteDeleteReview(object parameter)
		{
			if (parameter is Review review)
			{
				var result = MessageBox.Show(
					(string)Application.Current.Resources["DeleteReviewConfirm"],
					(string)Application.Current.Resources["DeleteConfirmTitle"],
					MessageBoxButton.YesNo,
					MessageBoxImage.Question);

				if (result == MessageBoxResult.Yes)
				{
					// Сохраняем текущее состояние для возможности отмены
					Subscription originalState = new Subscription(
						_currentSubscription.Name,
						_currentSubscription.Price,
						_currentSubscription.Description,
						_currentSubscription.ImagePath,
						_currentSubscription.Duration,
						_currentSubscription.SubscriptionType,
						new List<Review>(_currentSubscription.Reviews ?? new List<Review>())
					);
					originalState.Id = _currentSubscription.Id;
					
					// Удаляем отзыв
					_currentSubscription.Reviews.Remove(review);
					_currentSubscription.Rating = _currentSubscription.CalculateRating();
					Reviews = new ObservableCollection<Review>(_currentSubscription.Reviews);
					
					// Создаем и выполняем команду редактирования через UndoManager
					var cmd = new EditSubscriptionCommand(_subscriptions, _currentSubscription, originalState);
					_undoManager.ExecuteAction(cmd);
					
					ReviewDeleted?.Invoke(this, review);
				}
			}
		}

		private bool CanExecuteAddReview(object parameter)
		{
			return !string.IsNullOrWhiteSpace(ReviewName) && 
				  !string.IsNullOrWhiteSpace(ReviewComment) && 
				  ReviewRating > 0;
		}

		private void ExecuteAddReview(object parameter)
		{
			// Сохраняем текущее состояние для возможности отмены
			Subscription originalState = new Subscription(
				_currentSubscription.Name,
				_currentSubscription.Price,
				_currentSubscription.Description,
				_currentSubscription.ImagePath,
				_currentSubscription.Duration,
				_currentSubscription.SubscriptionType,
				new List<Review>(_currentSubscription.Reviews ?? new List<Review>())
			);
			originalState.Id = _currentSubscription.Id;
			
			// Добавляем новый отзыв
			var newReview = new Review(ReviewName, ReviewRating, ReviewComment);
			_currentSubscription.Reviews.Add(newReview);
			_currentSubscription.Rating = _currentSubscription.CalculateRating();
			
			Reviews = new ObservableCollection<Review>(_currentSubscription.Reviews);
			
			// Создаем и выполняем команду редактирования через UndoManager
			var cmd = new EditSubscriptionCommand(_subscriptions, _currentSubscription, originalState);
			_undoManager.ExecuteAction(cmd);
			
			// Очистка полей
			ReviewName = string.Empty;
			ReviewComment = string.Empty;
			ReviewRating = 0;
			
			ReviewAdded?.Invoke(this, newReview);
		}

		private void ExecuteChooseImage(object parameter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*"
			};

			if (openFileDialog.ShowDialog() == true)
			{
				ImagePath = openFileDialog.FileName;
			}
		}
		#endregion

		#region Вспомогательные методы
		private void LoadDetails()
		{
			SubscrName = _currentSubscription.Name;
			ImagePath = _currentSubscription.ImagePath;
			Price = _currentSubscription.Price;
			Type = _currentSubscription.SubscriptionType;
			Duration = _currentSubscription.Duration;
			Description = _currentSubscription.Description;
		}
		#endregion
	}
}
