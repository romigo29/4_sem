using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPF_FitnessClub.Commands;
using WPF_FitnessClub.Model;
using WPF_FitnessClub.Repositories;

namespace WPF_FitnessClub.ViewModels
{
    public class SubscriptionDetailsViewModel : INotifyPropertyChanged
    {
        private Subscription _subscription;
        private SubscriptionRepository _repository;
        private ReviewRepository _reviewRepository;
        private bool _isEditMode;
        private string _reviewText;
        private int _reviewRating;
        private bool _canWriteReview;

        public SubscriptionDetailsViewModel(Subscription subscription, bool canWriteReview)
        {
            _subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
            _repository = new SubscriptionRepository();
            _reviewRepository = new ReviewRepository();
            _canWriteReview = canWriteReview;
            
            LoadReviews();
            
            EditCommand = new RelayCommand(param => EnterEditMode());
            SaveCommand = new RelayCommand(param => SaveSubscription());
            CancelCommand = new RelayCommand(param => CancelEdit());
            DeleteCommand = new RelayCommand(param => DeleteSubscription());
            SubmitReviewCommand = new RelayCommand(param => SubmitReview(), param => CanSubmitReview());
        }

        #region Properties

        public string SubscrName
        {
            get => _subscription.Name;
            set
            {
                if (_subscription.Name != value)
                {
                    _subscription.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImagePath
        {
            get => _subscription.ImagePath;
            set
            {
                if (_subscription.ImagePath != value)
                {
                    _subscription.ImagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Price
        {
            get => _subscription.Price;
            set
            {
                if (_subscription.Price != value)
                {
                    _subscription.Price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Type
        {
            get => _subscription.Type;
            set
            {
                if (_subscription.Type != value)
                {
                    _subscription.Type = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Duration
        {
            get => _subscription.Duration;
            set
            {
                if (_subscription.Duration != value)
                {
                    _subscription.Duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _subscription.Description;
            set
            {
                if (_subscription.Description != value)
                {
                    _subscription.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Review> Reviews { get; } = new ObservableCollection<Review>();

        public string ReviewText
        {
            get => _reviewText;
            set
            {
                if (_reviewText != value)
                {
                    _reviewText = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int ReviewRating
        {
            get => _reviewRating;
            set
            {
                if (_reviewRating != value)
                {
                    _reviewRating = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsViewMode));
                }
            }
        }

        public bool IsViewMode => !IsEditMode;

        public bool CanWriteReview => _canWriteReview;

        #endregion

        #region Commands

        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SubmitReviewCommand { get; }

        #endregion

        #region Command Methods

        private void EnterEditMode()
        {
            IsEditMode = true;
        }

        private void SaveSubscription()
        {
            try
            {
                _repository.Update(_subscription);
                IsEditMode = false;
                MessageBox.Show("Подписка успешно обновлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении подписки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelEdit()
        {
            // Восстановить исходные значения из базы данных
            _subscription = _repository.GetById(_subscription.Id);
            OnPropertyChanged(nameof(SubscrName));
            OnPropertyChanged(nameof(ImagePath));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(Duration));
            OnPropertyChanged(nameof(Description));
            
            IsEditMode = false;
        }

        private void DeleteSubscription()
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить эту подписку?", "Подтверждение удаления", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
                
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(_subscription.Id);
                    MessageBox.Show("Подписка успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    RequestClose?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении подписки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadReviews()
        {
            try
            {
                var reviews = _reviewRepository.GetBySubscriptionId(_subscription.Id);
                Reviews.Clear();
                foreach (var review in reviews)
                {
                    Reviews.Add(review);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке отзывов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SubmitReview()
        {
            if (!CanSubmitReview())
                return;

            try
            {
                var review = new Review
                {
                    SubscriptionId = _subscription.Id,
                    Text = ReviewText,
                    Rating = ReviewRating,
                    Date = DateTime.Now
                };

                _reviewRepository.Add(review);
                ReviewText = string.Empty;
                ReviewRating = 0;
                LoadReviews();
                MessageBox.Show("Отзыв успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении отзыва: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSubmitReview()
        {
            return CanWriteReview && !string.IsNullOrWhiteSpace(ReviewText) && ReviewRating > 0;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action RequestClose;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
} 