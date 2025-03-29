create view Количество_товаров
as select Наименование_товара[Товар],
count(*) [Количество]
from СКЛАД
join ТОВАР on СКЛАД.Артикул_товара = ТОВАР.Артикул
group by ТОВАР.Наименование_товара
go

select * from Количество_товаров

alter view Количество_товаров with SCHEMABINDING
as select Наименование_товара[Товар],
count(*) [Количество]
from dbo.СКЛАД
join dbo.ТОВАР on dbo.СКЛАД.Артикул_товара = dbo.ТОВАР.Артикул
group by dbo.ТОВАР.Наименование_товара
go