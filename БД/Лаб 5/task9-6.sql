use R_MyBase

select top(1)
	(select avg(Цена) from Товар)[Средняя цена товаров]

