create function FCTEACHER(@pulpit varchar(20)) returns int
as begin
	declare @rc int = (select count(*) from TEACHER t
	where t.PULPIT = isnull(@pulpit, t.pulpit));
	
	return @rc
	end;

	select PULPIT, dbo.FCTEACHER(PULPIT) [����������]
	from PULPIT

	select dbo.FCTEACHER(null) [����������]
	from PULPIT

	print '����� ��������������: ' + cast(dbo.FCTEACHER(null) as varchar(20))
