alter procedure PSUBJECT @p varchar(20)
as begin
	select * from SUBJECT where SUBJECT = @p
	end;

create table #SUBJECT
( ��� char(10) primary key,
���������� nvarchar(100),
������� char(20))

insert #SUBJECT exec PSUBJECT @p = '��'
insert #SUBJECT exec PSUBJECT @p = '���'

select * from #SUBJECT

