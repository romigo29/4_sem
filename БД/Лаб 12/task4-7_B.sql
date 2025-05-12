drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT

--TASK4
-- B ---
    -- Грязное чтение
	begin transaction 
    delete from PULPIT_COPY where PULPIT = 'ОХ'; 

--TASK5
--- B --	
---отсутствие грязного чтения
	begin transaction
	delete from PULPIT_COPY where PULPIT = 'ОХ'
	-------------------------- t1 ------------------
	rollback
	--------------------------t2---------------
	----неповторяюшееся чтение------
	begin transaction 
    update PULPIT_COPY set PULPIT = 'Изменено' where PULPIT = 'ОХ';
	commit


--TASK6
--- B --	
	------------------------t2---------------------
	---- отсутствие неповторяющегося чтения------
	begin transaction 
    update PULPIT_COPY set PULPIT = 'Изменено' where PULPIT = 'ОХ';
	commit
	-------------t1----------------
	----Фантомное чтение
	begin transaction 
    insert PULPIT_COPY values ('ОХ', 'Органической химии', 'ТОВ')
	commit
	--------------t1-------------

--TASK7
--- B --	
	-----отсутствие фантомного чтения
	begin transaction 
	insert PULPIT_COPY values ('ОХ', 'Органической химии', 'ТОВ')
	commit
	-------------t1----------------



