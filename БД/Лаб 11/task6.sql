select * from PROGRESS
INSERT INTO PROGRESS(SUBJECT, IDSTUDENT, PDATE, NOTE)
VALUES
('нюХо', 1006, '2013-01-15', 3),
('ад',   1007, '2013-02-17', 2),
('нюХо', 1008, '2013-03-18', 1),
('ад',   1009, '2013-04-19', 3),
('нюХо', 1010, '2013-05-20', 2),
('ад',   1011, '2013-06-21', 3),
('нюХо', 1012, '2013-07-22', 1),
('ад',   1013, '2013-08-23', 2);

delete PROGRESS where IDSTUDENT between 1006 and 1013


--6-1
declare @sname nvarchar(50), @subj nvarchar(50), @mark int
declare prog_stud cursor local
for select s.NAME, p.SUBJECT, p.NOTE from PROGRESS p
join STUDENT s on s.IDSTUDENT = p.IDSTUDENT
join GROUPS g on g.IDGROUP = s.IDGROUP
where p.NOTE < 4

open prog_stud
fetch prog_stud into @sname, @subj, @mark
while @@FETCH_STATUS = 0
begin
	delete PROGRESS where current of prog_stud
	fetch prog_stud into @sname, @subj, @mark
end
close prog_stud
go

--6-2

declare @sname nvarchar(50), @subj nvarchar(50), @mark int
declare prog_stud cursor local
for select s.NAME, p.SUBJECT, p.NOTE from PROGRESS p
join STUDENT s on s.IDSTUDENT = p.IDSTUDENT
join GROUPS g on g.IDGROUP = s.IDGROUP
where p.NOTE < 4

open prog_stud
fetch prog_stud into @sname, @subj, @mark
while @@FETCH_STATUS = 0
begin
	update PROGRESS set NOTE = NOTE + 1 where current of prog_stud
	fetch prog_stud into @sname, @subj, @mark
end
close prog_stud
go