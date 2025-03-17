Use R_MyBase

select *
from
(select Case
when Цена < 1000 then 'меньше 1000'
when Цена between 1000 and 3000 then 'между 1000 и 3000'
when Цена > 3000 then 'больше 3000'
end as [Интервал],
count(*) [Количество]
from ТОВАР

group by Case
when Цена < 1000 then 'меньше 1000'
when Цена between 1000 and 3000 then 'между 1000 и 3000'
when Цена > 3000 then 'больше 3000'
end) as T

order by Case [Интервал]
when 'меньше 1000' then 3
when 'между 1000 и 3000' then 2
when 'больше 3000' then 1
end