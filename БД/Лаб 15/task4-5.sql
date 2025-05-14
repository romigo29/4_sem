create trigger TR_TEACHER on TEACHER after INSERT, DELETE, UPDATE
as 
	declare @a1 char(10), @a2  nvarchar(100), @a3 char(1), @a4 char(20), @in nvarchar(300);
	declare @ins int = (select count(*) from inserted),
            @del int = (select count(*) from deleted); 

if  @ins > 0 and  @del = 0  
begin 
	print 'Операция вставки';
	set @a1 = (select [Teacher] from inserted);
	set @a2 = (select [Teacher_Name] from inserted);
	set @a3 = (select [Gender] from inserted);
	set @a4 = (select [Pulpit] from inserted);
	set @in = @a1 + ',' + @a2 + ', ' + @a3 + ', ' + @a4
	insert into TR_AUDIT (stmt, trname, cc)
	values ('INS', 'TR_TEACHER_INS', @in);
end; 

else	
if @ins = 0 and  @del > 0  
begin 
	print 'Операция удаления';
	set @a1 = (select [Teacher] from deleted);
	set @a2 = (select [Teacher_Name] from deleted);
	set @a3 = (select [Gender] from deleted);
	set @a4 = (select [Pulpit] from deleted);
	set @in = @a1 + ',' + @a2 + ', ' + @a3 + ', ' + @a4
	insert into TR_AUDIT (stmt, trname, cc)
	values ('DEL', 'TR_TEACHER_DEL', @in);
end; 
else	  
if @ins > 0 and  @del > 0  
begin 
	print 'Операция обновления';

	set @a1 = (select [Teacher] from inserted);
	set @a2 = (select [Teacher_Name] from inserted);
	set @a3 = (select [Gender] from inserted);
	set @a4 = (select [Pulpit] from inserted);
	set @in = @a1 + ',' + @a2 + ', ' + @a3 + ', ' + @a4

	set @a1 = (select [Teacher] from deleted);
	set @a2 = (select [Teacher_Name] from deleted);
	set @a3 = (select [Gender] from deleted);
	set @a4 = (select [Pulpit] from deleted);
	set @in = @a1 + ',' + @a2 + ', ' + @a3 + ', ' + @a4 + @in
	insert into TR_AUDIT (stmt, trname, cc)
	values ('UPD', 'TR_TEACHER_UPD', @in);
	end

return;


insert into TEACHER
values('РМНВ', 'Романов Игорь Вячеславович', 'м', 'ИСиТ')

delete from TEACHER where TEACHER = 'РМНВ'

update TEACHER set TEACHER = 'СМЛВ' where TEACHER = 'СМЛ'

---task5
insert into TEACHER values ('test', 'test', 'н', 'ИСиТ')
-----------------

select * from TR_AUDIT

select * from TEACHER
delete from TR_AUDIT
drop trigger TR_TEACHER_UPD
drop trigger TR_TEACHER_INS
drop trigger TR_TEACHER_DEL
drop trigger TR_TEACHER
	