use Univer

select top(1)
	(select avg(NOTE) from PROGRESS
		where PROGRESS.SUBJECT like '%����%')[����],
	(select avg(NOTE) from PROGRESS
		where PROGRESS.SUBJECT like '%����%')[����],
	(select avg(NOTE) from PROGRESS
		where PROGRESS.SUBJECT like '%��%')[��]

from PROGRESS 
