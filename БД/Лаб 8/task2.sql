create view ����������_������
as select p.FACULTY [���������],
count(*) [���������� ������]
from PULPIT p
join FACULTY f on p.FACULTY = f.FACULTY
group by p.FACULTY
go

select * from ����������_������
go

drop view ����������_������
go


--task6
alter view ����������_������ with SCHEMABINDING
as select p.FACULTY [���������],
count(*) [���������� ������]
from dbo.PULPIT p
join dbo.FACULTY f on p.FACULTY = f.FACULTY
group by p.FACULTY
go

select * from ����������_������

alter table dbo.PULPIT
drop column FACULTY

