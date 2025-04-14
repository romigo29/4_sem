create table #tempTable5
(
	ID int identity(1,1),
	randnum real,
);

set nocount on
declare @i int = 0
while @i < 10000
begin
	insert #tempTable5
	values (floor(3000* rand()))
	set @i = @i + 1
end

checkpoint; --фиксация БД
DBCC DROPCLEANBUFFERS; --очистить буферный кэш

select randnum from #tempTable5
where randnum >= 1500 and randnum <= 3000
order by randnum

create index #tempTable5_randnum on #tempTable5(randnum)
drop index #tempTable5_randnum on #tempTable5
drop table #tempTable5

SELECT 
    i.name AS [Индекс],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)]
FROM sys.dm_db_index_physical_stats (
    DB_ID('tempdb'), 
    OBJECT_ID('tempdb..#tempTable5'), 
    NULL, 
    NULL, 
    NULL
) AS ips
JOIN tempdb.sys.indexes AS i 
    ON ips.object_id = i.object_id 
    AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;

INSERT top(10000) #tempTable5(randnum) select randnum from #tempTable5;

--реорганизация
alter index #tempTable5_randnum on #tempTable5 reorganize

--перестройка
alter index #tempTable5_randnum on #tempTable5 rebuild with (online = off);


--task6
drop index #tempTable5_randnum on #tempTable5
create index #tempTable5_randnum on #tempTable5(randnum) with (fillfactor=65)

INSERT top(50)percent INTO #tempTable5(randnum)
SELECT randnum from #tempTable5

SELECT 
    i.name AS [Индекс],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)]
FROM sys.dm_db_index_physical_stats (
    DB_ID('tempdb'), 
    OBJECT_ID('tempdb..#tempTable5'), 
    NULL, 
    NULL, 
    NULL
) AS ips
JOIN tempdb.sys.indexes AS i 
    ON ips.object_id = i.object_id 
    AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;

