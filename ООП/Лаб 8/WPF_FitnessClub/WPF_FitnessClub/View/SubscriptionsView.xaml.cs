using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.ViewModels;

namespace WPF_FitnessClub.View
{
    /// <summary>
    /// Логика взаимодействия для SubscriptionsView.xaml
    /// </summary>
    public partial class SubscriptionsView : UserControl
    {
        private SubscriptionsVM _viewModel;

        public event Action<Subscription> SubscriptionSelected;

        public SubscriptionsView(MainWindow mainWindow, List<Subscription> subscriptions)
        {
            InitializeComponent();
            
            // Создание и настройка ViewModel
            _viewModel = new SubscriptionsVM(mainWindow, subscriptions);
            _viewModel.SubscriptionSelected += OnSubscriptionSelected;
            
            // Установка контекста данных
            DataContext = _viewModel;
        }

        private void OnSubscriptionSelected(Subscription subscription)
        {
            // Перенаправляем событие выше
            SubscriptionSelected?.Invoke(subscription);
        }

        public void UpdateSubscriptions(List<Subscription> subscriptions)
        {
            // Делегируем обновление во ViewModel
            _viewModel.UpdateSubscriptions(subscriptions);
        }
     
        public void UpdateSubscriptions(List<Subscription> subscriptions, bool resetFilters)
        {
            if (resetFilters)
            {
                _viewModel.ResetFilters();
            }
            
            // Делегируем обновление во ViewModel
            _viewModel.UpdateSubscriptions(subscriptions);
        }

        private void Subscription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Subscription subscription)
            {
                _viewModel.SelectSubscriptionCommand.Execute(subscription);
            }
        }
    }
} 