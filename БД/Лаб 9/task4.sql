--4-1
declare @z real, @t real, @x real

select @t = 3,
@x = 3


if (@t > @x) set @z = POWER(SIN(@t), 2)
else if (@t < @x) set @z = 4 * (@t + @x)
else set @z = 1 - POWER(EXP(1), @x - 2)

select @z z

--4-2
declare @fullName char(100) = 'Романов Игорь Вячеславович',
@shortName char(4)

declare @indexToCut int = charindex(' ', @fullName);
declare @newName char(100) = ltrim(substring(@fullName, @indexToCut, len(@fullname)))

set @shortName = substring(@newName, 1, 1) + '.'
+ substring(@newName, CHARINDEX(' ', @newName) + 1, 1) + '.'

print 'Полное имя - ' + @fullName
print 'Короткое имя - ' + @shortName


--4-3
declare @currentDate datetime = getdate()

select STUDENT.NAME, STUDENT.BDAY, 
datediff(yy, STUDENT.BDAY, @currentDate) [Возраст]
from STUDENT
where MONTH(STUDENT.BDAY) = MONTH(dateadd(m, 1, @currentDate))


--4-4
select STUDENT.NAME,
PROGRESS.PDATE,
DATEPART(DW, PROGRESS.PDATE) [День недели]
from STUDENT join PROGRESS
on STUDENT.IDSTUDENT = PROGRESS.IDSTUDENT
where PROGRESS.SUBJECT  like '%БД%'