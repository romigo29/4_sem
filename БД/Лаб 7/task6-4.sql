use R_MyBase

select t.������������_������,
s.�����_��������,
round(avg(cast(t.���� as float(1))), 2) [������� ����]
from ����� t
join ����� s on t.������� = s.�������_������
join ������ u on u.�����_������ = t.�������
where t.���� > 100
group by t.������������_������, s.�����_��������

intersect

select t.������������_������,
s.�����_��������,
round(avg(cast(t.���� as float(1))), 2) [������� ����]
from ����� t
join ����� s on t.������� = s.�������_������
join ������ u on u.�����_������ = t.�������
where t.���� < 2000
group by t.������������_������, s.�����_��������