--task2
drop table �����_COPY
select * into �����_COPY from �����
alter table �����_COPY
add constraint PK_�����_COPY primary key (�������);

begin try
	begin tran
	delete �����_COPY where ������������_������ like '%�������%'
	insert �����_COPY values ('PRD-202402-005',	'���������� RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	insert �����_COPY values ('PRD-202402-005',	'���������� RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	commit tran
	print 'ok'
end try
	
begin catch
print '������: �' + cast(error_number() as varchar(5)) + ' ' + cast(error_message() as varchar(5))

if @@TRANCOUNT > 0 rollback tran;
end catch

select * from �����_COPY


--task3
drop table �����_COPY
select * into �����_COPY from �����
alter table �����_COPY
add constraint PK_�����_COPY primary key (�������);

declare @point varchar(32)

begin try
	begin tran
	delete �����_COPY where ������������_������ like '%�������%'
	set @point = 'p1' save tran @point
	insert �����_COPY values ('PRD-202402-005',	'���������� RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	set @point = 'p2' save tran @point
	insert �����_COPY values ('PRD-202402-005',	'���������� RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	set @point = 'p3' save tran @point
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



--TASK4
drop table �����_COPY
select * into �����_COPY from �����
-- A ---
	set transaction isolation level READ UNCOMMITTED 
	begin transaction 
-------------------------- t1 ------------------
	select count(*) from �����_COPY where ������������_������ like '%����������%'
	commit; 
	-------------------------- t2 -----------------

--TASK5
-- A ---
	set transaction isolation level READ COMMITTED 
	begin transaction 
	select count(*) from �����_COPY where ������������_������ like '%����������%'
	-------------------------- t1 -----------------
	-------------------------- t2 -----------------
	select '���������', count(*) from �����_COPY where ������������_������ like '%����������%'
	commit

--TASK6
-- A ---
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from �����_COPY where ������������_������ like '%����������%'
------------------------------ t1 -----------------
	commit; 
------------------------------ t2 -----------------
	insert �����_COPY values ('PRD-202402-005', '���������� RTX 4070 Ti', 2900, '6GB GDDR6X, DLSS 3')
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from �����_COPY where ������������_������ like '%����������%'
----------------------------------------------------
	select count(*) from �����_COPY where ������������_������ like '%����������%'
	commit; 

--TASK7
-- A ---
	set transaction isolation level SERIALIZABLE 
	begin transaction
	select count(*) from �����_COPY where ������������_������ like '%����������%'
	-------------------------- t1 -----------------
	commit; 
	-------------------------- t2 -----------------


--TASK4 B-----------
drop table �����_COPY
select * into �����_COPY from �����

--TASK4
--- B --	

	begin transaction 
	delete �����_COPY where ������������_������ like '%����������%'
	-------------------------- t1 --------------------
	-------------------------- t2 --------------------
	rollback;

--TASK5
--- B --	
-------------------------- t1 ------------------
	begin transaction
	delete �����_COPY where ������������_������ like '%����������%'

	-------------------------- t2 --------------------
	rollback
--TASK6
--- B --	
	begin transaction 
	delete �����_COPY where ������������_������ like '%����������%'
-------------------------- t1 ------------------
	commit
-------------------------- t2 (��������� ������) --------------------
	begin transaction 
	insert �����_COPY values ('PRD-202402-005', '���������� RTX 4070 Ti', 2900, '6GB GDDR6X, DLSS 3')
	commit
	-----------------------

--TASK7
--- B --	
	begin transaction 
	insert �����_COPY values ('PRD-202402-005', '���������� RTX 4070 Ti', 2900, '6GB GDDR6X, DLSS 3')
	commit
-------------------------- t1 ------------------
	select count(*) from �����_COPY where ������������_������ like '%����������%'
-------------------------- t2 --------------------


--task8
drop table �����_COPY
select * into �����_COPY from �����

begin tran
insert �����_COPY values ('PRD-202402-005',	'���������� RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	begin tran
	update �����_COPY set ������������_������='����' where ������������_������ like '%����������%'
	commit

	print @@trancount
	if @@TRANCOUNT > 0 rollback;
	print @@trancount
	select count(*) from �����_COPY where ������������_������ like '%����������%'


	


