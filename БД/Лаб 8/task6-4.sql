create view Ноутбуки
as select Наименование_товара Ноутбук,
Цена[Цена]
from ТОВАР
where Наименование_товара like '%Ноутбук%'
with CHECK OPTION

select * from Ноутбуки
select * from ТОВАР

drop view Ноутбуки