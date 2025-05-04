drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT

--TASK4
-- A ---
	set transaction isolation level READ UNCOMMITTED 
	begin transaction 
-------------------------- t1 ------------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ';
	commit; 
	-------------------------- t2 -----------------

--TASK5
-- A ---
	set transaction isolation level READ COMMITTED 
	begin transaction 
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	-------------------------- t1 -----------------
	-------------------------- t2 -----------------
	select 'ОХ'  'результат', count(*) from PULPIT_COPY where PULPIT = 'ОХ';
	commit

--TASK6
-- A ---
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
------------------------------ t1 -----------------
	commit; 
------------------------------ t2 -----------------
	insert PULPIT_COPY values ('ОХ', 'Органической химии', 'ТОВ')
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
----------------------------------------------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	commit; 

--TASK7
-- A ---
	set transaction isolation level SERIALIZABLE 
	begin transaction
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	-------------------------- t1 -----------------
	commit; 
	-------------------------- t2 -----------------
	
