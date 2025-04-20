drop table SUBJECT_COPY
select * into SUBJECT_COPY from SUBJECT
select * from SUBJECT_COPY

declare @dsc char(10), @output char(150) = ' '

declare dsc_cur cursor local static
for select PULPIT from dbo.SUBJECT_COPY
open dsc_cur
print 'Количество строк: ' + cast(@@cursor_rows as varchar(5))
select * from SUBJECT_COPY
delete SUBJECT_COPY where PULPIT = 'ИСиТ'
fetch dsc_cur into @dsc
while @@FETCH_STATUS = 0
	begin
	set @output = trim(@dsc) + ', ' + @output 
	fetch dsc_cur into @dsc
	end

print @output
close dsc_cur
