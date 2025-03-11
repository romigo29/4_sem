use Univer

select PULPIT.PULPIT_NAME	
from PULPIT, FACULTY
where PULPIT.FACULTY = FACULTY.FACULTY and
PULPIT.FACULTY IN (select FACULTY from PROFESSION
where PROFESSION_NAME like '%технология%' or
	PROFESSION_NAME like '%технологии%')