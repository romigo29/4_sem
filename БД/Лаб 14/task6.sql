create function COUNT_PULPITS(@faculty varchar(20)) returns int
as begin 
	declare @rc int = (select count(PULPIT) from PULPIT where FACULTY = @faculty)
	return @rc
end
go

create function COUNT_GROUPS(@faculty varchar(20)) returns int
as begin 
	declare @rc int = (select count(PULPIT) from PULPIT where FACULTY = @faculty)
	return @rc
end
go

create function COUNT_PROFESSION(@faculty varchar(20)) returns int
as begin 
	declare @rc int = (select count(PULPIT) from PULPIT where FACULTY = @faculty)
	return @rc
end
go

alter function FACULTY_REPORT(@c int) returns @fr table
	                        ( [Факультет] varchar(50), [Количество кафедр] int, [Количество групп]  int, 
	                                                                 [Количество студентов] int, [Количество специальностей] int )
	as begin 
           declare cc CURSOR static for 
	       select FACULTY from FACULTY 
		   where dbo.COUNT_STUDENTS(FACULTY.FACULTY, default) > @c; 
	       declare @f varchar(30);
	       open cc;  
           fetch cc into @f;
	       while @@fetch_status = 0
	       begin
	            insert @fr values( @f,  dbo.COUNT_PULPITS(@f),
	            dbo.COUNT_GROUPS(@f),   dbo.COUNT_STUDENTS(@f, default),
	            dbo.COUNT_PROFESSION(@f)); 
	            fetch cc into @f;  
	       end;   
           return; 
	end;
go

select * from dbo.FACULTY_REPORT(0)

