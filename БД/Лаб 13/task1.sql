--TASK1
drop procedure PSUBJECT
go
create procedure PSUBJECT
as begin
	declare @amount int = (select count(*) from SUBJECT)
	select SUBJECT [���],
	SUBJECT_NAME [����������],
	PULPIT [�������] from SUBJECT
	return @amount
end;

declare @output int = 0
exec @output = PSUBJECT
print '���-�� ��������=' + cast(@output as varchar(5))





