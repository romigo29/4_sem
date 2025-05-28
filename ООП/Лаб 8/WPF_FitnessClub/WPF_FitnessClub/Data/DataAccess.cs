using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.SQL;
using System.Text;

namespace WPF_FitnessClub.Data
{
    /// <summary>
    /// Класс для работы с базой данных фитнес-клуба
    /// </summary>
    public class DataAccess
    {
        private readonly string connectionString;

        public DataAccess()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FitnessClubConnectionString"].ConnectionString;
        }

        #region Users


        public int AddUser(User user)
        {
            int userId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
      
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(SqlQueries.AddUser, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@FullName", user.FullName);
                            command.Parameters.AddWithValue("@Email", user.Email);
                            command.Parameters.AddWithValue("@Login", user.Login);
                            command.Parameters.AddWithValue("@Password", user.Password);
                            command.Parameters.AddWithValue("@Role", ((int)user.Role).ToString());

                            userId = Convert.ToInt32(command.ExecuteScalar());
                        }
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {   
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return userId;
        }
        public void UpdateUser(int userId, User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                        UPDATE Users
                        SET FullName = @FullName,
                            Email = @Email,
                            Login = @Login,
                            Password = @Password,
                            Role = @Role
                        WHERE Id = @UserId";

                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@FullName", user.FullName);
                            command.Parameters.AddWithValue("@Email", user.Email);
                            command.Parameters.AddWithValue("@Login", user.Login);
                            command.Parameters.AddWithValue("@Password", user.Password);
                            command.Parameters.AddWithValue("@Role", (int)user.Role);

                            command.ExecuteNonQuery();
                        }
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public User GetUserById(int userId)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, FullName, Email, Login, Password, Role FROM Users WHERE Id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = CreateUserFromReader(reader);
                        }
                    }
                }
            }

            return user;
        }

        public User GetUserByLogin(string login)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, FullName, Email, Login, Password, Role FROM Users WHERE Login = @Login";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = CreateUserFromReader(reader);
                        }
                    }
                }
            }

            return user;
        }


        public void DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM Users WHERE Id = @UserId";

                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.ExecuteNonQuery();
                        }
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id, FullName, Email, Login, Password, Role FROM Users";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(CreateUserFromReader(reader));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
            }

            return users;
        }

        private User CreateUserFromReader(SqlDataReader reader)
        {
            return new User(
                reader["FullName"].ToString(),
                reader["Email"].ToString(),
                reader["Login"].ToString(),
                reader["Password"].ToString(),
                (UserRole)Convert.ToInt32(reader["Role"])
            );
        }

        #endregion

        #region UserAccounts
        public int AddUserAccount(UserAccount userAccount, int userId)
        {
            int accountId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                        INSERT INTO UserAccounts (UserId, RegistrationDate, IsActive, ProfileImagePath)
                        VALUES (@UserId, @RegistrationDate, @IsActive, @ProfileImagePath);
                        SELECT SCOPE_IDENTITY();";

                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@RegistrationDate", userAccount.RegistrationDate);
                            command.Parameters.AddWithValue("@IsActive", userAccount.IsActive);
                            command.Parameters.AddWithValue("@ProfileImagePath", userAccount.ProfileImagePath ?? (object)DBNull.Value);

                            accountId = Convert.ToInt32(command.ExecuteScalar());
                        }
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {

                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return accountId;
        }

        public UserAccount GetUserAccountByUserId(int userId)
        {
            UserAccount userAccount = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT ua.Id, ua.RegistrationDate, ua.IsActive, ua.ProfileImagePath, 
                       u.Id as UserId, u.FullName, u.Email, u.Login, u.Password, u.Role
                FROM UserAccounts ua
                JOIN Users u ON ua.UserId = u.Id
                WHERE u.Id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User(
                                reader["FullName"].ToString(),
                                reader["Email"].ToString(),
                                reader["Login"].ToString(),
                                reader["Password"].ToString(),
                                (UserRole)Convert.ToInt32(reader["Role"])
                            );

                            userAccount = new UserAccount(user)
                            {
                                RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                ProfileImagePath = reader["ProfileImagePath"] == DBNull.Value ? null : reader["ProfileImagePath"].ToString()
                            };

                            int accountId = Convert.ToInt32(reader["Id"]);
                            userAccount.Subscriptions = GetSubscriptionsByUserAccountId(accountId);
                        }
                    }
                }
            }

            return userAccount;
        }

        #endregion

        #region Subscriptions

        public List<Subscription> GetAllSubscriptions()
        {
            List<Subscription> subscriptions = new List<Subscription>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("custom_GetAllSubscriptions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subscription subscription = CreateSubscriptionFromReader(reader);
                            subscriptions.Add(subscription);
                        }
                    }
                }

                // Загружаем отзывы для каждой подписки
                foreach (var subscription in subscriptions)
                {
                    subscription.Reviews = GetReviewsBySubscriptionId(Convert.ToInt32(subscription.GetType().GetProperty("Id").GetValue(subscription)));
                    subscription.Rating = subscription.CalculateRating();
                }


            }

            return subscriptions;
        }

        public void DeleteSubscription(int subscriptionId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand("custom_DeleteSubscription", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
                            command.ExecuteNonQuery();
                        }
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {

                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private Subscription CreateSubscriptionFromReader(SqlDataReader reader)
        {
            int id = Convert.ToInt32(reader["Id"]);
            
            Subscription subscription = new Subscription(
                reader["Name"].ToString(),
                Convert.ToDouble(reader["Price"]),
                reader["Description"].ToString(),
                reader["ImagePath"].ToString(),
                reader["Duration"].ToString(),
                reader["SubscriptionType"].ToString(),
                new List<Review>()
            );
            
            subscription.GetType().GetProperty("Id").SetValue(subscription, id);
            
            return subscription;
        }

        public Subscription GetSubscriptionById(int subscriptionId)
        {
            Subscription subscription = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("custom_GetSubscriptionById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", subscriptionId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            subscription = CreateSubscriptionFromReader(reader);
                            
                            subscription.Reviews = GetReviewsBySubscriptionId(subscriptionId);
                        }
                    }
                }
            }

            return subscription;
        }

        public int AddSubscription(Subscription subscription)
        {
            int subscriptionId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand("custom_AddSubscription", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            
                            command.Parameters.AddWithValue("@Name", subscription.Name);
                            command.Parameters.AddWithValue("@Price", subscription.Price);
                            command.Parameters.AddWithValue("@Description", subscription.Description);
                            command.Parameters.AddWithValue("@ImagePath", subscription.ImagePath);
                            command.Parameters.AddWithValue("@Duration", subscription.Duration);
                            command.Parameters.AddWithValue("@SubscriptionType", subscription.SubscriptionType);

                            subscriptionId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        if (subscription.Reviews != null && subscription.Reviews.Count > 0)
                        {
                            foreach (var review in subscription.Reviews)
                            {
                                using (SqlCommand command = new SqlCommand("custom_AddReview", connection, transaction))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    
                                    command.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
                                    command.Parameters.AddWithValue("@UserName", review.User);
                                    command.Parameters.AddWithValue("@Score", review.Score);
                                    command.Parameters.AddWithValue("@Comment", review.Comment);

                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        
                        transaction.Commit();
                    }
                    catch (Exception)
                    {

                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return subscriptionId;
        }

        public void UpdateSubscription(int subscriptionId, Subscription subscription)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                // Создание транзакции
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand("custom_UpdateSubscription", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            
                            command.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
                            command.Parameters.AddWithValue("@Name", subscription.Name);
                            command.Parameters.AddWithValue("@Price", subscription.Price);
                            command.Parameters.AddWithValue("@Description", subscription.Description);
                            command.Parameters.AddWithValue("@ImagePath", subscription.ImagePath);
                            command.Parameters.AddWithValue("@Duration", subscription.Duration);
                            command.Parameters.AddWithValue("@SubscriptionType", subscription.SubscriptionType);

                            command.ExecuteNonQuery();
                        }
                        
                        // Подтверждение транзакции
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Откат транзакции при ошибке
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Получает список подписок для аккаунта пользователя
        /// </summary>
        /// <param name="userAccountId">ID аккаунта пользователя</param>
        /// <returns>Список подписок</returns>
        private List<Subscription> GetSubscriptionsByUserAccountId(int userAccountId)
        {
            List<Subscription> subscriptions = new List<Subscription>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT s.Id, s.Name, s.Price, s.Description, s.ImagePath, s.Duration, s.SubscriptionType
                FROM Subscriptions s
                JOIN UserSubscriptions us ON s.Id = us.SubscriptionId
                WHERE us.UserAccountId = @UserAccountId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserAccountId", userAccountId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subscription subscription = CreateSubscriptionFromReader(reader);
                            subscriptions.Add(subscription);
                        }
                    }
                }

                foreach (var subscription in subscriptions)
                {
                    subscription.Reviews = GetReviewsBySubscriptionId(Convert.ToInt32(subscription.GetType().GetProperty("Id").GetValue(subscription)));
                }
            }

            return subscriptions;
        }

        #endregion

        #region Reviews

        /// <summary>
        /// Добавляет новый отзыв для подписки
        /// </summary>
        /// <param name="review">Отзыв</param>
        /// <param name="subscriptionId">ID подписки</param>
        /// <returns>ID добавленного отзыва</returns>
        public int AddReview(Review review, int subscriptionId)
        {
            int reviewId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                // Создание транзакции
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand("custom_AddReview", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            
                            command.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
                            command.Parameters.AddWithValue("@UserName", review.User);
                            command.Parameters.AddWithValue("@Score", review.Score);
                            command.Parameters.AddWithValue("@Comment", review.Comment);

                            // Получаем ID добавленного отзыва
                            reviewId = Convert.ToInt32(command.ExecuteScalar());
                        }
                        
                        // Подтверждение транзакции
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Откат транзакции при ошибке
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return reviewId;
        }

        /// <summary>
        /// Получает все отзывы для подписки
        /// </summary>
        /// <param name="subscriptionId">ID подписки</param>
        /// <returns>Список отзывов</returns>
        public List<Review> GetReviewsBySubscriptionId(int subscriptionId)
        {
            List<Review> reviews = new List<Review>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("custom_GetReviewsBySubscriptionId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SubscriptionId", subscriptionId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Review review = new Review(
                                reader["UserName"].ToString(),
                                Convert.ToInt32(reader["Score"]),
                                reader["Comment"].ToString()
                            );
                            reviews.Add(review);
                        }
                    }
                }
            }

            return reviews;
        }

        /// <summary>
        /// Удаляет отзыв по ID
        /// </summary>
        /// <param name="reviewId">ID отзыва</param>
        public void DeleteReview(int reviewId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                // Создание транзакции
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand("custom_DeleteReview", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ReviewId", reviewId);
                            command.ExecuteNonQuery();
                        }
                        
                        // Подтверждение транзакции
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Откат транзакции при ошибке
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Сортировка

        public List<Subscription> FilterSubscription(string name = null, double minPrice = 0, double maxPrice = double.MaxValue, string type = null, string duration = null)
        {
            List<Subscription> subscriptions = new List<Subscription>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (SqlCommand command = new SqlCommand("custom_FilterSubscriptions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    // Добавляем параметры
                    command.Parameters.AddWithValue("@Name", name != null ? name : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MinPrice", minPrice);
                    command.Parameters.AddWithValue("@MaxPrice", maxPrice);
                    command.Parameters.AddWithValue("@Type", type != null ? type : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Duration", duration != null ? duration : (object)DBNull.Value);
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subscription subscription = CreateSubscriptionFromReader(reader);
                            subscriptions.Add(subscription);
                        }
                    }
                }
                
                // Загружаем отзывы для каждой подписки
                foreach (var subscription in subscriptions)
                {
                    subscription.Reviews = GetReviewsBySubscriptionId(Convert.ToInt32(subscription.GetType().GetProperty("Id").GetValue(subscription)));
                    subscription.Rating = subscription.CalculateRating();
                }
            }

            return subscriptions;
        }

        #endregion
    }
} 