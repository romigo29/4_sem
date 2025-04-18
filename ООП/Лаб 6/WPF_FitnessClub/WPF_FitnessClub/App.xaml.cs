using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using WPF_FitnessClub.Models;

namespace WPF_FitnessClub
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			
			ThemeManager.Instance.ChangeTheme(ThemeManager.AppTheme.Light);

			try
			{
				Uri cursorUri = new Uri("pack://application:,,,/WPF_FitnessClub;component/Resources/arrow.cur", UriKind.Absolute);
				StreamResourceInfo streamInfo = Application.GetResourceStream(cursorUri);
				if (streamInfo != null)
				{
					Mouse.OverrideCursor = new Cursor(streamInfo.Stream);
				}
				else
				{
					MessageBox.Show(
						(string)Application.Current.Resources["CursorFileNotFound"],
						(string)Application.Current.Resources["ErrorTitle"],
						MessageBoxButton.OK,
						MessageBoxImage.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format((string)Application.Current.Resources["ErrorLoadingCursor"], ex.Message),
					(string)Application.Current.Resources["ErrorTitle"],
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}

			try
			{
				Uri iconUri = new Uri("pack://application:,,,/WPF_FitnessClub;component/Resources/app_icon.ico", UriKind.Absolute);
				StreamResourceInfo iconInfo = Application.GetResourceStream(iconUri);

				if (iconInfo != null)
				{
					foreach (Window window in this.Windows)
					{
						window.Icon = BitmapFrame.Create(iconInfo.Stream);
					}
				}
				else
				{
					Console.WriteLine((string)Application.Current.Resources["IconNotFound"]);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format((string)Application.Current.Resources["ErrorLoadingIcon"], ex.Message));
			}
		}
	}
}