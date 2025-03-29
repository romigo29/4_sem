create view Дорогие_товары
as select top(5) Наименование_товара,
Цена[Цена]
from ТОВАР
where Цена > 1000
order by Цена
go

select * from Дорогие_товары

