Use R_MyBase

select *
from
(select Case
when ���� < 1000 then '������ 1000'
when ���� between 1000 and 3000 then '����� 1000 � 3000'
when ���� > 3000 then '������ 3000'
end as [��������],
count(*) [����������]
from �����

group by Case
when ���� < 1000 then '������ 1000'
when ���� between 1000 and 3000 then '����� 1000 � 3000'
when ���� > 3000 then '������ 3000'
end) as T

order by Case [��������]
when '������ 1000' then 3
when '����� 1000 � 3000' then 2
when '������ 3000' then 1
end