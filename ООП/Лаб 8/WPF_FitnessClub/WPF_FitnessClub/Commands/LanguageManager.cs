using System.Windows;
using System.Globalization;
using System.Windows.Markup;
using System.Linq;
using System;
using System.Threading;

namespace WPF_FitnessClub
{
	public class LanguageManager
	{
		private static LanguageManager instance;
		public static LanguageManager Instance => instance = new LanguageManager();

		public event EventHandler<string> LanguageChanged;

		private LanguageManager() { }

		public void ChangeLanguage(string languageCode)
		{

			try
			{
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
				Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);


				var dictionary = new ResourceDictionary
				{
					Source = new Uri($"Resources/Dictionary_{(languageCode == "ru-RU" ? "ru" : "en")}.xaml", UriKind.Relative)
				};

				// Найти и удалить только языковые словари
				var languageDictionaries = Application.Current.Resources.MergedDictionaries
					.Where(d => d.Source != null && d.Source.OriginalString.Contains("Dictionary_"))
					.ToList();

				foreach (var oldDict in languageDictionaries)
				{
					Application.Current.Resources.MergedDictionaries.Remove(oldDict);
				}

				// Добавить новый языковой словарь
				Application.Current.Resources.MergedDictionaries.Add(dictionary);

				foreach (Window window in Application.Current.Windows)
				{
					window.Language = XmlLanguage.GetLanguage(languageCode);
					window.UpdateLayout();
				}

				if (Application.Current.MainWindow != null)
				{
					Application.Current.MainWindow.Language = XmlLanguage.GetLanguage(languageCode);
					Application.Current.MainWindow.UpdateLayout();
				}

				LanguageChanged?.Invoke(this, languageCode);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format((string)Application.Current.Resources["ErrorChangingLanguage"], ex.Message), 
					(string)Application.Current.Resources["ErrorTitle"], 
					MessageBoxButton.OK, 
					MessageBoxImage.Error);
			}

		}
	}
}