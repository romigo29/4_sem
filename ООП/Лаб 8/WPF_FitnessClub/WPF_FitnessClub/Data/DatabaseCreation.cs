using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using WPF_FitnessClub.SQL;
using System.Text.RegularExpressions;

namespace WPF_FitnessClub.Data
{
    public class DatabaseCreation
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["MasterConnectionString"].ConnectionString;
        
        private const string dbName = "FitnessClubDB";
      
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["FitnessClubConnectionString"].ConnectionString;
        
        /// <summary>
        /// Создает базу данных и все необходимые таблицы
        /// </summary>
        public static void CreateDatabase()
        {
            try
            {
                CreateDatabaseIfNotExists();
                
                CreateTables();
                
                //MessageBox.Show($"База данных {dbName} успешно создана!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

		/// <summary>
		/// Удаляет существующую базу данных и создает новую
		/// </summary>
		private static void CreateDatabaseIfNotExists()
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

                string checkDbExistsQuery = $@"
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{dbName}')
            BEGIN
                CREATE DATABASE [{dbName}]
            END";

				using (SqlCommand command = new SqlCommand(checkDbExistsQuery, connection))
				{
					command.ExecuteNonQuery();
				}
			}
		}


		/// <summary>
		/// Создает все необходимые таблицы в базе данных
		/// </summary>
		private static void CreateTables()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                
                // Используем SQL-запрос из класса SqlQueries
                ExecuteNonQuery(connection, SqlQueries.CreateTables);
                
                // Создаем хранимые процедуры
                CreateStoredProcedures(connection);
            }
        }
        
        /// <summary>
        /// Создает хранимые процедуры в базе данных
        /// </summary>
        private static void CreateStoredProcedures(SqlConnection connection)
        {
            // Процедуры для работы с пользователями
            ExecuteNonQuery(connection, SqlQueries.CreateProcGetUserByLoginAndPassword);
            ExecuteNonQuery(connection, SqlQueries.CreateProcAddUser);
            ExecuteNonQuery(connection, SqlQueries.CreateProcAddUserAccount);
            
            // Процедуры для работы с подписками
            ExecuteNonQuery(connection, SqlQueries.CreateProcGetAllSubscriptions);
            ExecuteNonQuery(connection, SqlQueries.CreateProcGetSubscriptionById);
            ExecuteNonQuery(connection, SqlQueries.CreateProcAddSubscription);
            ExecuteNonQuery(connection, SqlQueries.CreateProcUpdateSubscription);
            ExecuteNonQuery(connection, SqlQueries.CreateProcDeleteSubscription);
            
            // Процедуры для работы с отзывами
            ExecuteNonQuery(connection, SqlQueries.CreateProcGetReviewsBySubscriptionId);
            ExecuteNonQuery(connection, SqlQueries.CreateProcAddReview);
            ExecuteNonQuery(connection, SqlQueries.CreateProcDeleteReview);
            
            // Процедура для фильтрации подписок
            ExecuteNonQuery(connection, SqlQueries.CreateProcFilterSubscriptions);
        }
        
        /// <summary>
        /// Выполняет SQL-запрос без возврата результатов
        /// </summary>
        private static void ExecuteNonQuery(SqlConnection connection, string query)
        {
            // Разделяем запрос на пакеты по команде GO
            string[] batches = Regex.Split(query, @"\bGO\b", RegexOptions.IgnoreCase);
            
            foreach (string batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch))
                    continue;
                    
                using (SqlCommand command = new SqlCommand(batch, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
} 