create view Ноутбуки
as select Наименование_товара Ноутбук,
Цена[Цена]
from ТОВАР
where Наименование_товара like '%Ноутбук%'

select * from Ноутбуки
select * from ТОВАР