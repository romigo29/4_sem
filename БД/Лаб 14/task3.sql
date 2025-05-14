create function FFACPUL(@faculty varchar(20), @pulpit varchar(20)) returns table
as return
select f.FACULTY, p.PULPIT from FACULTY f
left join PULPIT p on f.FACULTY = p.FACULTY
where f.FACULTY = isnull(@faculty, f.FACULTY)
and
p.PULPIT = ISNULL(@pulpit, p.PULPIT);

select * from dbo.FFACPUL(null, null)
select * from dbo.FFACPUL('À’‘', null)
select * from dbo.FFACPUL(null, 'À”')
select * from dbo.FFACPUL('»“', '»—Ë“')