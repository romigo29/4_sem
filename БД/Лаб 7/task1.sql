Use Univer;

select f.FACULTY,
g.PROFESSION,
p.SUBJECT,
round(avg(cast(p.NOTE as float(1))), 2) [Среднее экзамен]

from GROUPS g
join STUDENT s on g.IDGROUP = s.IDGROUP
join PROGRESS p  on s.IDSTUDENT = p.IDSTUDENT
join FACULTY f on g.FACULTY = f.FACULTY

group by rollup (f.FACULTY,
g.PROFESSION,
p.SUBJECT)

