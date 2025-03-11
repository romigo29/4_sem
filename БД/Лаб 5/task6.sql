use Univer

select top(1)
	(select avg(NOTE) from PROGRESS
		where PROGRESS.SUBJECT like '%нюХо%')[нюХо],
	(select avg(NOTE) from PROGRESS
		where PROGRESS.SUBJECT like '%ясад%')[ясад],
	(select avg(NOTE) from PROGRESS
		where PROGRESS.SUBJECT like '%йц%')[йц]

from PROGRESS 
