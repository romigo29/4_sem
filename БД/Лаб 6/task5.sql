use Univer;

select g.COURSE,
g.PROFESSION,
f.FACULTY,
p.SUBJECT,
round(avg(cast(p.NOTE as float(1))), 2)[������� ����]

from GROUPS g
join FACULTY f on g.FACULTY = f.FACULTY
join STUDENT s on s.IDGROUP = g.IDGROUP
join PROGRESS p on s.IDSTUDENT = p.IDSTUDENT

where p.SUBJECT like '����' 
or p.SUBJECT like '%��%'

group by 
g.COURSE,
g.PROFESSION,
f.FACULTY,
p.SUBJECT

order by [������� ����] desc
