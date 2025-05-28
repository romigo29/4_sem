-- Создание базы данных FitnessClubDB
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FitnessClubDB')
BEGIN
    CREATE DATABASE FitnessClubDB
END 