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
VALUES (@clientAccountId, GETDATE(), 90, 'Тренировка на силу', 'Иванов И.И.'); 