print '����� ������������ ����� - ' + convert(varchar, @@ROWCOUNT)
print '������ SQL_������� - ' + convert(varchar, @@VERSION)
print '��������� ������������� �������� - ' + convert(varchar, @@SPID)
print '��� ��������� ������ - ' + convert(varchar, @@ERROR)
print '������� ����������� ���������� - ' + convert(varchar, @@TRANCOUNT)
print '��������� ���������� ������ ��������������� ������ - ' + convert(varchar, @@FETCH_STATUS)
print '������� ����������� ������� ��������� - ' + convert(varchar, @@NESTLEVEL)

