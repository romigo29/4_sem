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

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Subscriptions_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void AddButon_Click(object sender, RoutedEventArgs e)
		{
			AddSubscription addSubscription = new AddSubscription();
			addSubscription.Show();
        }
    }
}
