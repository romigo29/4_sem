using System;
using System.Collections.Generic;
using System.Windows;
using WPF_FitnessClub.Models;
using System.Linq;

namespace WPF_FitnessClub.Data
{
    /// <summary>
    /// Класс для инициализации базы данных начальными данными
    /// </summary>
    public class DatabaseInitializer
    {
        private readonly DataAccess _dataAccess;

        public DatabaseInitializer()
        {
            _dataAccess = new DataAccess();
        }

        /// <summary>
        /// Заполняет базу данных начальными тестовыми данными
        /// </summary>
        public void SeedData()
        {
            try
            {
                // Создание базы данных и таблиц
                DatabaseCreation.CreateDatabase();

                // Проверяем, есть ли уже данные в базе
                if (!IsDataSeeded())
                {
                    // Добавление тестовых пользователей
                    SeedUsers();

                    // Добавление тестовых подписок
                    SeedSubscriptions();

                    MessageBox.Show("База данных успешно инициализирована тестовыми данными!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Проверяет, заполнена ли база данных начальными данными
        /// </summary>
        /// <returns>true, если данные уже существуют</returns>
        private bool IsDataSeeded()
        {
            // Проверяем наличие пользователей в базе
            var users = _dataAccess.GetAllUsers();
            if (users.Count > 0)
            {
                return true;
            }

            // Проверяем наличие подписок в базе
            var subscriptions = _dataAccess.GetAllSubscriptions();
            if (subscriptions.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Добавляет тестовых пользователей
        /// </summary>
        private void SeedUsers()
        {
            // Создание администратора
            User adminUser = new User("Администратор Системы", "admin@fitness.ru", "admin", "admin123", UserRole.Admin);
            int adminId = _dataAccess.AddUser(adminUser);
            UserAccount adminAccount = new UserAccount(adminUser);
            _dataAccess.AddUserAccount(adminAccount, adminId);

            // Создание тренера
            User coachUser = new User("Иванов Иван Иванович", "ivan@fitness.ru", "coach", "coach123", UserRole.Coach);
            int coachId = _dataAccess.AddUser(coachUser);
            UserAccount coachAccount = new UserAccount(coachUser);
            _dataAccess.AddUserAccount(coachAccount, coachId);

            // Создание клиента
            User clientUser = new User("Петров Петр Петрович", "petr@mail.ru", "client", "client123", UserRole.Client);
            int clientId = _dataAccess.AddUser(clientUser);
            UserAccount clientAccount = new UserAccount(clientUser);
            _dataAccess.AddUserAccount(clientAccount, clientId);
        }

        /// <summary>
        /// Добавляет тестовые подписки
        /// </summary>
        private void SeedSubscriptions()
        {
            // Подписка 1: Тренажерный зал
            Subscription subscription1 = new Subscription(
                "Тренажерный зал",
                85.00,
                "Абонемент на месяц с доступом к тренажерному залу",
                "/Images/iron1.jpg",
                "1 месяц",
                "Безлимит",
                new List<Review>
                {
                    new Review("Иван", 5, "Отличный зал, много оборудования!"),
                    new Review("Мария", 4, "Чисто и комфортно, но дороговато.")
                }
            );
            _dataAccess.AddSubscription(subscription1);

            // Подписка 2: Тренажерный зал - премиум
            Subscription subscription2 = new Subscription(
                "Тренажерный зал - премиум",
                850.00,
                "Годовой абонемент с неограниченным доступом ко всем зонам клуба",
                "/Images/iron1.jpg",
                "12 месяцев",
                "Обычный",
                new List<Review>
                {
                    new Review("Алексей", 5, "Лучший клуб в городе!"),
                    new Review("Ольга", 3, "Много людей в вечернее время.")
                }
            );
            _dataAccess.AddSubscription(subscription2);

            // Подписка 3: Тренажерный зал - 1 смена
            Subscription subscription3 = new Subscription(
                "Тренажерный зал - 1 смена",
                60.00,
                "Абонемент на утренние тренировки с 6:00 до 12:00",
                "/Images/gym3.jpg",
                "1 месяц",
                "Обычный",
                new List<Review>
                {
                    new Review("Дмитрий", 4, "Отличное время для занятий, мало людей.")
                }
            );
            _dataAccess.AddSubscription(subscription3);

            // Подписка 4: Тренажерный зал – Персональные тренировки
            Subscription subscription4 = new Subscription(
                "Тренажерный зал – Персональные тренировки",
                270.00,
                "Абонемент на 10 индивидуальных занятий с тренером",
                "/Images/gym2.jpg",
                "10 занятий",
                "Обычный",
                new List<Review>
                {
                    new Review("Светлана", 5, "Тренер профессионал, занятия очень полезные.")
                }
            );
            _dataAccess.AddSubscription(subscription4);

            // Подписка 5: Тренажерный зал - групповые тренировки
            Subscription subscription5 = new Subscription(
                "Тренажерный зал - групповые тренировки",
                100.00,
                "Абонемент на месяц с доступом к групповым тренировкам (йога, пилатес, зумба)",
                "/Images/yoga.jpg",
                "1 месяц",
                "Безлимит",
                new List<Review>
                {
                    new Review("Елена", 5, "Люблю групповые тренировки, тренеры супер!"),
                    new Review("Сергей", 4, "Йога – класс!")
                }
            );
            _dataAccess.AddSubscription(subscription5);
        }
    }
} 