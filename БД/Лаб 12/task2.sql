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

