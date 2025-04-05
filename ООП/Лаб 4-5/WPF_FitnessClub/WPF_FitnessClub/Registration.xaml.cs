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
using System.Windows.Shapes;

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для Registration.xaml
	/// </summary>
	public partial class Registration : Window
	{

		private readonly string adminLogin = "";
		private readonly string adminPassword = "";

		private readonly string coachLogin = "1";
		private readonly string coachPassword = "1";

		private readonly string userLogin = "2";
		private readonly string userPassword = "2";

		public Registration()
		{
			InitializeComponent();
		}

		private void OpenMainWindow(UserRole role)
		{
			MainWindow mainWindow = new MainWindow(role);
			mainWindow.Show();
			this.Close();
		}

		private void EnterButton_Click(object sender, RoutedEventArgs e)
		{
			string EnteredLogin = LoginInput.Text;
			string EnteredPassword = PasswordInput.Text;

			if (EnteredLogin == adminLogin && EnteredPassword == adminPassword)
			{
				MessageBox.Show($"{Application.Current.Resources["EnteredAdmin"]}");
				OpenMainWindow(UserRole.Admin);
			}

			else if (EnteredLogin == coachLogin && EnteredPassword == coachPassword)
			{
				MessageBox.Show($"{Application.Current.Resources["EnteredCoach"]}");
				OpenMainWindow(UserRole.Coach);

			}

			else if (EnteredLogin == userLogin && EnteredPassword == userPassword)
			{
				MessageBox.Show($"{Application.Current.Resources["EnteredClient"]}");
				OpenMainWindow(UserRole.Client);

			}

			else
			{
				MessageBox.Show($"{Application.Current.Resources["FailedLogin"]}");
			}


		}

		private void RussianButton_Click(object sender, RoutedEventArgs e)
		{
			 LanguageManager.Instance.ChangeLanguage("ru-RU");
		}

		private void EnglishButton_Click(object sender, RoutedEventArgs e)
		{
			LanguageManager.Instance.ChangeLanguage("en-US");
		}
	}
}
