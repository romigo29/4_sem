use R_MyBase

select Номер_товара
from УСЛУГА 
where Количество in (select top(1) Количество from УСЛУГА) 


use R_MyBase

select * from ПОКУПАТЕЛЬ	
select * from склад
select * from товар
select * from УСЛУГА