drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT

--TASK4
-- A ---
------ грязное, неповторяющееся и фантомное чтения
	set transaction isolation level READ UNCOMMITTED 
	begin transaction 
    select count(*) from PULPIT_COPY where PULPIT = 'ОХ';
	--------------------t1----------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ';
	--------------------t2-------------------
	commit

--TASK5
-- A ---
	-----отсутствие грязного чтения; ------------
	-----неповторяющееся и фантомное чтения---------
	set transaction isolation level READ COMMITTED 
	begin transaction 
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	--------------t1--------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	--------------t2-----------------
	commit

--TASK6
-- A ---
	------отсутствие неповторяющегося чтения---------
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	--------------t1--------------
	commit
	--------------t2-----------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	-------фантомное чтение----------
	set transaction isolation level REPEATABLE READ 
	begin transaction 
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	--------------t1--------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	--------------t2-----------------
	commit


--TASK7
-- A ---
------отсутствие фантомного чтения
	set transaction isolation level SERIALIZABLE
	begin transaction 
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
	--------------t1--------------
	commit
	--------------t2-----------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'

