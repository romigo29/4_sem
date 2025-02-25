CREATE TABLE ТОВАРЫ(
Наименование nvarchar(20) not null,
Цена real not null,
Количество int not null,
CONSTRAINT РК_Наименование PRIMARY KEY(Наименование))
