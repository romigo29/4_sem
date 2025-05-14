create function COUNT_STUDENTS(@faculty varchar(20)) returns int
as begin declare @rc int = 0
set @rc = (select count(*)
from FACULTY f
join GROUPS g on f.FACULTY = g.FACULTY
join STUDENT s on g.IDGROUP = s.IDGROUP
where f.FACULTY = @faculty)
return @rc
end;
go

declare @f int = dbo.COUNT_STUDENTS('ТОВ')
print 'Количество студентов: ' + cast(@f as varchar(4));
go

alter function COUNT_STUDENTS(@faculty varchar(20), @prof varchar(20) = null) returns int
as begin declare @rc int = 0
set @rc = (select count(*)
from FACULTY f
join GROUPS g on f.FACULTY = g.FACULTY
join STUDENT s on g.IDGROUP = s.IDGROUP
where f.FACULTY = isnull(@faculty, f.FACULTY) and g.PROFESSION = isnull(@prof, g.PROFESSION))
return @rc
end;
go;

declare @r int = dbo.COUNT_STUDENTS('ТОВ', '1-48 01 02')
print 'Количество студентов: ' + cast(@r as varchar)

select f.FACULTY ,dbo.COUNT_STUDENTS(f.FACULTY, '1-48 01 02') 
from FACULTY f

drop function dbo.COUNT_STUDENTS 
