--TASK1
set nocount on
if  exists (select * from  SYS.OBJECTS        
	where OBJECT_ID= object_id(N'DBO.Y') )
drop table Y;           

declare @c int, @flag char = 'c';         
SET IMPLICIT_TRANSACTIONS  ON   -- �����. ����� ������� ����������
CREATE table Y(text nvarchar(50));                          
INSERT Y values ('���'),('���'),('���');
set @c = (select count(*) from Y);
print '���������� ����� � ������� Y: ' + cast( @c as varchar(2));
if @flag = 'c'  commit;                   -- ���������� ����������: �������� 
else   rollback;                                 -- ���������� ����������: �����  
SET IMPLICIT_TRANSACTIONS  OFF   -- ������. ����� ������� ����������
	
if  exists (select * from  SYS.OBJECTS       
	where OBJECT_ID= object_id(N'DBO.Y') )
print '������� Y ����';  
else print '������� Y ���'



--TASK2
drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT
alter table PULPIT_COPY
add constraint PK_PULPIT_COPY primary key (PULPIT);

begin try
	begin tran
	delete PULPIT_COPY where FACULTY = '���'
	insert PULPIT_COPY values ('��', '����������� ���������', '��')
	insert PULPIT_COPY values ('��', '����������� ���������', '��')
	commit tran
	print 'ok'
end try
	
begin catch
print '������: �' + cast(error_number() as varchar(5)) + ' ' + cast(error_message() as varchar(5))

if @@TRANCOUNT > 0 rollback tran;
end catch

select * from PULPIT_COPY



--TASK3
drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT
alter table PULPIT_COPY
add constraint PK_PULPIT_COPY primary key (PULPIT);

declare @point varchar(32)

begin try
	begin tran
	delete PULPIT_COPY where FACULTY = '���'
	set @point = 'p1'; save tran @point
	insert PULPIT_COPY values ('��', '����������� ���������', '��')
	set @point = 'p2'; save tran @point
	insert PULPIT_COPY values ('��', '����������� ���������', '��')
	set @point = 'p3'; save tran @point
	commit tran
	print 'ok'
end try
	
begin catch
print '������: �' + cast(error_number() as varchar(5)) + ' ' + cast(error_message() as varchar(5))

if @@TRANCOUNT > 0
	begin
		print '����������� �����: ' + @point 
		rollback tran @point;
		commit tran
	end;
end catch


