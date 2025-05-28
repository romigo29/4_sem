using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.ViewModels;

namespace WPF_FitnessClub.View
{
    /// <summary>
    /// Логика взаимодействия для SubscriptionsView.xaml
    /// </summary>
    public partial class SubscriptionsView : UserControl
    {
        public SubscriptionsVM _viewModel;

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

        /// <summary>
        /// Обновляет список абонементов в представлении
        /// </summary>
        /// <param name="subscriptions">Обновленный список абонементов</param>
        public void UpdateSubscriptions(List<Subscription> subscriptions)
        {
            // Делегируем обновление во ViewModel
            _viewModel.UpdateSubscriptions(subscriptions);
        }
     
        /// <summary>
        /// Обновляет список абонементов в представлении с возможностью сброса фильтров
        /// </summary>
        /// <param name="subscriptions">Обновленный список абонементов</param>
        /// <param name="resetFilters">Флаг сброса фильтров</param>
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
        
        /// <summary>
        /// Обработчик события предварительного ввода текста в поля цены
        /// Разрешает ввод только цифр и одной десятичной точки/запятой
        /// </summary>
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и одну десятичную точку/запятую
            Regex regex = new Regex(@"^[0-9]|[.,]$");
            bool isMatch = regex.IsMatch(e.Text);

            if (!isMatch)
            {
                e.Handled = true;
                return;
            }

            // Проверяем, содержит ли текст уже десятичную точку или запятую
            if (e.Text == "." || e.Text == ",")
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null && (textBox.Text.Contains(".") || textBox.Text.Contains(",")))
                {
                    e.Handled = true;
                    return;
                }

                // Если текст пустой, добавляем "0" перед десятичной точкой/запятой
                if (textBox != null && string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = "0";
                    textBox.CaretIndex = 1;
                }
            }
        }

        private void PriceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Разрешаем специальные клавиши (Delete, Backspace, стрелки и т.д.)
            if (e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Left || 
                e.Key == Key.Right || e.Key == Key.Tab)
            {
                return;
            }

            // Запрещаем вставку, если в буфере обмена содержатся недопустимые символы
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (System.Windows.Clipboard.ContainsText())
                {
                    string clipboardText = System.Windows.Clipboard.GetText();
                    Regex regex = new Regex(@"^[0-9]*([.,][0-9]*)?$");
                    if (!regex.IsMatch(clipboardText))
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null || string.IsNullOrEmpty(textBox.Text))
                return;

            // Запоминаем позицию курсора
            int caretIndex = textBox.CaretIndex;
            bool textWasChanged = false;

            // Заменяем запятую на точку для единообразия
            if (textBox.Text.Contains(","))
            {
                // Отключаем обработчик события, чтобы избежать рекурсии
                textBox.TextChanged -= PriceTextBox_TextChanged;
                
                textBox.Text = textBox.Text.Replace(",", ".");
                textWasChanged = true;
                
                // Корректируем позицию курсора, если она была после запятой
                if (caretIndex > textBox.Text.IndexOf('.'))
                    caretIndex = Math.Min(caretIndex, textBox.Text.Length);
                
                // Снова подключаем обработчик события
                textBox.TextChanged += PriceTextBox_TextChanged;
            }

            // Проверяем, что текст соответствует формату числа
            try
            {
                if (textBox.Text != "." && textBox.Text != "0.")
                {
                    decimal value = Convert.ToDecimal(textBox.Text);
                    // Если число отрицательное, заменяем его на 0
                    if (value < 0)
                    {
                        // Отключаем обработчик события, чтобы избежать рекурсии
                        if (!textWasChanged)
                            textBox.TextChanged -= PriceTextBox_TextChanged;
                        
                        textBox.Text = "0";
                        
                        // Снова подключаем обработчик события
                        if (!textWasChanged)
                            textBox.TextChanged += PriceTextBox_TextChanged;
                        
                        caretIndex = textBox.Text.Length;
                        textWasChanged = true;
                    }
                }
            }
            catch
            {

            }

            // Устанавливаем позицию курсора
            if (textWasChanged)
                textBox.CaretIndex = caretIndex;
                
            // После всех изменений обновляем привязку данных, чтобы ViewModel получила новое значение
            BindingExpression binding = textBox.GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
                binding.UpdateSource();
        }
    }
} 