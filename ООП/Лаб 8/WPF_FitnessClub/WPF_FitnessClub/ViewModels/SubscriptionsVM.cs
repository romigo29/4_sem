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

namespace WPF_FitnessClub.ViewModels
{
	public class SubscriptionsVM : ViewModelBase
	{
		private MainWindow _mainWindow;
		private List<Subscription> _allSubscriptions;
		private ObservableCollection<Subscription> _filteredSubscriptions;
		private Visibility _filterPanelVisibility = Visibility.Visible;
		private DataAccess dataAccess;
		private bool _isFiltersApplied = false;

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

		#endregion

		#region Команды

		public ICommand ToggleFilterPanelCommand { get; }
		public ICommand SelectSubscriptionCommand { get; }

		#endregion

		public SubscriptionsVM(MainWindow mainWindow, List<Subscription> subscriptions)
		{
			_mainWindow = mainWindow;
			dataAccess = new DataAccess();

			// Инициализация команд
			ToggleFilterPanelCommand = new RelayCommand(ToggleFilterPanel);
			SelectSubscriptionCommand = new RelayCommand<Subscription>(OnSubscriptionSelected);
            
			// Загрузка абонементов
			LoadAllSubscriptions(subscriptions);
		}
        
		/// <summary>
		/// Загружает полный список абонементов и обновляет отображение
		/// </summary>
		/// <param name="subscriptions">Список абонементов для загрузки</param>
		private void LoadAllSubscriptions(List<Subscription> subscriptions)
		{
			_allSubscriptions = subscriptions;
			
			// Если фильтры не применены, показываем все абонементы
			if (!_isFiltersApplied)
			{
				FilteredSubscriptions = new ObservableCollection<Subscription>(subscriptions);
			}
			else
			{
				// Иначе применяем текущие фильтры к обновленному списку
				ApplyFilters();
			}
		}

		private void ApplyFilters()
		{
			var filtered = _allSubscriptions.Where(s =>
			{
				bool matchesName = string.IsNullOrWhiteSpace(SearchText) ||
								  s.Name.ToLower().Contains(SearchText.ToLower());

				bool matchesPrice = true;
				if (!string.IsNullOrWhiteSpace(MinCost) && double.TryParse(MinCost, out double minCostValue))
				{
					matchesPrice = matchesPrice && s.Price >= minCostValue;
				}
				if (!string.IsNullOrWhiteSpace(MaxCost) && double.TryParse(MaxCost, out double maxCostValue))
				{
					matchesPrice = matchesPrice && s.Price <= maxCostValue;
				}

				string chosenType = SelectedType?.Content.ToString();
				bool matchesType = string.IsNullOrEmpty(chosenType) ||
								  chosenType == s.SubscriptionType;

				string chosenDuration = SelectedDuration?.Content.ToString();
				bool matchesDuration = string.IsNullOrEmpty(chosenDuration) ||
								  chosenDuration == s.Duration;

				return matchesName && matchesPrice && matchesType && matchesDuration;
			}).ToList();

			FilteredSubscriptions = new ObservableCollection<Subscription>(filtered);
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

		/// <summary>
		/// Обновляет список абонементов и применяет фильтры при необходимости
		/// </summary>
		/// <param name="subscriptions">Обновленный список абонементов</param>
		public void UpdateSubscriptions(List<Subscription> subscriptions)
		{
			LoadAllSubscriptions(subscriptions);
		}
        
		/// <summary>
		/// Сбрасывает все примененные фильтры и показывает полный список абонементов
		/// </summary>
		public void ResetFilters()
		{
			_searchText = null;
			_minCost = null;
			_maxCost = null;
			_selectedType = null;
			_selectedDuration = null;
			_isFiltersApplied = false;
			
			OnPropertyChanged(nameof(SearchText));
			OnPropertyChanged(nameof(MinCost));
			OnPropertyChanged(nameof(MaxCost));
			OnPropertyChanged(nameof(SelectedType));
			OnPropertyChanged(nameof(SelectedDuration));
			
			// Показываем все абонементы без фильтрации
			FilteredSubscriptions = new ObservableCollection<Subscription>(_allSubscriptions);
		}
	}
}