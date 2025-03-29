--основное задание
select * into AUDITORIUM_COPY from AUDITORIUM
alter table AUDITORIUM_COPY
alter column AUDITORIUM_TYPE char(10) null
go

create view Аудитории
as select a.AUDITORIUM[Код],
a.AUDITORIUM_NAME[Наименование_аудитории]
from AUDITORIUM_COPY a
where a.AUDITORIUM_TYPE like '%ЛК%'
go

select * from Аудитории
select * from AUDITORIUM_COPY	

--доп
drop view dbo.Аудитории
drop table AUDITORIUM_COPY
go

insert into Аудитории
values ('322-1', '322-1')

delete from AUDITORIUM_COPY
where AUDITORIUM like '%408-2%'

delete from AUDITORIUM_COPY
where AUDITORIUM like '%322-1%'

update Аудитории 
set Код = '322-1',
Наименование_аудитории = '322-1'
where Код = '236-1'

