USE master
CREATE database [Romanov_MyBase]
ON PRIMARY 
(NAME =  N'Romanov_MyBase',
FILENAME = N'D:\BSTU\4 семестр\БД\Лаб 3\Romanov_MyBase.mdf')

LOG ON 
(NAME = N'Romanov_MyBase_log',
FILENAME = N'D:\BSTU\4 семестр\БД\Лаб 3\Romanov_MyBase_log.ldf')

GO
