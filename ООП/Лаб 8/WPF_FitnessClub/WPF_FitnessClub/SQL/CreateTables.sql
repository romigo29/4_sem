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
END