using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.ViewModels;
using static System.Globalization.CultureInfo;

namespace WPF_FitnessClub.View
{
	/// <summary>
	/// Логика взаимодействия для SubscriptionDetailsView.xaml
	/// </summary>
	public partial class SubscriptionDetailsView : Window
	{
		private SubscriptionDetailsVM _viewModel;

		public SubscriptionDetailsView(MainWindow mainWindow, List<Subscription> subscriptions, Subscription subscription, UserRole role, UndoManager undoManager)
		{
			InitializeComponent();

			_viewModel = new SubscriptionDetailsVM(mainWindow, subscriptions, subscription, role, undoManager);
			
			this.Title = string.Format((string)Application.Current.Resources["SubscriptionDetailsFormat"], subscription.Name);
			
			_viewModel.RequestClose += ViewModel_RequestClose;
			_viewModel.ReviewAdded += ViewModel_ReviewAdded;
			_viewModel.ReviewDeleted += ViewModel_ReviewDeleted;

			DataContext = _viewModel;
			
			ThemeManager.Instance.ThemeChanged += OnThemeChanged;
			
			LoadReviews();
		}

		private void ViewModel_RequestClose(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ViewModel_ReviewAdded(object sender, Review e)
		{
			LoadReviews();
		}

		private void ViewModel_ReviewDeleted(object sender, Review e)
		{
			LoadReviews();
		}

		private void LoadReviews()
		{
			ReviewWrapPanel.Children.Clear();
			
			foreach (var review in _viewModel.Reviews)
			{
				Border reviewBorder = new Border
				{
					BorderBrush = new SolidColorBrush(Colors.Gray),
					BorderThickness = new Thickness(1),
					CornerRadius = new CornerRadius(5),
					Padding = new Thickness(10),
					Margin = new Thickness(5),
					MinWidth = 200
				};

				StackPanel reviewPanel = new StackPanel();

				// Имя пользователя
				TextBlock userNameBlock = new TextBlock
				{
					Text = review.User,
					FontWeight = FontWeights.Bold,
					Margin = new Thickness(0, 0, 0, 5),
					Foreground = (SolidColorBrush)Application.Current.Resources["TextBrush"]
				};
				reviewPanel.Children.Add(userNameBlock);

				// Рейтинг
				StackPanel ratingPanel = new StackPanel
				{
					Orientation = Orientation.Horizontal,
					Margin = new Thickness(0, 0, 0, 5)
				};

				// Отображаем звезды рейтинга
				for (int i = 0; i < 5; i++)
				{
					TextBlock star = new TextBlock
					{
						Text = i < review.Score ? "★" : "☆",
						FontSize = 16,
						Foreground = i < review.Score ? Brushes.Gold : Brushes.Gray,
						Margin = new Thickness(0, 0, 5, 0)
					};
					ratingPanel.Children.Add(star);
				}

				reviewPanel.Children.Add(ratingPanel);

				// Комментарий
				TextBlock commentBlock = new TextBlock
				{
					Text = review.Comment,
					TextWrapping = TextWrapping.Wrap,
					Foreground = (SolidColorBrush)Application.Current.Resources["TextBrush"]
				};
				reviewPanel.Children.Add(commentBlock);

				if (_viewModel.DeleteReviewVisible == Visibility.Visible)
				{
					Button deleteButton = new Button
					{
						Content = (string)Application.Current.Resources["DeleteReviewButton"],
						Margin = new Thickness(0, 10, 0, 0),
						Tag = review,
						CommandParameter = review,
						Style = (Style)Application.Current.Resources["DefaultButtonStyle"]
					};
					deleteButton.Command = _viewModel.DeleteReviewCommand;
					reviewPanel.Children.Add(deleteButton);
				}

				reviewBorder.Child = reviewPanel;
				ReviewWrapPanel.Children.Add(reviewBorder);
			}
		}

		private void Radio_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton radioButton && int.TryParse(radioButton.Content.ToString(), out int rating))
			{
				_viewModel.ReviewRating = rating;
			}
		}

		private void OnThemeChanged(object sender, ThemeManager.AppTheme e)
		{
			LoadReviews();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			ThemeManager.Instance.ThemeChanged -= OnThemeChanged;
		}
	}
}
