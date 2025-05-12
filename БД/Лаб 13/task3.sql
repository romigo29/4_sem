alter procedure PSUBJECT @p varchar(20)
as begin
	select * from SUBJECT where SUBJECT = @p
	end;

create table #SUBJECT
( Код char(10) primary key,
Дисциплина nvarchar(100),
Кафедра char(20))

insert #SUBJECT exec PSUBJECT @p = 'БД'
insert #SUBJECT exec PSUBJECT @p = 'ООП'

select * from #SUBJECT

