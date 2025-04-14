create table #tempTable4
(
	ID int identity(1,1),
	randnum real,
);

set nocount on
declare @i int = 0
while @i < 10000
begin
	insert #tempTable4
	values (floor(3000* rand()))
	set @i = @i + 1
end

checkpoint; --фиксация БД
DBCC DROPCLEANBUFFERS; --очистить буферный кэш

select randnum from #tempTable4
where randnum >= 1500 and randnum <= 3000
order by randnum

create index #tempTable4_nonclu on #tempTable4(randnum) where (randnum >= 1500 and randnum <= 3000)
drop index #tempTable4_nonclu on #tempTable4