exec SP_HELPINDEX '����������' 
exec SP_HELPINDEX '�����' 
exec SP_HELPINDEX '�����' 
exec SP_HELPINDEX '������' 

checkpoint; --�������� ��
DBCC DROPCLEANBUFFERS; --�������� �������� ���

select * from �����
where ���� between 200 and 1000 order by ����
create index #�����_nonclu on �����(������������_������, ����)
drop index #�����_nonclu on �����

create index #�����_nonclu2 on �����(����) include (������������_������)
drop index #�����_nonclu2 on �����

create index #�����_nonclu3 on �����(����) where (���� >= 1500 and ���� <= 3000)
drop index #�����4_nonclu3 on �����

create index #�����_nonclu4 on �����(����) with (fillfactor=80)
drop index #�����_nonclu4 on �����


