using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Windows;
using WPF_FitnessClub.Models;

namespace WPF_FitnessClub.ViewModels
{
	public class DataTableVM : ViewModelBase
	{
		private readonly string connectionString;
		private DataSet dataSet;
		private string currentTable;
		private ObservableCollection<string> tableNames;

		public DataTableVM()
		{
			// Получение строки подключения из конфигурационного файла
			connectionString = ConfigurationManager.ConnectionStrings["FitnessClubConnectionString"].ConnectionString;
			dataSet = new DataSet();
			tableNames = new ObservableCollection<string>();
			LoadTables();
		}

		// Свойство для хранения имен таблиц
		public ObservableCollection<string> TableNames
		{
			get { return tableNames; }
			set
			{
				tableNames = value;
				OnPropertyChanged("TableNames");
			}
		}

		// Свойство для текущей выбранной таблицы
		public string CurrentTable
		{
			get { return currentTable; }
			set
			{
				if (currentTable != value)
				{
					currentTable = value;
					OnPropertyChanged("CurrentTable");
					OnPropertyChanged("CurrentDataTable");
				}
			}
		}

		// Свойство для получения данных текущей таблицы
		public DataTable CurrentDataTable
		{
			get
			{
				if (currentTable != null && dataSet.Tables.Contains(currentTable))
				{
					return dataSet.Tables[currentTable];
				}
				return null;
			}
		}

		// Метод для загрузки списка таблиц из базы данных
		private void LoadTables()
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					// Получение списка таблиц из базы данных
					DataTable schema = connection.GetSchema("Tables");
					
					foreach (DataRow row in schema.Rows)
					{
						if (row["TABLE_TYPE"].ToString() == "BASE TABLE")
						{
							string tableName = row["TABLE_NAME"].ToString();
							// Исключаем системные таблицы
							if (!tableName.StartsWith("sys") && !tableName.StartsWith("dt_"))
							{
								tableNames.Add(tableName);
								LoadTableData(connection, tableName);
							}
						}
					}

					// Устанавливаем первую таблицу как текущую, если список не пуст
					if (tableNames.Count > 0)
					{
						CurrentTable = tableNames[0];
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке таблиц: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Метод для загрузки данных конкретной таблицы
		private void LoadTableData(SqlConnection connection, string tableName)
		{
			try
			{
				string query = $"SELECT * FROM [{tableName}]";
				SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
				adapter.Fill(dataSet, tableName);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при загрузке данных таблицы {tableName}: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// Метод для обновления данных в таблице
		public void RefreshTable(string tableName = null)
		{
			if (string.IsNullOrEmpty(tableName))
			{
				tableName = CurrentTable;
			}

			if (!string.IsNullOrEmpty(tableName) && dataSet.Tables.Contains(tableName))
			{
				dataSet.Tables[tableName].Clear();
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					LoadTableData(connection, tableName);
				}
				OnPropertyChanged("CurrentDataTable");
			}
		}
	}
}
