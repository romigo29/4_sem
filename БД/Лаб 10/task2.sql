create table #tempTable2
(
	ID int identity(1,1),
	randnum real,
);

set nocount on
declare @i int = 0
while @i < 10000
begin
	insert #tempTable2
	values (floor(3000* rand()))
	set @i = @i + 1
end

checkpoint; --фиксация БД
DBCC DROPCLEANBUFFERS; --очистить буферный кэш

select * from #tempTable2
where randnum = 1000 and ID < 3000
order by randnum

create index #tempTable2_nonclu on #tempTable2(ID, randnum)

drop index #tempTable2_nonclu on #tempTable2
drop table #tempTable2