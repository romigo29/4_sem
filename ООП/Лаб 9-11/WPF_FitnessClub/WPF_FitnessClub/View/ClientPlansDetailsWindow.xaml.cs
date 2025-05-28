using System.Windows;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.ViewModels;

namespace WPF_FitnessClub.View
{
    /// <summary>
    /// Логика взаимодействия для ClientPlansDetailsWindow.xaml
    /// </summary>
    public partial class ClientPlansDetailsWindow : Window
    {
        private ClientPlansDetailsViewModel _viewModel;

        public ClientPlansDetailsWindow()
        {
            InitializeComponent();
        }

        public ClientPlansDetailsWindow(User client)
        {
            InitializeComponent();
            
            // Устанавливаем заголовок окна с именем клиента
            Title = $"Планы клиента: {client.FullName}";
            
            // Создаем ViewModel и устанавливаем клиента
            _viewModel = new ClientPlansDetailsViewModel(client);
            
            // Устанавливаем DataContext
            DataContext = _viewModel;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем ViewModel из контекста данных
            if (DataContext is ClientPlansDetailsViewModel viewModel)
            {
                // Загружаем планы клиента
                viewModel.LoadClientPlans();
            }
        }
    }
} 