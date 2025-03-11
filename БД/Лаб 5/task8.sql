use Univer

select NAME, BDAY
from STUDENT 
where BDAY > any (select BDAY from STUDENT
where NAME like '%Мария%')

SELECT * FROM STUDENT
where NAME like '%Мария%'