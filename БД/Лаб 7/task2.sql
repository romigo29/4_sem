Use Univer;

select g.PROFESSION,
f.FACULTY,
p.SUBJECT,
round(avg(cast(p.NOTE as float(1))), 2) [Среднее экзамен]

from GROUPS g
join STUDENT s on g.IDGROUP = s.IDGROUP
join PROGRESS p  on s.IDSTUDENT = p.IDSTUDENT
join FACULTY f on g.FACULTY = f.FACULTY

group by cube (g.PROFESSION,
f.FACULTY,
p.SUBJECT)

