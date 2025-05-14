create trigger Univer_INSTEADOF on TEACHER INSTEAD OF DELETE
as 
	raiserror('Удаление запрещено', 10, 1)
	return


delete from TEACHER where TEACHER = 'СМЛВ'
select * from TEACHER

drop trigger Univer_INSTEADOF