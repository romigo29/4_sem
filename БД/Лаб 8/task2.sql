create view Количество_кафедр
as select p.FACULTY [Факультет],
count(*) [Количество кафедр]
from PULPIT p
join FACULTY f on p.FACULTY = f.FACULTY
group by p.FACULTY
go

select * from Количество_кафедр
go

drop view Количество_кафедр
go


--task6
alter view Количество_кафедр with SCHEMABINDING
as select p.FACULTY [Факультет],
count(*) [Количество кафедр]
from dbo.PULPIT p
join dbo.FACULTY f on p.FACULTY = f.FACULTY
group by p.FACULTY
go

select * from Количество_кафедр

alter table dbo.PULPIT
drop column FACULTY

