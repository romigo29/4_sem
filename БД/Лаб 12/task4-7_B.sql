drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT

--TASK4
-- B ---
    -- ������� ������
	begin transaction 
    delete from PULPIT_COPY where PULPIT = '��'; 

--TASK5
--- B --	
---���������� �������� ������
	begin transaction
	delete from PULPIT_COPY where PULPIT = '��'
	-------------------------- t1 ------------------
	rollback
	--------------------------t2---------------
	----��������������� ������------
	begin transaction 
    update PULPIT_COPY set PULPIT = '��������' where PULPIT = '��';
	commit


--TASK6
--- B --	
	------------------------t2---------------------
	---- ���������� ���������������� ������------
	begin transaction 
    update PULPIT_COPY set PULPIT = '��������' where PULPIT = '��';
	commit
	-------------t1----------------
	----��������� ������
	begin transaction 
    insert PULPIT_COPY values ('��', '������������ �����', '���')
	commit
	--------------t1-------------

--TASK7
--- B --	
	-----���������� ���������� ������
	begin transaction 
	insert PULPIT_COPY values ('��', '������������ �����', '���')
	commit
	-------------t1----------------



