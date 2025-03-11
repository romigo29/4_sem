use R_MyBase

select Наименование_товара, Цена
from ТОВАР
where Цена > any (select Цена from ТОВАР
where Наименование_товара like '%Клавиатура%')

use R_MyBase

select * from ПОКУПАТЕЛЬ	
select * from склад
select * from товар
select * from УСЛУГА
