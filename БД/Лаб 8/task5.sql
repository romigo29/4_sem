select * into SUBJECT_COPY from SUBJECT

create view ���������� (���, ������������_����������, ���_�������)
as select top(22) s.SUBJECT, s.SUBJECT_NAME, s.PULPIT
from SUBJECT_COPY s
order by s.SUBJECT_NAME

select * from ����������

drop view ����������
go
