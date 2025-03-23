use R_MyBase

select t.Наименование_товара,
s.Место_хранения,
round(avg(cast(t.Цена as float(1))), 2) [Средняя цена]
from ТОВАР t
join СКЛАД s on t.Артикул = s.Артикул_товара
join УСЛУГА u on u.Номер_товара = t.Артикул
where t.Цена > 100

group by rollup(t.Наименование_товара, s.Место_хранения)