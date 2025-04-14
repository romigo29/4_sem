create table #tempTable3
(
	ID int identity(1,1),
	randnum real,
);

set nocount on
declare @i int = 0
while @i < 10000
begin
	insert #tempTable3
	values (floor(3000* rand()))
	set @i = @i + 1
end

checkpoint; --фиксация БД
DBCC DROPCLEANBUFFERS; --очистить буферный кэш

select * from #tempTable3
where randnum > 1000
order by randnum

create index #tempTable3_nonclu on #tempTable3(randnum) include (ID)
drop index #tempTable3_nonclu on #tempTable3
