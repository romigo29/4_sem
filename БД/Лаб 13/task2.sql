--TASK2
alter procedure PSUBJECT @p varchar(20), @c int output
as begin
	declare @k int = (select count(*) from SUBJECT)

	select SUBJECT [���],
	SUBJECT_NAME [����������],
	PULPIT [�������] from SUBJECT
	where SUBJECT = @p
	set @c = @@ROWCOUNT
	return @k
end;

declare @k int = 0, @r int = 0, @p varchar(20)
set @p = '��'
exec @k = PSUBJECT @p, @c = @r output
print '���-�� ��������� ����� = ' + cast(@k as varchar(3))
print '���-�� ����� � ��������� ' + cast (@p as varchar(3)) + '=' + cast(@r as varchar(3))