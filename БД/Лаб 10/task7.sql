exec SP_HELPINDEX '����������' 
exec SP_HELPINDEX '�����' 
exec SP_HELPINDEX '�����' 
exec SP_HELPINDEX '������' 

select * into �����_copy from �����

checkpoint; --�������� ��
DBCC DROPCLEANBUFFERS; --�������� �������� ���
select * from �����_copy
where ���� >= 1500 and ���� <= 3000 
order by ����

create clustered index #�����_clustered on �����_copy(����, ������������_������)
drop index #�����_clustered on �����_copy

create index #�����_nonclu on �����_copy(����, ������������_������)
drop index #�����_nonclu on �����_copy

create index #�����_nonclu2 on �����_copy(����) include (������������_������)
drop index #�����_nonclu2 on �����_copy

create index #�����_nonclu3 on �����_copy(����) where (���� >= 1500 and ���� <= 3000)
drop index #�����_nonclu3 on �����_copy

create index #�����_nonclu4 on �����_copy(����) with (fillfactor=80)
drop index #�����_nonclu4 on �����_copy
