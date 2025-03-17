use Univer;

select g.COURSE,
g.PROFESSION,
f.FACULTY,
p.SUBJECT,
round(avg(cast(p.NOTE as float(1))), 2)[Средний балл]

from GROUPS g
join FACULTY f on g.FACULTY = f.FACULTY
join STUDENT s on s.IDGROUP = g.IDGROUP
join PROGRESS p on s.IDSTUDENT = p.IDSTUDENT

where p.SUBJECT like 'ОАиП' 
or p.SUBJECT like '%БД%'

group by 
g.COURSE,
g.PROFESSION,
f.FACULTY,
p.SUBJECT

order by [Средний балл] desc
