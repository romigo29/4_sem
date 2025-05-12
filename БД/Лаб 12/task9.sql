--task2
drop table ТОВАР_COPY
select * into ТОВАР_COPY from ТОВАР
alter table ТОВАР_COPY
add constraint PK_ТОВАР_COPY primary key (Артикул);

begin try
	begin tran
	delete ТОВАР_COPY where Наименование_товара like '%Ноутбук%'
	insert ТОВАР_COPY values ('PRD-202402-005',	'Видеокарта RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	insert ТОВАР_COPY values ('PRD-202402-005',	'Видеокарта RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	commit tran
	print 'ok'
end try
	
begin catch
print 'ошибка: №' + cast(error_number() as varchar(5)) + ' ' + cast(error_message() as varchar(5))

if @@TRANCOUNT > 0 rollback tran;
end catch

select * from ТОВАР_COPY


--task3
drop table ТОВАР_COPY
select * into ТОВАР_COPY from ТОВАР
alter table ТОВАР_COPY
add constraint PK_ТОВАР_COPY primary key (Артикул);

declare @point varchar(32)

begin try
	begin tran
	delete ТОВАР_COPY where Наименование_товара like '%Ноутбук%'
	set @point = 'p1' save tran @point
	insert ТОВАР_COPY values ('PRD-202402-005',	'Видеокарта RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	set @point = 'p2' save tran @point
	insert ТОВАР_COPY values ('PRD-202402-005',	'Видеокарта RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	set @point = 'p3' save tran @point
	commit tran
	print 'ok'
end try
	
	
begin catch
print 'ошибка: №' + cast(error_number() as varchar(5)) + ' ' + cast(error_message() as varchar(5))

if @@TRANCOUNT > 0
	begin
		print 'контрольная точка: ' + @point 
		rollback tran @point;
		commit tran
	end;
end catch



--TASK4
drop table ТОВАР_COPY
select * into ТОВАР_COPY from ТОВАР
-- A ---
	set transaction isolation level READ UNCOMMITTED 
	begin transaction 
-------------------------- t1 ------------------
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
	commit; 
	-------------------------- t2 -----------------

--TASK5
-- A ---
	set transaction isolation level READ COMMITTED 
	begin transaction 
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
	-------------------------- t1 -----------------
	-------------------------- t2 -----------------
	select 'результат', count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
	commit

--TASK6
-- A ---
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
------------------------------ t1 -----------------
	commit; 
------------------------------ t2 -----------------
	insert ТОВАР_COPY values ('PRD-202402-005', 'Видеокарта RTX 4070 Ti', 2900, '6GB GDDR6X, DLSS 3')
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
----------------------------------------------------
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
	commit; 

--TASK7
-- A ---
	set transaction isolation level SERIALIZABLE 
	begin transaction
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
	-------------------------- t1 -----------------
	commit; 
	-------------------------- t2 -----------------


--TASK4 B-----------
drop table ТОВАР_COPY
select * into ТОВАР_COPY from ТОВАР

--TASK4
--- B --	

	begin transaction 
	delete ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
	-------------------------- t1 --------------------
	-------------------------- t2 --------------------
	rollback;

--TASK5
--- B --	
-------------------------- t1 ------------------
	begin transaction
	delete ТОВАР_COPY where Наименование_товара like '%Видеокарта%'

	-------------------------- t2 --------------------
	rollback
--TASK6
--- B --	
	begin transaction 
	delete ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
-------------------------- t1 ------------------
	commit
-------------------------- t2 (фантомное чтение) --------------------
	begin transaction 
	insert ТОВАР_COPY values ('PRD-202402-005', 'Видеокарта RTX 4070 Ti', 2900, '6GB GDDR6X, DLSS 3')
	commit
	-----------------------

--TASK7
--- B --	
	begin transaction 
	insert ТОВАР_COPY values ('PRD-202402-005', 'Видеокарта RTX 4070 Ti', 2900, '6GB GDDR6X, DLSS 3')
	commit
-------------------------- t1 ------------------
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'
-------------------------- t2 --------------------


--task8
drop table ТОВАР_COPY
select * into ТОВАР_COPY from ТОВАР

begin tran
insert ТОВАР_COPY values ('PRD-202402-005',	'Видеокарта RTX 4070 Ti', 2900,	'6GB GDDR6X, DLSS 3')
	begin tran
	update ТОВАР_COPY set Наименование_товара='Тест' where Наименование_товара like '%Видеокарта%'
	commit

	print @@trancount
	if @@TRANCOUNT > 0 rollback;
	print @@trancount
	select count(*) from ТОВАР_COPY where Наименование_товара like '%Видеокарта%'


	


