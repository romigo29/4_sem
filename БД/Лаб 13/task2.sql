--TASK2
alter procedure PSUBJECT @p varchar(20), @c int output
as begin
	declare @k int = (select count(*) from SUBJECT)

	select SUBJECT [код],
	SUBJECT_NAME [дисциплина],
	PULPIT [кафедра] from SUBJECT
	where SUBJECT = @p
	set @c = @@ROWCOUNT
	return @k
end;

declare @k int = 0, @r int = 0, @p varchar(20)
set @p = 'БД'
exec @k = PSUBJECT @p, @c = @r output
print 'кол-во предметов всего = ' + cast(@k as varchar(3))
print 'кол-во строк с предметов ' + cast (@p as varchar(3)) + '=' + cast(@r as varchar(3))