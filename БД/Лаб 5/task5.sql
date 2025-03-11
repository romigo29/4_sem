use Univer

select FACULTY.FACULTY_NAME
from FACULTY
where not exists (select * from PULPIT
WHERE FACULTY.FACULTY = PULPIT.FACULTY)
