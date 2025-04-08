select G.FACULTY, Case
when p.NOTE > 8 then 'Умняши'
when p.NOTE between 6 and 8 then 'Почти умняши'
when p.NOTE < 6 then 'Стремятся быть умняшами'
else '...'
end Результат,
count(*) [Количество]

from STUDENT s
join PROGRESS p on p.IDSTUDENT = s.IDSTUDENT
join GROUPS g on s.IDGROUP = g.IDGROUP
where g.FACULTY like 'ХТиТ'

group by G.FACULTY,
Case
when p.NOTE > 8 then 'Умняши'
when p.NOTE between 6 and 8 then 'Почти умняши'
when p.NOTE < 6 then 'Стремятся быть умняшами'
else '...'
end

