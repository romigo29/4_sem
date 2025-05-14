create trigger TR_TEACHER_DEL on TEACHER after DELETE
as 
	declare @a1 char(10), @a2  nvarchar(100), @a3 char(1), @a4 char(20), @in nvarchar(300);
	print 'Операция удаления';
	set @a1 = (select [Teacher] from deleted);
	set @a2 = (select [Teacher_Name] from deleted);
	set @a3 = (select [Gender] from deleted);
	set @a4 = (select [Pulpit] from deleted);
	set @in = @a1 + ',' + @a2 + ', ' + @a3 + ', ' + @a4
	insert into TR_AUDIT (stmt, trname, cc)
	values ('DEL', 'TR_TEACHER_DEL', @in);
	return;

delete from TEACHER where TEACHER = 'РМНВ'
select * from TR_AUDIT


select * from TEACHER

delete from TR_AUDIT
drop trigger TR_TEACHER_DEL
	