drop table SUBJECT_COPY
select * into SUBJECT_COPY from SUBJECT
select * from SUBJECT_COPY

declare @dsc char(10), @name nvarchar(100), @pulpit char(20)
declare dsc_cur cursor local
for select SUBJECT, SUBJECT_NAME, PULPIT from SUBJECT_COPY for update

open dsc_cur
select * from SUBJECT_COPY
fetch from dsc_cur into @dsc, @name, @pulpit
DELETE SUBJECT_COPY where current of dsc_cur
fetch from dsc_cur into @dsc, @name, @pulpit
UPDATE SUBJECT_COPY set SUBJECT_NAME = 'test' where current of dsc_cur
select * from SUBJECT_COPY

close dsc_cur
