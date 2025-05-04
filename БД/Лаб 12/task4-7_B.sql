drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT

--TASK4
--- B --	

	begin transaction 
	delete PULPIT_COPY where PULPIT = 'ОХ'
	-------------------------- t1 --------------------
	-------------------------- t2 --------------------
	rollback;

--TASK5
--- B --	
-------------------------- t1 ------------------
	begin transaction
	delete PULPIT_COPY where PULPIT = 'ОХ'

	-------------------------- t2 --------------------
	rollback
--TASK6
--- B --	
	begin transaction 
	delete PULPIT_COPY where PULPIT = 'ОХ'
-------------------------- t1 ------------------
	commit
-------------------------- t2 (фантомное чтение) --------------------
	begin transaction 
	insert PULPIT_COPY values ('ОХ', 'Органической химии', 'ТОВ')
	commit
	-----------------------

--TASK7
--- B --	
	begin transaction 
	insert PULPIT_COPY values ('ОХ', 'Органической химии', 'ТОВ')
	commit
-------------------------- t1 ------------------
	select count(*) from PULPIT_COPY where PULPIT = 'ОХ'
-------------------------- t2 --------------------

