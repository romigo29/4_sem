drop table PULPIT_COPY
select * into PULPIT_COPY from PULPIT

begin tran
	insert PULPIT_COPY values ('��', '������', '�����')
	begin tran
	update PULPIT_COPY set PULPIT_NAME='������' where PULPIT_NAME = '������������ �����'
	commit

	print @@trancount
	if @@TRANCOUNT > 0 rollback;
	print @@trancount
	select * from PULPIT_COPY where PULPIT = '��'