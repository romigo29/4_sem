using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_FitnessClub.Models;
using static WPF_FitnessClub.Models.Subscription;
using static WPF_FitnessClub.Commands;
using WPF_FitnessClub.Data;

namespace WPF_FitnessClub.ViewModels
{
	public class AddSubscriptionVM : ViewModelBase
	{
		private string _name;
		private string _description;
		private string _price;
		private string _imagePath;
		private string _duration;
		private string _subscriptionType;
		private DataAccess dataAccess;


		#region Свойства для привязки данных
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
				CommandManager.InvalidateRequerySuggested();
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				OnPropertyChanged(nameof(Description));
			}
		}

		public string Price
		{
			get { return _price; }
			set
			{
				_price = value;
				OnPropertyChanged(nameof(Price));
				CommandManager.InvalidateRequerySuggested();
			}
		}

		public string ImagePath
		{
			get { return _imagePath; }
			set
			{
				_imagePath = value;
				OnPropertyChanged(nameof(ImagePath));
				CommandManager.InvalidateRequerySuggested();
			}
		}

		public string Duration
		{
			get { return _duration; }
			set
			{
				_duration = value;
				OnPropertyChanged(nameof(Duration));
				CommandManager.InvalidateRequerySuggested();
			}
		}

		public string SubscriptionType
		{
			get { return _subscriptionType; }
			set
			{
				_subscriptionType = value;
				OnPropertyChanged(nameof(SubscriptionType));
				CommandManager.InvalidateRequerySuggested();
			}
		}

		#endregion


		#region Команды
		public ICommand SaveCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }
		public ICommand SelectImageCommand { get; private set; }

		#endregion

		public Subscription NewSubscription { get; private set; }

		public event Action<bool, Subscription> CloseRequested;

		public AddSubscriptionVM()
		{

			dataAccess = new DataAccess();

			SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
			CancelCommand = new RelayCommand(ExecuteCancelCommand);
			SelectImageCommand = new RelayCommand(ExecuteSelectImageCommand);
		}

		public bool CanExecuteSaveCommand(object parameter)
		{
			return !string.IsNullOrEmpty(Name) &&
				   !string.IsNullOrEmpty(Price) &&
				   !string.IsNullOrEmpty(Description) &&
				   !string.IsNullOrEmpty(ImagePath) &&
				   !string.IsNullOrEmpty(Duration) &&
				   !string.IsNullOrEmpty(SubscriptionType);
		}

		public void ExecuteSaveCommand(object parameter)
		{
			try
			{
				// Валидация имени
				string namePattern = @"^[a-zA-Zа-яА-Я\s]+$";
				if (!Regex.IsMatch(Name, namePattern))
				{
					MessageBox.Show(
						(string)Application.Current.Resources["InvalidName"],
						(string)Application.Current.Resources["ValidationErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Warning);
					return;
				}

				// Валидация цены
				if (!double.TryParse(Price, out double priceValue) || priceValue < 0)
				{
					MessageBox.Show(
						(string)Application.Current.Resources["InvalidPrice"],
						(string)Application.Current.Resources["ValidationErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Warning);
					return;
				}

				// Валидация описания
				if (string.IsNullOrEmpty(Description?.Trim()))
				{
					MessageBox.Show(
						(string)Application.Current.Resources["EnterDescription"],
						(string)Application.Current.Resources["ValidationErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Warning);
					return;
				}

				// Создаем новый объект подписки
				NewSubscription = new Subscription(
					Name.Trim(),
					priceValue,
					Description.Trim(),
					ImagePath,
					Duration,
					SubscriptionType,
					new List<Review>()
				);

				MessageBox.Show(
					(string)Application.Current.Resources["SubscriptionAddedSuccess"],
					(string)Application.Current.Resources["SuccessTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				CloseRequested?.Invoke(true, NewSubscription);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format((string)Application.Current.Resources["ErrorSavingSubscription"], ex.Message),
					(string)Application.Current.Resources["ErrorTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}

		private void ExecuteCancelCommand(object obj)
		{
			CloseRequested?.Invoke(false, null);
		}

		private void ExecuteSelectImageCommand(object obj)
		{
			var openFileDialog = new System.Windows.Forms.OpenFileDialog
			{
				Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*"
			};

			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string selectedPath = openFileDialog.FileName;
				string fileName = System.IO.Path.GetFileName(selectedPath);
				string relativePath = $"/Images/{fileName}";
				
				ImagePath = relativePath;
			}
		}
	}
}
