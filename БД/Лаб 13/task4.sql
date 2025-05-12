create procedure PAUDITORIUM_INSERT
	@a char(20), @n varchar(50), @c int = 0, @t char(10)
	as declare @rc int = 1
	begin try
		insert into AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_CAPACITY, AUDITORIUM_TYPE)
		values (@a,  @n, @c, @t)
		return @rc;
	end try
	begin catch
		print 'номер ошибки: ' + cast(error_number() as varchar(6))
		print 'сообщение: ' + error_message()
		print 'номер строки: ' + cast(error_line() as varchar(6))
		if ERROR_PROCEDURE() is not null
		print 'имя процедуры: ' + error_procedure()
		return -1;
	end catch

declare @rc int;
exec @rc = PAUDITORIUM_INSERT @a='413-1',  @n='413-1', @c='15', @t='ЛК-К'
print 'код ошибки: ' + cast(@rc as varchar(3))

drop procedure PAUDITORIUM_INSERT

select * from AUDITORIUM

delete from AUDITORIUM where AUDITORIUM = '413-1'




	