select G.FACULTY, Case
when p.NOTE > 8 then '������'
when p.NOTE between 6 and 8 then '����� ������'
when p.NOTE < 6 then '��������� ���� ��������'
else '...'
end ���������,
count(*) [����������]

from STUDENT s
join PROGRESS p on p.IDSTUDENT = s.IDSTUDENT
join GROUPS g on s.IDGROUP = g.IDGROUP
where g.FACULTY like '����'

group by G.FACULTY,
Case
when p.NOTE > 8 then '������'
when p.NOTE between 6 and 8 then '����� ������'
when p.NOTE < 6 then '��������� ���� ��������'
else '...'
end

