CREATE TABLE ЗАКАЗЫ(
Номер_заказа int PRIMARY KEY,
Наименование_товара nvarchar(20) foreign key references ТОВАРЫ(Наименование),
Цена_продажи real,
Количество int not null,
Дата_поставки date not null,
Заказчик nvarchar(20) foreign key references ЗАКАЗЧИКИ(Наименование_фирмы))
