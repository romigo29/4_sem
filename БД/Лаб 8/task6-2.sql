create view ����������_�������
as select ������������_������[�����],
count(*) [����������]
from �����
join ����� on �����.�������_������ = �����.�������
group by �����.������������_������
go

select * from ����������_�������

alter view ����������_������� with SCHEMABINDING
as select ������������_������[�����],
count(*) [����������]
from dbo.�����
join dbo.����� on dbo.�����.�������_������ = dbo.�����.�������
group by dbo.�����.������������_������
go