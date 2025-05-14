create trigger Univer_Tran on TEACHER after INSERT, DELETE, UPDATE
as 
	declare @c int = (select COUNT(TEACHER) from TEACHER)
	if (@c > 14)
	begin
		raiserror('�������� �� ����� ���� ������ 14', 10, 1)
		rollback
	end
	return

	
insert into TEACHER
values('����', '������� ����� ������������', '�', '����')

select * from TEACHER

drop trigger Univer_Tran