use Univer

select p.SUBJECT, p.NOTE, count(*) [����������]
from PROGRESS p
group by p.SUBJECT, p.NOTE 
having p.NOTE in (8, 9)