--�������� �������
select * into AUDITORIUM_COPY from AUDITORIUM
alter table AUDITORIUM_COPY
alter column AUDITORIUM_TYPE char(10) null
go

create view ���������
as select a.AUDITORIUM[���],
a.AUDITORIUM_NAME[������������_���������]
from AUDITORIUM_COPY a
where a.AUDITORIUM_TYPE like '%��%'
go

select * from ���������
select * from AUDITORIUM_COPY	

--���
drop view dbo.���������
drop table AUDITORIUM_COPY
go

insert into ���������
values ('322-1', '322-1')

delete from AUDITORIUM_COPY
where AUDITORIUM like '%408-2%'

delete from AUDITORIUM_COPY
where AUDITORIUM like '%322-1%'

update ��������� 
set ��� = '322-1',
������������_��������� = '322-1'
where ��� = '236-1'

