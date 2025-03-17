use Univer

select * from (select 
Case
when NOTE between 4 and 5 then 'между 4 и 5'
when NOTE between 6 and 7 then 'между 6 и 7'
when NOTE between 8 and 9 then 'между 8 и 9'
else '10'
end as Интервал,
count(*)[Количество]
from PROGRESS
group by Case
when NOTE between 4 and 5 then 'между 4 и 5'
when NOTE between 6 and 7 then 'между 6 и 7'
when NOTE between 8 and 9 then 'между 8 и 9'
else '10'
end) as T

order by Case[Интервал]
when 'между 4 и 5' then 3
when 'между 6 и 7' then 2
when 'между 8 и 9' then 1
else '10'
end

