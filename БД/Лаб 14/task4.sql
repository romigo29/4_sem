create function FCTEACHER(@pulpit varchar(20)) returns int
as begin
	declare @rc int = (select count(*) from TEACHER t
	where t.PULPIT = isnull(@pulpit, t.pulpit));
	
	return @rc
	end;

	select PULPIT, dbo.FCTEACHER(PULPIT) [Количество]
	from PULPIT

	select dbo.FCTEACHER(null) [Количество]
	from PULPIT

	print 'Всего преподавателей: ' + cast(dbo.FCTEACHER(null) as varchar(20))
