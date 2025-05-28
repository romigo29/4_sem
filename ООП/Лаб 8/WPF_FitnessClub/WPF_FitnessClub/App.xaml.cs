using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.Data;

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

			// Установка культуры для всего приложения
			Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
			
			// Создание класса для форматирования валюты в белорусских рублях
			CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.CurrencySymbol = "Br";
			customCulture.NumberFormat.CurrencyDecimalSeparator = ",";
			customCulture.NumberFormat.CurrencyGroupSeparator = " ";
			
			// Установка пользовательской культуры
			Thread.CurrentThread.CurrentCulture = customCulture;

			// Инициализация базы данных
			try
			{
				DatabaseInitializer dbInitializer = new DatabaseInitializer();
				dbInitializer.SeedData();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}

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