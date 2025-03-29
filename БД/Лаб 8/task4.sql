--основное задание
select * into AUDITORIUM_COPY from AUDITORIUM

alter table AUDITORIUM_COPY
alter column AUDITORIUM_TYPE char(10) null

create view Лекционные_аудитории
as select a.AUDITORIUM[Код],
a.AUDITORIUM_NAME[Наименование_аудитории]
from AUDITORIUM_COPY a
where a.AUDITORIUM_TYPE like '%ЛК%'
with check option
go

select * from Лекционные_аудитории
select * from AUDITORIUM_COPY	

--доп
drop view dbo.Лекционные_аудитории
drop table AUDITORIUM_COPY
go

insert into Лекционные_аудитории
values ('304-1', '304-1')

delete from AUDITORIUM_COPY
where AUDITORIUM like '%408-2%'

delete from AUDITORIUM_COPY
where AUDITORIUM like '%322-1%'

update Лекционные_аудитории 
set Код = '322-1',
Наименование_аудитории = '322-1'
where Код = '236-1'

