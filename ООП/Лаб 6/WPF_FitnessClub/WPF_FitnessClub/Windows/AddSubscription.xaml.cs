using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
	/// Логика взаимодействия для AddSubscription.xaml
	/// </summary>
	public partial class AddSubscription : Window
	{
		public Subscription NewSubscription { get; set; }
		
		string NewImagePath { get; set; }

		public ICommand SaveCommand { get; private set; }
		public ICommand CancelCommand {  get; private set; }

		public ICommand SelectImageCommand { get; private set; }

		public AddSubscription()
		{
			InitializeComponent();
			NewSubscription = new Subscription();
			SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
			CancelCommand = new RelayCommand(ExecuteCancelCommand);
			SelectImageCommand = new RelayCommand(ExceuteSelectImageCommand);
			DataContext = this;
		}

		private bool CanExecuteSaveCommand(object parameter)
		{

			return !string.IsNullOrEmpty(InputName.Text) &&
           !string.IsNullOrEmpty(InputPrice.Text) &&
           !string.IsNullOrEmpty(InputDescription.Text) &&
           !string.IsNullOrEmpty(NewImagePath) &&  // проверка выбора изображения
           !string.IsNullOrEmpty(ChooseDuration.Text) &&         // проверка длительности
           !string.IsNullOrEmpty(ChooseType.Text);              // проверка типа абонемента
}

		private void ExecuteSaveCommand(object parameter)
		{
			try
			{

				string namePattern = @"^[a-zA-Zа-яА-Я\s]+$";
				string _name = InputName.Text.Trim();
				if (!Regex.IsMatch(_name, namePattern))
				{
					MessageBox.Show(
						(string)Application.Current.Resources["InvalidName"],
						(string)Application.Current.Resources["ValidationErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Warning);
					return;
				}

				if(!double.TryParse(InputPrice.Text, out double _price) || _price < 0)
				{
					MessageBox.Show(
						(string)Application.Current.Resources["InvalidPrice"],
						(string)Application.Current.Resources["ValidationErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Warning);
					return;
				}

				string _description = InputDescription.Text.Trim();
				if (string.IsNullOrEmpty(_description))
				{
					MessageBox.Show(
						(string)Application.Current.Resources["EnterDescription"],
						(string)Application.Current.Resources["ValidationErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Warning);
					return;
				}

				string selectedDuration = "";
				if (ChooseType.SelectedItem is ComboBoxItem selectedItem)
				{
					if (selectedItem.Content is string contentString)
					{
						selectedDuration = contentString;
					}
				}

				string selectedType = "";
				if (ChooseType.SelectedItem is ComboBoxItem selectedItem2)
				{
					if (selectedItem2.Content is string contentString)
					{
						selectedType = contentString;
					}
				}


				// Создаем новый объект подписки
				NewSubscription = new Subscription(
					_name,
					double.Parse(InputPrice.Text),
					InputDescription.Text,
					NewImagePath,
					ChooseDuration.Text,
					ChooseType.Text,
					new List<Review>()
				);

				DialogResult = true;
				Close();

				MessageBox.Show(
					(string)Application.Current.Resources["SubscriptionAddedSuccess"], 
					(string)Application.Current.Resources["SuccessTitle"], 
					MessageBoxButton.OK, 
					MessageBoxImage.Information);
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
			Close();
		}

		private void ExceuteSelectImageCommand(object obj)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*"
			};

			if (openFileDialog.ShowDialog() == true)
			{
				string selectedPath = openFileDialog.FileName; 
				string fileName = System.IO.Path.GetFileName(selectedPath);

				string relativePath = $"/Images/{fileName}";

				NewImagePath = relativePath; 
			}

		}


	}
}
