--7-1
declare @dsc char(10), @output char(100) = ' '
declare dsc_cur cursor 
for select ������������_������ from �����

open dsc_cur
fetch dsc_cur into @dsc
print '������'
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
for select ������������_������ from �����
open dsc_cur
fetch dsc_cur into @dsc
print '������'
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
drop table �����_copy
select * into �����_copy from �����

declare @dsc char(10), @output char(150) = ' '

declare dsc_cur cursor local static
for select ������������_������ from �����_copy
open dsc_cur
print '���������� �����: ' + cast(@@cursor_rows as varchar(5))
select * from �����_copy
delete �����_copy where ������������_������ like '%����������%'
select * from �����_copy
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
select * from �����
declare @dsc char(50)
declare dsc_cur cursor local scroll
for select ������������_������ from �����

open dsc_cur

fetch first from dsc_cur into @dsc
print '������ ������: ' + @dsc

fetch last from dsc_cur into @dsc
print '��������� ������: ' + @dsc 

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
drop table �����_copy
select * into �����_copy from �����
select * from �����_copy

declare @dsc char(50)
declare dsc_cur cursor local
for select ������������_������ from �����_copy for update

open dsc_cur
select * from �����_copy
fetch from dsc_cur into @dsc
DELETE �����_copy where current of dsc_cur
fetch from dsc_cur into @dsc
UPDATE �����_copy set ������������_������ = 'test' where current of dsc_cur
select * from �����_copy

close dsc_cur
go


--7-6-1
drop table �����_copy
select * into �����_copy from �����
select * from �����_copy

declare @tname nvarchar(50), @price int
declare tovary cursor local
for select t.������������_������, t.���� from �����_copy t
where t.���� < 1000
for update

open tovary
fetch tovary into @tname, @price
while @@FETCH_STATUS = 0
begin
	delete �����_copy where current of tovary
	fetch tovary into @tname, @price
end
close tovary
go

--7-6-2
drop table �����_copy
select * into �����_copy from �����
select * from �����_copy

declare @tname nvarchar(50), @price int
declare tovary cursor local
for select t.������������_������, t.���� from �����_copy t
where t.���� < 1000
for update

open tovary
fetch tovary into @tname, @price
while @@FETCH_STATUS = 0
begin
	update �����_copy set ���� = ���� + 100 where current of tovary
	fetch tovary into @tname, @price
end
close tovary
go
