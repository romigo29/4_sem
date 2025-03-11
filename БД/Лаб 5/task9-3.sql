use R_MyBase

select Наименование_товара
from УСЛУГА 
join ТОВАР on УСЛУГА.Номер_товара = ТОВАР.Артикул
join СКЛАД on УСЛУГА.Номер_товара = СКЛАД.Артикул_товара
where Номер_товара in (select Артикул_товара from СКЛАД
where Место_хранения like '%Склад A%')

use R_MyBase

select * from ПОКУПАТЕЛЬ	
select * from склад
select * from товар
select * from УСЛУГА