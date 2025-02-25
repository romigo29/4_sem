CREATE TABLE Заказчики(
Наименование_фирмы nvarchar(20) NOT NULL,
Адрес nvarchar(50) NOT NULL,
Расчетный_счет nvarchar(15),
CONSTRAINT РК_Наименование_фирмы PRIMARY KEY(Наименование_фирмы))
