use R_MyBase

select Наименование_товара
from ТОВАР
where not exists (select * from УСЛУГА
WHERE ТОВАР.Артикул = УСЛУГА.Номер_товара)