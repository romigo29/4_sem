create trigger Univer_INSTEADOF on TEACHER INSTEAD OF DELETE
as 
	raiserror('�������� ���������', 10, 1)
	return


delete from TEACHER where TEACHER = '����'
select * from TEACHER

drop trigger Univer_INSTEADOF