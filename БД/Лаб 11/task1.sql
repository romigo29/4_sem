declare @dsc char(10), @output char(100) = ' '
declare dsc_cur cursor 
for select SUBJECT from SUBJECT

open dsc_cur
fetch dsc_cur into @dsc
print 'Дисциплины'
while @@FETCH_STATUS = 0
	begin
	set @output = trim(@dsc) + ', ' + @output 
	fetch dsc_cur into @dsc
	end
print @output
close dsc_cur
deallocate dsc_cur
