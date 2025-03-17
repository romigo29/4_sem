use Univer;

select g.PROFESSION,
f.FACULTY,
p.SUBJECT,
round(avg(cast(p.NOTE as float(1))), 2)[—реднее]
from GROUPS g
join FACULTY f on g.FACULTY = f.FACULTY
join STUDENT s on s.IDGROUP = g.IDGROUP
join PROGRESS p on p.IDSTUDENT = s.IDSTUDENT

where f.FACULTY like '’“и“'

group by g.PROFESSION,
f.FACULTY,
p.SUBJECT



