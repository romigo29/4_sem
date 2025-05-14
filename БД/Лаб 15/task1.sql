create table TR_AUDIT
(
	ID int identity,	--номер
	stmt varchar(20)	-- DML-оператор 
	check (STMT in ('INS', 'DEL', 'UPD')),
	trname varchar(50),		--имя триггера
	cc varchar(300)			--комментарий
)
go

create trigger TR_TEACHER_INS on TEACHER after INSERT
as 
	declare @a1 char(10), @a2  nvarchar(100), @a3 char(1), @a4 char(20), @in nvarchar(300);

	print 'Операция вставки';
	set @a1 = (select [Teacher] from inserted);
	set @a2 = (select [Teacher_Name] from inserted);
	set @a3 = (select [Gender] from inserted);
	set @a4 = (select [Pulpit] from inserted);
	set @in = @a1 + ',' + @a2 + ', ' + @a3 + ', ' + @a4
	insert into TR_AUDIT (stmt, trname, cc)
	values ('INS', 'TR_TEACHER_INS', @in);
	return;

insert into TEACHER
values('РМНВ', 'Романов Игорь Вячеславович', 'м', 'ИСиТ')
select * from TR_AUDIT


delete from TEACHER where TEACHER = 'РМНВ'
delete from TR_AUDIT
select * from TEACHER
drop trigger TR_TEACHER_INS
drop table  TR_AUDIT


