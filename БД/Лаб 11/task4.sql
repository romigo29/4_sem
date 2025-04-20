select * from SUBJECT

declare @dsc char(10), @name nvarchar(100), @pulpit char(20)
declare dsc_cur cursor local scroll
for select SUBJECT, SUBJECT_NAME, PULPIT from SUBJECT

open dsc_cur

fetch first from dsc_cur into @dsc, @name, @pulpit
print 'Первая строка: ' + @dsc + @name + '	' + @pulpit

fetch last from dsc_cur into @dsc, @name, @pulpit
print 'Последняя строка: ' + @dsc + @name + '	' + @pulpit

fetch absolute 10 from dsc_cur into @dsc, @name, @pulpit
print 'absolute 10: ' + @dsc + @name + '	' + @pulpit

fetch relative 5 from dsc_cur into @dsc, @name, @pulpit
print 'relative 5 : ' + @dsc + @name + '	' + @pulpit

fetch next from dsc_cur into @dsc, @name, @pulpit
print 'next : ' + @dsc + @name + '	' + @pulpit

fetch prior from dsc_cur into @dsc, @name, @pulpit
print 'prior : ' + @dsc + @name + '	' + @pulpit

close dsc_cur