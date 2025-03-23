use Univer

select g.PROFESSION,
p.SUBJECT,
round(avg(cast(p.NOTE as float(1))), 2) [Среднее студенты]
from GROUPS g 
join STUDENT s on g.IDGROUP = s.IDGROUP
join PROGRESS p on s.IDSTUDENT = p.IDSTUDENT
where g.FACULTY like '%ТОВ%'
group by g.PROFESSION,
p.SUBJECT

intersect

select g.PROFESSION,
p.SUBJECT,
round(avg(cast(p.NOTE as float(1))), 2) [Среднее студенты]
from GROUPS g 
join STUDENT s on g.IDGROUP = s.IDGROUP
join PROGRESS p on s.IDSTUDENT = p.IDSTUDENT
where g.FACULTY like '%ХТиТ%'
group by g.PROFESSION,
p.SUBJECT
