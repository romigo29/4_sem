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
using System.Windows.Shapes;
using WPF_FitnessClub.ViewModels;

namespace WPF_FitnessClub.View
{
	/// <summary>
	/// Логика взаимодействия для DataTableView.xaml
	/// </summary>
	public partial class DataTableView : UserControl
	{
		private DataTableVM viewModel;

		public DataTableView()
		{
			InitializeComponent();
			Loaded += DataTableView_Loaded;
		}

		private void DataTableView_Loaded(object sender, RoutedEventArgs e)
		{
			viewModel = DataContext as DataTableVM;
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			if (viewModel != null)
			{
				viewModel.RefreshTable();
				MessageBox.Show("Данные успешно обновлены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	}
}
