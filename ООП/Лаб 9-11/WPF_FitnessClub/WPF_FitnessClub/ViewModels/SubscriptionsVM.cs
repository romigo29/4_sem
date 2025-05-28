using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_FitnessClub.Models;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static WPF_FitnessClub.Commands;
using WPF_FitnessClub.Data;
using WPF_FitnessClub.Data.Services;

namespace WPF_FitnessClub.ViewModels
{
	public class SubscriptionsVM : ViewModelBase
	{
		private MainWindow _mainWindow;
		private List<Subscription> _allSubscriptions;
		private ObservableCollection<Subscription> _filteredSubscriptions;
		private Visibility _filterPanelVisibility = Visibility.Visible;
		private SubscriptionService _subscriptionService;
		private bool _isFiltersApplied = false;
		private bool _isLoading = false;

		private readonly Dictionary<string, string> _typeTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			// Английские варианты к русским
			{ "Unlimited", "Безлимит" },
			{ "Standard", "Обычный" },
			// Русские варианты к русским (для единообразия)
			{ "Безлимит", "Безлимит" },
			{ "Обычный", "Обычный" },
		};

		private readonly Dictionary<string, string> _durationTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			// Английские варианты к русским
			{ "OneMonth", "1 месяц" },
			{ "One Month", "1 месяц" },
			{ "1 Month", "1 месяц" },
			{ "ThreeMonths", "3 месяца" },
			{ "Three Months", "3 месяца" },
			{ "3 Months", "3 месяца" },
			{ "SixMonths", "6 месяцев" },
			{ "Six Months", "6 месяцев" },
			{ "6 Months", "6 месяцев" },
			{ "OneYear", "1 год" },
			{ "One Year", "1 год" },
			{ "1 Year", "1 год" },
			{ "TwelveMonths", "12 месяцев" },
			{ "Twelve Months", "12 месяцев" },
			{ "12 Months", "12 месяцев" },
			// Русские варианты к русским (для единообразия)
			{ "1 месяц", "1 месяц" },
			{ "3 месяца", "3 месяца" },
			{ "6 месяцев", "6 месяцев" },
			{ "1 год", "1 год" },
			{ "12 месяцев", "12 месяцев" }
		};

		// Фильтры
		private string _searchText;
		private string _minCost;
		private string _maxCost;
		private ComboBoxItem _selectedType;
		private ComboBoxItem _selectedDuration;

		// Кнопка скрытия/показа панели фильтров
		private string _manipulatePanelButtonContent = "◀";

		public event Action<Subscription> SubscriptionSelected;

		#region Свойства

		public ObservableCollection<Subscription> FilteredSubscriptions
		{
			get => _filteredSubscriptions;
			set
			{
				_filteredSubscriptions = value;
				OnPropertyChanged(nameof(FilteredSubscriptions));
			}
		}

		public Visibility FilterPanelVisibility
		{
			get => _filterPanelVisibility;
			set
			{
				_filterPanelVisibility = value;
				OnPropertyChanged(nameof(FilterPanelVisibility));
			}
		}

		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				OnPropertyChanged(nameof(SearchText));
				_isFiltersApplied = true;
				ApplyFilters();
			}
		}

		public string MinCost
		{
			get => _minCost;
			set
			{
				_minCost = value;
				OnPropertyChanged(nameof(MinCost));
				_isFiltersApplied = true;
				ValidateAndCorrectPrices();
				ApplyFilters();
			}
		}

		public string MaxCost
		{
			get => _maxCost;
			set
			{
				_maxCost = value;
				OnPropertyChanged(nameof(MaxCost));
				_isFiltersApplied = true;
				ValidateAndCorrectPrices();
				ApplyFilters();
			}
		}

		public ComboBoxItem SelectedType
		{
			get => _selectedType;
			set
			{
				_selectedType = value;
				OnPropertyChanged(nameof(SelectedType));
				_isFiltersApplied = true;
				ApplyFilters();
			}
		}

		public ComboBoxItem SelectedDuration
		{
			get => _selectedDuration;
			set
			{
				_selectedDuration = value;
				OnPropertyChanged(nameof(SelectedDuration));
				_isFiltersApplied = true;
				ApplyFilters();
			}
		}

		public string ManipulatePanelButtonContent
		{
			get => _manipulatePanelButtonContent;
			set
			{
				_manipulatePanelButtonContent = value;
				OnPropertyChanged(nameof(ManipulatePanelButtonContent));
			}
		}
        
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

		#endregion

		#region Команды

		public ICommand ToggleFilterPanelCommand { get; }
		public ICommand SelectSubscriptionCommand { get; }
		public ICommand RefreshSubscriptionsCommand { get; }

		#endregion

		public SubscriptionsVM(MainWindow mainWindow, List<Subscription> subscriptions)
		{
			_mainWindow = mainWindow;
			_subscriptionService = new SubscriptionService();

			// Подписываемся на событие изменения языка
			LanguageManager.Instance.LanguageChanged += OnLanguageChanged;

			// Инициализация команд
			ToggleFilterPanelCommand = new RelayCommand(ToggleFilterPanel);
			SelectSubscriptionCommand = new RelayCommand<Subscription>(OnSubscriptionSelected);
            RefreshSubscriptionsCommand = new RelayCommand(_ => RefreshSubscriptions());
            
            // Инициализация значений по умолчанию
            _searchText = string.Empty;
            _minCost = string.Empty;
            _maxCost = string.Empty;
            _selectedType = null;  // Будет соответствовать опции "Все"
            _selectedDuration = null;  // Будет соответствовать опции "Все"
            
			// Логируем текущий язык
			System.Diagnostics.Debug.WriteLine($"Текущий язык интерфейса: {GetCurrentLanguage()}");
            
			// Загрузка абонементов
			RefreshSubscriptions();
		}
        
		private void LoadAllSubscriptions(List<Subscription> subscriptions)
		{
			System.Diagnostics.Debug.WriteLine($"LoadAllSubscriptions вызван с {subscriptions?.Count ?? 0} абонементами");
			
			// Проверка на null
			if (subscriptions == null)
			{
				System.Diagnostics.Debug.WriteLine("Список абонементов равен null, создаем пустой список");
				_allSubscriptions = new List<Subscription>();
			}
			else
			{
				_allSubscriptions = subscriptions;
				
				// Пересчитываем рейтинг для каждого абонемента
				foreach (var subscription in _allSubscriptions)
				{
					if (subscription.Reviews != null && subscription.Reviews.Count > 0)
					{
						// Вызываем CalculateRating для обновления свойства Rating
						subscription.Rating = subscription.CalculateRating();
						System.Diagnostics.Debug.WriteLine($"Рассчитан рейтинг для абонемента {subscription.Name}: {subscription.Rating}");
					}
					else
					{
						subscription.Rating = 0;
						System.Diagnostics.Debug.WriteLine($"Нулевой рейтинг для абонемента {subscription.Name} (нет отзывов)");
					}
				}
			}
			
			// Если фильтры не применены, показываем все абонементы
			if (!_isFiltersApplied)
			{
				System.Diagnostics.Debug.WriteLine("Фильтры не применены, показываем все абонементы");
				FilteredSubscriptions = new ObservableCollection<Subscription>(_allSubscriptions);
				System.Diagnostics.Debug.WriteLine($"FilteredSubscriptions содержит {FilteredSubscriptions.Count} элементов");
			}
			else
			{
				// Иначе применяем текущие фильтры к обновленному списку
				System.Diagnostics.Debug.WriteLine("Применяем фильтры к обновленному списку");
				ApplyFilters();
			}
		}


		private void RefreshSubscriptions()
		{
			try
			{
                IsLoading = true;
                
				// Запрашиваем свежие данные из БД через сервис
				var subscriptions = _subscriptionService.GetAll();
				LoadAllSubscriptions(subscriptions);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при обновлении списка абонементов: {ex.Message}", 
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
                IsLoading = false;
			}
		}

		private bool IsYearDuration(string duration)
		{
			if (string.IsNullOrEmpty(duration))
				return false;
				
			// Нормализуем строку для сравнения
			string normalizedDuration = duration.ToLower().Trim();
			
			// Проверяем различные варианты годовой длительности
			return normalizedDuration == "1 год" || 
				   normalizedDuration == "12 месяцев" ||
				   normalizedDuration == "годовой" ||
				   normalizedDuration == "1year" ||
				   normalizedDuration == "12months" ||
				   normalizedDuration == "one year" ||
				   normalizedDuration == "twelve months" ||
				   normalizedDuration.Contains("год") && normalizedDuration.Contains("1") ||
				   normalizedDuration.Contains("months") && normalizedDuration.Contains("12") ||
				   normalizedDuration.Contains("месяц") && normalizedDuration.Contains("12");
		}

		private void ApplyFilters()
		{
			try
			{
				string currentLang = GetCurrentLanguage();
				System.Diagnostics.Debug.WriteLine($"Applying filters... Current language: {currentLang}");
				
				if (_allSubscriptions == null)
				{
					System.Diagnostics.Debug.WriteLine("_allSubscriptions is null, returning empty list");
					FilteredSubscriptions = new ObservableCollection<Subscription>();
					return;
				}

				decimal? minCostValue = null;
				decimal? maxCostValue = null;

				// Парсим значения Min и Max
				if (!string.IsNullOrEmpty(MinCost) && decimal.TryParse(MinCost, out decimal min))
				{
					minCostValue = min;
				}

				if (!string.IsNullOrEmpty(MaxCost) && decimal.TryParse(MaxCost, out decimal max))
				{
					maxCostValue = max;
				}

				// Дополнительная проверка корректности значений
				if (minCostValue.HasValue && maxCostValue.HasValue)
				{
					if (minCostValue.Value > maxCostValue.Value)
					{
						// Если мин > макс, меняем их местами
						var temp = minCostValue;
						minCostValue = maxCostValue;
						maxCostValue = temp;
						
						// Обновляем текстовые представления
						_minCost = minCostValue.Value.ToString();
						_maxCost = maxCostValue.Value.ToString();
						
						// Уведомляем UI об изменении свойств
						OnPropertyChanged(nameof(MinCost));
						OnPropertyChanged(nameof(MaxCost));
					}
				}

				// Получаем значения фильтров из комбобоксов
				string typeFilter = SelectedType?.Content?.ToString();
				string durationFilter = SelectedDuration?.Content?.ToString();

				System.Diagnostics.Debug.WriteLine($"Выбранный тип: {typeFilter}, выбранная длительность: {durationFilter}");

				// Преобразуем значения для поиска в базе данных (на русском)
				string typeFilterDb = GetDatabaseValue(typeFilter, _typeTranslations);
				string durationFilterDb = GetDatabaseValue(durationFilter, _durationTranslations);

				System.Diagnostics.Debug.WriteLine($"Значение для поиска в БД - тип: {typeFilterDb}, длительность: {durationFilterDb}");

				// Начинаем с полного списка
				var filtered = _allSubscriptions.AsQueryable();

				// Применяем фильтр поиска по тексту
				if (!string.IsNullOrEmpty(SearchText))
				{
					System.Diagnostics.Debug.WriteLine($"Применяем фильтр по тексту: '{SearchText}'");
					filtered = filtered.Where(s => 
						s.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
						(s.Description != null && s.Description.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
					);
				}

				// Фильтр по минимальной цене
				if (minCostValue.HasValue)
				{
					System.Diagnostics.Debug.WriteLine($"Применяем фильтр по минимальной цене: {minCostValue.Value}");
					filtered = filtered.Where(s => s.Price >= minCostValue.Value);
				}

				// Фильтр по максимальной цене
				if (maxCostValue.HasValue)
				{
					System.Diagnostics.Debug.WriteLine($"Применяем фильтр по максимальной цене: {maxCostValue.Value}");
					filtered = filtered.Where(s => s.Price <= maxCostValue.Value);
				}

				// Фильтр по типу абонемента
				if (!string.IsNullOrEmpty(typeFilterDb))
				{
					System.Diagnostics.Debug.WriteLine($"Применяем фильтр по типу абонемента: '{typeFilterDb}'");
					
					// Для отладки выводим все типы абонементов
					var allTypes = _allSubscriptions.Select(s => s.SubscriptionType).Distinct().ToList();
					System.Diagnostics.Debug.WriteLine($"Доступные типы абонементов: {string.Join(", ", allTypes)}");
					
					filtered = filtered.Where(s => s.SubscriptionType == typeFilterDb);
				}

				// Фильтр по длительности
				if (!string.IsNullOrEmpty(durationFilterDb))
				{
					System.Diagnostics.Debug.WriteLine($"Применяем фильтр по длительности абонемента: '{durationFilterDb}'");
					
					// Для отладки выводим все длительности абонементов
					var allDurations = _allSubscriptions.Select(s => s.Duration).Distinct().ToList();
					System.Diagnostics.Debug.WriteLine($"Доступные длительности абонементов: {string.Join(", ", allDurations)}");
					
					// Учитываем эквивалентность годовых вариантов длительности
					if (IsYearDuration(durationFilterDb))
					{
						// Если это годовой фильтр, то находим все абонементы с годовой длительностью
						filtered = filtered.Where(s => IsYearDuration(s.Duration));
						System.Diagnostics.Debug.WriteLine($"Применен фильтр по годовой длительности");
					}
					else
					{
						filtered = filtered.Where(s => s.Duration == durationFilterDb);
					}
				}

				// Обновляем отфильтрованный список
				FilteredSubscriptions = new ObservableCollection<Subscription>(filtered);
				System.Diagnostics.Debug.WriteLine($"Отфильтровано {FilteredSubscriptions.Count} абонементов");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Ошибка при применении фильтров: {ex.Message}");
			}
		}


		private string GetCurrentLanguage()
		{
			try
			{
				// Получаем текущий язык из LanguageManager
				string languageCode = LanguageManager.Instance.CurrentLanguage;
				System.Diagnostics.Debug.WriteLine($"Получен язык из LanguageManager: {languageCode}");
				
				// Преобразуем полное название языка в двухбуквенный код
				if (languageCode.StartsWith("ru", StringComparison.OrdinalIgnoreCase))
					return "ru";
				else if (languageCode.StartsWith("en", StringComparison.OrdinalIgnoreCase))
					return "en";
				
				// Если язык не определен в LanguageManager, используем резервный метод
				
				// Попробуем получить текущую культуру из потока UI
				string uiCulture = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
				if (!string.IsNullOrEmpty(uiCulture))
				{
					System.Diagnostics.Debug.WriteLine($"Определен язык по культуре потока: {uiCulture}");
					if (uiCulture == "ru" || uiCulture == "en")
						return uiCulture;
				}

				// Более тщательный поиск в словарях ресурсов
				var resources = Application.Current.Resources;
				
				// Проверяем все словари в приложении
				foreach (var dict in resources.MergedDictionaries)
				{
					if (dict.Source != null)
					{
						string source = dict.Source.OriginalString;
						System.Diagnostics.Debug.WriteLine($"Найден словарь ресурсов: {source}");
						
						if (source.Contains("Dictionary_ru"))
						{
							// Дополнительная проверка, что словарь действительно активен
							if (dict.Contains("Language") && dict["Language"].ToString().Contains("ru"))
							{
								System.Diagnostics.Debug.WriteLine("Найден активный русский словарь");
								return "ru";
							}
							
							// Если нет маркера языка, полагаемся на имя файла
							System.Diagnostics.Debug.WriteLine("Определен русский язык по имени файла словаря");
							return "ru";
						}
						else if (source.Contains("Dictionary_en"))
						{
							// Аналогичная проверка для английского
							if (dict.Contains("Language") && dict["Language"].ToString().Contains("en"))
							{
								System.Diagnostics.Debug.WriteLine("Найден активный английский словарь");
								return "en";
							}
							
							System.Diagnostics.Debug.WriteLine("Определен английский язык по имени файла словаря");
							return "en";
						}
					}
				}
				
				// Если все вышеперечисленное не помогло, пробуем определить через системную культуру
				System.Globalization.CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
				if (currentCulture.Name.StartsWith("ru"))
				{
					System.Diagnostics.Debug.WriteLine("Определен русский язык по культуре потока");
					return "ru";
				}
				
				// Если не удалось определить, возвращаем английский по умолчанию
				System.Diagnostics.Debug.WriteLine("Не удалось определить язык, возвращаем английский по умолчанию");
				return "en";
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Ошибка при определении языка интерфейса: {ex.Message}");
				return "en"; // По умолчанию английский
			}
		}


		private string GetDatabaseValue(string uiValue, Dictionary<string, string> translations)
		{
			// Проверяем на пустоту или специальные значения
			if (string.IsNullOrEmpty(uiValue) || 
				uiValue.Equals("Все", StringComparison.OrdinalIgnoreCase) || 
				uiValue.Equals("All", StringComparison.OrdinalIgnoreCase))
			{
				return null; // Возвращаем null для пропуска фильтрации
			}

			// Логируем текущий язык и исходное значение
			string currentLang = GetCurrentLanguage();
			System.Diagnostics.Debug.WriteLine($"Текущий язык: {currentLang}, исходное значение: {uiValue}");

			// Специальная обработка для английского языка интерфейса
			if (currentLang == "en")
			{
				// Проверяем типичные значения и их русские эквиваленты
				if (uiValue.Equals("Unlimited", StringComparison.OrdinalIgnoreCase))
					return "Безлимит";
				if (uiValue.Equals("Standard", StringComparison.OrdinalIgnoreCase))
					return "Обычный";
					
				// Проверяем длительности
				if (uiValue.Contains("Month") || uiValue.Contains("месяц"))
				{
					if (uiValue.Contains("1") || uiValue.Contains("One"))
						return "1 месяц";
					if (uiValue.Contains("3") || uiValue.Contains("Three"))
						return "3 месяца";
					if (uiValue.Contains("6") || uiValue.Contains("Six"))
						return "6 месяцев";
					if (uiValue.Contains("12") || uiValue.Contains("Twelve"))
						return "12 месяцев";
				}
				if (uiValue.Contains("Year") || uiValue.Contains("год"))
					return "1 год";
			}

			// Пытаемся найти соответствие в словаре
			if (translations.TryGetValue(uiValue, out string dbValue))
			{
				System.Diagnostics.Debug.WriteLine($"Найдено прямое соответствие: {uiValue} -> {dbValue}");
				return dbValue;
			}
			
			// Если не нашли, пытаемся удалить лишние пробелы и проверить снова
			string trimmedValue = uiValue.Trim();
			if (translations.TryGetValue(trimmedValue, out dbValue))
			{
				System.Diagnostics.Debug.WriteLine($"Найдено соответствие после удаления пробелов: {trimmedValue} -> {dbValue}");
				return dbValue;
			}
			
			// Пробуем преобразовать текст (удалить пробелы, перевести в нижний регистр)
			string normalizedValue = trimmedValue.ToLower().Replace(" ", "");
			foreach (var pair in translations)
			{
				// Проверяем, содержится ли нормализованная форма ключа в нашем значении или наоборот
				string normalizedKey = pair.Key.ToLower().Replace(" ", "");
				if (normalizedKey == normalizedValue || 
					normalizedValue.Contains(normalizedKey) || 
					normalizedKey.Contains(normalizedValue))
				{
					System.Diagnostics.Debug.WriteLine($"Найдено соответствие после нормализации: {normalizedValue} -> {pair.Value}");
					return pair.Value;
				}
			}
			
			// Дополнительная проверка для значений типов и длительностей
			if (translations == _typeTranslations)
			{
				// Если это словарь типов, проверяем общие паттерны
				if (uiValue.Contains("без") || uiValue.Contains("лимит") || 
					uiValue.Contains("unlim") || uiValue.Contains("limit"))
					return "Безлимитный";
					
				if (uiValue.Contains("стандарт") || uiValue.Contains("обычн") || 
					uiValue.Contains("standard") || uiValue.Contains("regular"))
					return "Обычный";
			}
			else if (translations == _durationTranslations)
			{
				// Если это словарь длительностей, проверяем по числам
				if (uiValue.Contains("1") || uiValue.Contains("один") || uiValue.Contains("one"))
				{
					if (uiValue.Contains("мес") || uiValue.Contains("mon"))
						return "1 месяц";
					if (uiValue.Contains("год") || uiValue.Contains("year") || uiValue.Contains("лет"))
						return "1 год";
				}
				
				if (uiValue.Contains("3") || uiValue.Contains("три") || uiValue.Contains("three"))
					return "3 месяца";
					
				if (uiValue.Contains("6") || uiValue.Contains("шесть") || uiValue.Contains("six"))
					return "6 месяцев";
					
				// Специальная обработка для 12 месяцев/1 года
				if (uiValue.Contains("12") || uiValue.Contains("двенадцать") || uiValue.Contains("twelve"))
					return "12 месяцев";
					
				// Проверка на "годовой" и подобные слова
				if (uiValue.Contains("год") || uiValue.Contains("year") || 
				   uiValue.Contains("годов") || uiValue.Contains("annual"))
					return "1 год";
			}

			// Если не нашли соответствия, возвращаем оригинальное значение
			System.Diagnostics.Debug.WriteLine($"Значение '{uiValue}' не найдено в словаре соответствий");
			return uiValue;
		}

		private void ToggleFilterPanel(object parameter)
		{
			if (FilterPanelVisibility == Visibility.Visible)
			{
				FilterPanelVisibility = Visibility.Collapsed;
				ManipulatePanelButtonContent = "▶";
			}
			else
			{
				FilterPanelVisibility = Visibility.Visible;
				ManipulatePanelButtonContent = "◀";
			}
		}


		private void OnSubscriptionSelected(Subscription subscription)
		{
			SubscriptionSelected?.Invoke(subscription);
		}

	
		public void UpdateSubscriptions(List<Subscription> subscriptions)
		{
			try
			{
				IsLoading = true;
				System.Diagnostics.Debug.WriteLine($"SubscriptionsVM.UpdateSubscriptions: Обновление списка с {subscriptions?.Count ?? 0} абонементами");
				
				// Запоминаем прежнее количество элементов для проверки на удаление
				int oldCount = _allSubscriptions?.Count ?? 0;
				
				// Обновляем список всех абонементов
				_allSubscriptions = new List<Subscription>(subscriptions ?? new List<Subscription>());
				
				// Если было удаление, сбрасываем фильтры для надежности
				int newCount = _allSubscriptions.Count;
				if (newCount < oldCount)
				{
					System.Diagnostics.Debug.WriteLine($"SubscriptionsVM.UpdateSubscriptions: Обнаружено удаление элементов (было {oldCount}, стало {newCount})");
					// Сбрасываем фильтры автоматически при удалении
					_isFiltersApplied = false;
					// Сбрасываем значения фильтров
					ResetFilters();
				}
				
				// Переприменяем фильтры, если они активны
				if (_isFiltersApplied)
				{
					System.Diagnostics.Debug.WriteLine($"SubscriptionsVM.UpdateSubscriptions: Применяю фильтры");
					ApplyFilters();
				}
				else
				{
					// Иначе отображаем все абонементы
					System.Diagnostics.Debug.WriteLine($"SubscriptionsVM.UpdateSubscriptions: Отображаю все абонементы без фильтрации");
					FilteredSubscriptions = new ObservableCollection<Subscription>(_allSubscriptions);
				}
				
				// Уведомляем об изменении коллекции
				OnPropertyChanged(nameof(FilteredSubscriptions));
				System.Diagnostics.Debug.WriteLine($"SubscriptionsVM.UpdateSubscriptions: Отображается {FilteredSubscriptions.Count} элементов");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"SubscriptionsVM.UpdateSubscriptions: Ошибка: {ex.Message}");
				MessageBox.Show($"Ошибка обновления абонементов: {ex.Message}", 
					"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				IsLoading = false;
			}
		}
		
		public void ResetFilters()
		{
			// Сбрасываем значения фильтров
			_searchText = string.Empty;
			_minCost = string.Empty;
			_maxCost = string.Empty;
			
			// Устанавливаем значение "Все" для комбобоксов
			// Эти значения будут установлены при инициализации ViewModel
			_selectedType = null;
			_selectedDuration = null;
			_isFiltersApplied = false;
			
			// Обновляем свойства для уведомления UI
			OnPropertyChanged(nameof(SearchText));
			OnPropertyChanged(nameof(MinCost));
			OnPropertyChanged(nameof(MaxCost));
			OnPropertyChanged(nameof(SelectedType));
			OnPropertyChanged(nameof(SelectedDuration));
			
			// Отображаем все абонементы
			FilteredSubscriptions = new ObservableCollection<Subscription>(_allSubscriptions);
		}
		
		private void ValidateAndCorrectPrices()
		{
			// Проверяем, что оба значения не пустые
			if (string.IsNullOrEmpty(_minCost) || string.IsNullOrEmpty(_maxCost))
				return;

			// Пытаемся преобразовать значения в decimal
			if (decimal.TryParse(_minCost, out decimal min) && decimal.TryParse(_maxCost, out decimal max))
			{
				// Если максимальная цена меньше минимальной, меняем их местами
				if (max < min)
				{
					// Временно отключаем вызов этого метода, чтобы избежать рекурсии
					string tempMin = _minCost;
					string tempMax = _maxCost;

					_minCost = tempMax;
					_maxCost = tempMin;

					// Уведомляем UI об изменении свойств
					OnPropertyChanged(nameof(MinCost));
					OnPropertyChanged(nameof(MaxCost));
					
					// Показываем уведомление пользователю
					MessageBox.Show(
						"Значения минимальной и максимальной цены были автоматически поменяны местами, так как максимальная цена была меньше минимальной.",
						"Автоматическая коррекция",
						MessageBoxButton.OK,
						MessageBoxImage.Information);
				}
			}
		}

		private void OnLanguageChanged(object sender, string languageCode)
		{
			System.Diagnostics.Debug.WriteLine($"Язык изменен на: {languageCode}");
			
			// Если фильтры применены, переприменяем их с учетом нового языка
			if (_isFiltersApplied)
			{
				ApplyFilters();
			}
		}

	
	}
}