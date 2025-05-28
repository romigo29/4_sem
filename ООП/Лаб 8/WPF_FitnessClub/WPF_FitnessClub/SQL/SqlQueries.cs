using System;

namespace WPF_FitnessClub.SQL
{

    public static class SqlQueries
    {
		#region Создание базы данных и таблиц

		public const string DropDatabaseIfExists = @"
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'FitnessClubDB')
BEGIN
    ALTER DATABASE FitnessClubDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE FitnessClubDB;
END";


		public const string CheckDatabaseExists = @"
SELECT COUNT(*) FROM sys.databases WHERE name = 'FitnessClubDB'";


        public const string CloseConnections = @"
ALTER DATABASE [FitnessClubDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";


        public const string DropDatabase = @"
DROP DATABASE FitnessClubDB";


        public const string CreateDatabase = @"
-- Создание базы данных FitnessClubDB
CREATE DATABASE FitnessClubDB";

   
        public const string CreateTables = @"
-- Создание таблицы Users
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FullName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        Login NVARCHAR(50) NOT NULL,
        Password NVARCHAR(100) NOT NULL,
        Role NVARCHAR(20) NOT NULL
    )
END

-- Создание таблицы UserAccounts
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserAccounts')
BEGIN
    CREATE TABLE UserAccounts (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        ProfileImagePath NVARCHAR(200) NULL,
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    )
END

-- Создание таблицы Subscriptions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Subscriptions')
BEGIN
    CREATE TABLE Subscriptions (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Price DECIMAL(10, 2) NOT NULL,
        Description NVARCHAR(500) NULL,
        ImagePath NVARCHAR(200) NULL,
        Duration NVARCHAR(15) NOT NULL, -- длительность в днях
        SubscriptionType NVARCHAR(50) NOT NULL
    )
END

-- Создание таблицы UserSubscriptions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserSubscriptions')
BEGIN
    CREATE TABLE UserSubscriptions (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserAccountId INT NOT NULL,
        SubscriptionId INT NOT NULL,
        PurchaseDate DATETIME NOT NULL DEFAULT GETDATE(),
        ExpiryDate DATETIME NOT NULL,
        FOREIGN KEY (UserAccountId) REFERENCES UserAccounts(Id) ON DELETE CASCADE,
        FOREIGN KEY (SubscriptionId) REFERENCES Subscriptions(Id)
    )
END

-- Создание таблицы Visits
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Visits')
BEGIN
    CREATE TABLE Visits (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserAccountId INT NOT NULL,
        VisitDate DATETIME NOT NULL DEFAULT GETDATE(),
        Duration INT NULL, -- длительность в минутах
        Activity NVARCHAR(100) NULL,
        TrainerName NVARCHAR(100) NULL,
        FOREIGN KEY (UserAccountId) REFERENCES UserAccounts(Id) ON DELETE CASCADE
    )
END

-- Создание таблицы Reviews
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE Reviews (
        Id INT PRIMARY KEY IDENTITY(1,1),
        SubscriptionId INT NOT NULL,
        UserName NVARCHAR(100) NOT NULL,
        Score INT NOT NULL CHECK (Score BETWEEN 1 AND 5),
        Comment NVARCHAR(500) NULL,
        ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (SubscriptionId) REFERENCES Subscriptions(Id) ON DELETE CASCADE
    )
END";

        #endregion

        #region Вставка тестовых данных

        /// <summary>
        /// SQL-запрос для вставки тестовых данных
        /// </summary>
        public const string InsertSampleData = @"
-- Вставка тестовых пользователей
-- Создание администратора
INSERT INTO Users (FullName, Email, Login, Password, Role)
VALUES ('Администратор Системы', 'admin@fitness.ru', 'admin', 'admin123', 'Admin');

-- Получение ID администратора
DECLARE @adminId INT = SCOPE_IDENTITY();

-- Создание аккаунта администратора
INSERT INTO UserAccounts (UserId, RegistrationDate, IsActive, ProfileImagePath)
VALUES (@adminId, GETDATE(), 1, NULL);

-- Создание тренера
INSERT INTO Users (FullName, Email, Login, Password, Role)
VALUES ('Иванов Иван Иванович', 'ivan@fitness.ru', 'coach', 'coach123', 'Coach');

-- Получение ID тренера
DECLARE @coachId INT = SCOPE_IDENTITY();

-- Создание аккаунта тренера
INSERT INTO UserAccounts (UserId, RegistrationDate, IsActive, ProfileImagePath)
VALUES (@coachId, GETDATE(), 1, NULL);

-- Создание клиента
INSERT INTO Users (FullName, Email, Login, Password, Role)
VALUES ('Петров Петр Петрович', 'petr@mail.ru', 'client', 'client123', 'Client');

-- Получение ID клиента
DECLARE @clientId INT = SCOPE_IDENTITY();

-- Создание аккаунта клиента
INSERT INTO UserAccounts (UserId, RegistrationDate, IsActive, ProfileImagePath)
VALUES (@clientId, GETDATE(), 1, NULL);

-- Вставка тестовых подписок
INSERT INTO Subscriptions (Name, Price, Description, ImagePath, Duration, SubscriptionType)
VALUES ('Тренажерный зал', 85.00, 'Абонемент на месяц с доступом к тренажерному залу', '/Images/iron1.jpg', 30, 'Безлимит');

-- Получение ID первой подписки
DECLARE @subscription1Id INT = SCOPE_IDENTITY();

-- Добавление отзывов к первой подписке
INSERT INTO Reviews (SubscriptionId, UserName, Score, Comment, ReviewDate)
VALUES (@subscription1Id, 'Иван', 5, 'Отличный зал, много оборудования!', GETDATE());

INSERT INTO Reviews (SubscriptionId, UserName, Score, Comment, ReviewDate)
VALUES (@subscription1Id, 'Мария', 4, 'Чисто и комфортно, но дороговато.', GETDATE());

-- Добавление еще одной подписки
INSERT INTO Subscriptions (Name, Price, Description, ImagePath, Duration, SubscriptionType)
VALUES ('Тренажерный зал - премиум', 850.00, 'Годовой абонемент с неограниченным доступом ко всем зонам клуба', '/Images/iron1.jpg', 365, 'Обычный');

-- Получение ID второй подписки
DECLARE @subscription2Id INT = SCOPE_IDENTITY();

-- Добавление отзывов ко второй подписке
INSERT INTO Reviews (SubscriptionId, UserName, Score, Comment, ReviewDate)
VALUES (@subscription2Id, 'Алексей', 5, 'Лучший клуб в городе!', GETDATE());

INSERT INTO Reviews (SubscriptionId, UserName, Score, Comment, ReviewDate)
VALUES (@subscription2Id, 'Ольга', 3, 'Много людей в вечернее время.', GETDATE());

-- Добавление пользовательской подписки для клиента
-- Получаем ID аккаунта клиента
DECLARE @clientAccountId INT;
SELECT @clientAccountId = Id FROM UserAccounts WHERE UserId = @clientId;

-- Добавляем подписку для клиента
INSERT INTO UserSubscriptions (UserAccountId, SubscriptionId, PurchaseDate, ExpiryDate)
VALUES (@clientAccountId, @subscription1Id, GETDATE(), DATEADD(DAY, 30, GETDATE()));

-- Добавление посещения для клиента
INSERT INTO Visits (UserAccountId, VisitDate, Duration, Activity, TrainerName)
VALUES (@clientAccountId, GETDATE(), 90, 'Тренировка на силу', 'Иванов И.И.')";

        #endregion

        #region Запросы для работы с пользователями

        /// <summary>
        /// SQL-запрос для получения пользователя по логину и паролю
        /// </summary>
        public const string GetUserByLoginAndPassword = @"
SELECT * FROM Users WHERE Login = @Login AND Password = @Password";

        /// <summary>
        /// SQL-запрос для добавления нового пользователя
        /// </summary>
        public const string AddUser = @"
INSERT INTO Users (FullName, Email, Login, Password, Role)
VALUES (@FullName, @Email, @Login, @Password, @Role);
SELECT SCOPE_IDENTITY()";

        /// <summary>
        /// SQL-запрос для добавления аккаунта пользователя
        /// </summary>
        public const string AddUserAccount = @"
INSERT INTO UserAccounts (UserId, RegistrationDate, IsActive, ProfileImagePath)
VALUES (@UserId, @RegistrationDate, @IsActive, @ProfileImagePath);
SELECT SCOPE_IDENTITY()";

        #endregion

        #region Запросы для работы с подписками

        /// <summary>
        /// SQL-запрос для получения всех подписок
        /// </summary>
        public const string GetAllSubscriptions = @"
SELECT * FROM Subscriptions";

        /// <summary>
        /// SQL-запрос для получения подписки по ID
        /// </summary>
        public const string GetSubscriptionById = @"
SELECT * FROM Subscriptions WHERE Id = @Id";

        /// <summary>
        /// SQL-запрос для добавления новой подписки
        /// </summary>
        public const string AddSubscription = @"
INSERT INTO Subscriptions (Name, Price, Description, ImagePath, Duration, SubscriptionType)
VALUES (@Name, @Price, @Description, @ImagePath, @Duration, @SubscriptionType);
SELECT SCOPE_IDENTITY()";

        #endregion

        #region Запросы для работы с отзывами

        /// <summary>
        /// SQL-запрос для получения отзывов по ID подписки
        /// </summary>
        public const string GetReviewsBySubscriptionId = @"
SELECT * FROM Reviews WHERE SubscriptionId = @SubscriptionId ORDER BY ReviewDate DESC";

        /// <summary>
        /// SQL-запрос для добавления нового отзыва
        /// </summary>
        public const string AddReview = @"
INSERT INTO Reviews (SubscriptionId, UserName, Score, Comment, ReviewDate)
VALUES (@SubscriptionId, @UserName, @Score, @Comment, @ReviewDate);
SELECT SCOPE_IDENTITY()";

        #endregion
        
        #region Хранимые процедуры
        
        /// <summary>
        /// SQL-запрос для создания хранимой процедуры получения пользователя по логину и паролю
        /// </summary>
        public const string CreateProcGetUserByLoginAndPassword = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_GetUserByLoginAndPassword')
    DROP PROCEDURE custom_GetUserByLoginAndPassword
GO

CREATE PROCEDURE custom_GetUserByLoginAndPassword
    @Login NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Users WHERE Login = @Login AND Password = @Password
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры добавления нового пользователя
        /// </summary>
        public const string CreateProcAddUser = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_AddUser')
    DROP PROCEDURE custom_AddUser
GO

CREATE PROCEDURE custom_AddUser
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Login NVARCHAR(50),
    @Password NVARCHAR(100),
    @Role NVARCHAR(20)
AS
BEGIN
    INSERT INTO Users (FullName, Email, Login, Password, Role)
    VALUES (@FullName, @Email, @Login, @Password, @Role);
    
    SELECT SCOPE_IDENTITY()
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры добавления аккаунта пользователя
        /// </summary>
        public const string CreateProcAddUserAccount = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_AddUserAccount')
    DROP PROCEDURE custom_AddUserAccount
GO

CREATE PROCEDURE custom_AddUserAccount
    @UserId INT,
    @RegistrationDate DATETIME,
    @IsActive BIT,
    @ProfileImagePath NVARCHAR(200)
AS
BEGIN
    INSERT INTO UserAccounts (UserId, RegistrationDate, IsActive, ProfileImagePath)
    VALUES (@UserId, @RegistrationDate, @IsActive, @ProfileImagePath);
    
    SELECT SCOPE_IDENTITY()
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры получения всех подписок
        /// </summary>
        public const string CreateProcGetAllSubscriptions = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_GetAllSubscriptions')
    DROP PROCEDURE custom_GetAllSubscriptions
GO

CREATE PROCEDURE custom_GetAllSubscriptions
AS
BEGIN
    SELECT * FROM Subscriptions
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры получения подписки по ID
        /// </summary>
        public const string CreateProcGetSubscriptionById = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_GetSubscriptionById')
    DROP PROCEDURE custom_GetSubscriptionById
GO

CREATE PROCEDURE custom_GetSubscriptionById
    @Id INT
AS
BEGIN
    SELECT * FROM Subscriptions WHERE Id = @Id
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры добавления новой подписки
        /// </summary>
        public const string CreateProcAddSubscription = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_AddSubscription')
    DROP PROCEDURE custom_AddSubscription
GO

CREATE PROCEDURE custom_AddSubscription
    @Name NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @Description NVARCHAR(500),
    @ImagePath NVARCHAR(200),
    @Duration NVARCHAR(15),
    @SubscriptionType NVARCHAR(50)
AS
BEGIN
    INSERT INTO Subscriptions (Name, Price, Description, ImagePath, Duration, SubscriptionType)
    VALUES (@Name, @Price, @Description, @ImagePath, @Duration, @SubscriptionType);
    
    SELECT SCOPE_IDENTITY()
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры обновления подписки
        /// </summary>
        public const string CreateProcUpdateSubscription = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_UpdateSubscription')
    DROP PROCEDURE custom_UpdateSubscription
GO

CREATE PROCEDURE custom_UpdateSubscription
    @SubscriptionId INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @Description NVARCHAR(500),
    @ImagePath NVARCHAR(200),
    @Duration NVARCHAR(15),
    @SubscriptionType NVARCHAR(50)
AS
BEGIN
    UPDATE Subscriptions
    SET Name = @Name,
        Price = @Price,
        Description = @Description,
        ImagePath = @ImagePath,
        Duration = @Duration,
        SubscriptionType = @SubscriptionType
    WHERE Id = @SubscriptionId
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры удаления подписки
        /// </summary>
        public const string CreateProcDeleteSubscription = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_DeleteSubscription')
    DROP PROCEDURE custom_DeleteSubscription
GO

CREATE PROCEDURE custom_DeleteSubscription
    @SubscriptionId INT
AS
BEGIN
    -- Удаляем связанные отзывы
    DELETE FROM Reviews WHERE SubscriptionId = @SubscriptionId;
    
    -- Удаляем связи с пользовательскими аккаунтами
    DELETE FROM UserSubscriptions WHERE SubscriptionId = @SubscriptionId;
    
    -- Удаляем подписку
    DELETE FROM Subscriptions WHERE Id = @SubscriptionId;
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры получения отзывов по ID подписки
        /// </summary>
        public const string CreateProcGetReviewsBySubscriptionId = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_GetReviewsBySubscriptionId')
    DROP PROCEDURE custom_GetReviewsBySubscriptionId
GO

CREATE PROCEDURE custom_GetReviewsBySubscriptionId
    @SubscriptionId INT
AS
BEGIN
    SELECT * FROM Reviews 
    WHERE SubscriptionId = @SubscriptionId 
    ORDER BY ReviewDate DESC
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры добавления нового отзыва
        /// </summary>
        public const string CreateProcAddReview = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_AddReview')
    DROP PROCEDURE custom_AddReview
GO

CREATE PROCEDURE custom_AddReview
    @SubscriptionId INT,
    @UserName NVARCHAR(100),
    @Score INT,
    @Comment NVARCHAR(500)
AS
BEGIN
    INSERT INTO Reviews (SubscriptionId, UserName, Score, Comment, ReviewDate)
    VALUES (@SubscriptionId, @UserName, @Score, @Comment, GETDATE());
    
    SELECT SCOPE_IDENTITY()
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры удаления отзыва
        /// </summary>
        public const string CreateProcDeleteReview = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_DeleteReview')
    DROP PROCEDURE custom_DeleteReview
GO

CREATE PROCEDURE custom_DeleteReview
    @ReviewId INT
AS
BEGIN
    DELETE FROM Reviews WHERE Id = @ReviewId
END";

        /// <summary>
        /// SQL-запрос для создания хранимой процедуры фильтрации подписок
        /// </summary>
        public const string CreateProcFilterSubscriptions = @"
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'custom_FilterSubscriptions')
    DROP PROCEDURE custom_FilterSubscriptions
GO

CREATE PROCEDURE custom_FilterSubscriptions
    @Name NVARCHAR(100) = NULL,
    @MinPrice DECIMAL(10, 2) = 0,
    @MaxPrice DECIMAL(10, 2) = 100000,
    @Type NVARCHAR(50) = NULL,
    @Duration NVARCHAR(15) = NULL
AS
BEGIN
    SELECT Id, Name, Price, Description, ImagePath, Duration, SubscriptionType
    FROM Subscriptions
    WHERE (@Name IS NULL OR Name LIKE '%' + @Name + '%')
      AND Price BETWEEN @MinPrice AND @MaxPrice
      AND (@Type IS NULL OR SubscriptionType = @Type)
      AND (@Duration IS NULL OR Duration = @Duration)
END";

        #endregion
    }
} 