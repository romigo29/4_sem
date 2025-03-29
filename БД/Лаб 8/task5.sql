select * into SUBJECT_COPY from SUBJECT

create view Дисциплина (Код, Наименование_дисциплины, Код_кафедры)
as select top(22) s.SUBJECT, s.SUBJECT_NAME, s.PULPIT
from SUBJECT_COPY s
order by s.SUBJECT_NAME

select * from Дисциплина

drop view Дисциплина
go
