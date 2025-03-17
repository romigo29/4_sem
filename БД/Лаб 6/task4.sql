use Univer

select g.COURSE,
g.PROFESSION,
f.FACULTY,
round(avg(cast(p.NOTE as float(1))), 2)[Средний балл]

from GROUPS g
join FACULTY f on g.FACULTY = f.FACULTY
join STUDENT s on s.IDGROUP = g.IDGROUP
join PROGRESS p on s.IDSTUDENT = p.IDSTUDENT

group by 
g.COURSE,
g.PROFESSION,
f.FACULTY

order by [Средний балл] desc
