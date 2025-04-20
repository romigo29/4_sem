--7-1
declare @dsc char(10), @output char(100) = ' '
declare dsc_cur cursor 
for select Наименование_товара from ТОВАР

open dsc_cur
fetch dsc_cur into @dsc
print 'Товары'
while @@FETCH_STATUS = 0
	begin
	set @output = trim(@dsc) + ', ' + @output 
	fetch dsc_cur into @dsc
	end
print @output
close dsc_cur
deallocate dsc_cur
go

--7-2
declare @dsc char(10), @output char(100) = ' '
declare dsc_cur cursor local
for select Наименование_товара from ТОВАР
open dsc_cur
fetch dsc_cur into @dsc
print 'Товары'
while @@FETCH_STATUS = 0
	begin
	set @output = trim(@dsc) + ', ' + @output 
	fetch dsc_cur into @dsc
	end
print @output
go

declare @dsc char(10)
fetch dsc_cur into @dsc
print @dsc
go

--7-3
drop table ТОВАР_copy
select * into ТОВАР_copy from ТОВАР

declare @dsc char(10), @output char(150) = ' '

declare dsc_cur cursor local static
for select Наименование_товара from ТОВАР_copy
open dsc_cur
print 'Количество строк: ' + cast(@@cursor_rows as varchar(5))
select * from ТОВАР_copy
delete ТОВАР_copy where Наименование_товара like '%Видеокарта%'
select * from ТОВАР_copy
fetch dsc_cur into @dsc
while @@FETCH_STATUS = 0
	begin
	set @output = trim(@dsc) + ', ' + @output 
	fetch dsc_cur into @dsc
	end

print @output
close dsc_cur
go

--7-4
select * from ТОВАР
declare @dsc char(50)
declare dsc_cur cursor local scroll
for select Наименование_товара from ТОВАР

open dsc_cur

fetch first from dsc_cur into @dsc
print 'Первая строка: ' + @dsc

fetch last from dsc_cur into @dsc
print 'Последняя строка: ' + @dsc 

fetch absolute 3 from dsc_cur into @dsc
print 'absolute 3: ' + @dsc 

fetch relative 3 from dsc_cur into @dsc
print 'relative 3 : ' + @dsc 

fetch next from dsc_cur into @dsc
print 'next : ' + @dsc 

fetch prior from dsc_cur into @dsc
print 'prior : ' + @dsc

close dsc_cur
go

--7-5
drop table ТОВАР_copy
select * into ТОВАР_copy from ТОВАР
select * from ТОВАР_copy

declare @dsc char(50)
declare dsc_cur cursor local
for select Наименование_товара from ТОВАР_copy for update

open dsc_cur
select * from ТОВАР_copy
fetch from dsc_cur into @dsc
DELETE ТОВАР_copy where current of dsc_cur
fetch from dsc_cur into @dsc
UPDATE ТОВАР_copy set Наименование_товара = 'test' where current of dsc_cur
select * from ТОВАР_copy

close dsc_cur
go


--7-6-1
drop table ТОВАР_copy
select * into ТОВАР_copy from ТОВАР
select * from ТОВАР_copy

declare @tname nvarchar(50), @price int
declare tovary cursor local
for select t.Наименование_товара, t.Цена from ТОВАР_copy t
where t.Цена < 1000
for update

open tovary
fetch tovary into @tname, @price
while @@FETCH_STATUS = 0
begin
	delete ТОВАР_copy where current of tovary
	fetch tovary into @tname, @price
end
close tovary
go

--7-6-2
drop table ТОВАР_copy
select * into ТОВАР_copy from ТОВАР
select * from ТОВАР_copy

declare @tname nvarchar(50), @price int
declare tovary cursor local
for select t.Наименование_товара, t.Цена from ТОВАР_copy t
where t.Цена < 1000
for update

open tovary
fetch tovary into @tname, @price
while @@FETCH_STATUS = 0
begin
	update ТОВАР_copy set Цена = Цена + 100 where current of tovary
	fetch tovary into @tname, @price
end
close tovary
go
