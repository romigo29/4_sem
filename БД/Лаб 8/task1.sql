create view Преподаватель
as select t.TEACHER [Код],
t.TEACHER_NAME [Имя], t.GENDER[Пол], t.PULPIT[Кафедра]
from TEACHER t
go

select * from Преподаватель
go

drop view Преподаватель
go
