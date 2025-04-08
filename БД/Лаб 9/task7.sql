create table #tempTable 
(randint int,
duplicates nvarchar(100),
currentDate datetime
);

declare @i int = 0
while @i < 10
	begin
	insert #tempTable(randint, duplicates, currentDate)
	values (
	floor(30000 * RAND()),
	replicate('Êמפו ןמלמדאוע מע ְכüצדוילונא', 3),
	getdate()
	)
	set @i = @i + 1
	end


select * from #tempTable

drop table #tempTable