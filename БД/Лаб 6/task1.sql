use Univer

select AUDITORIUM_TYPE.AUDITORIUM_TYPE,
max(AUDITORIUM_CAPACITY) [Макс. вместимость],
min(AUDITORIUM_CAPACITY) [Мин. вместимость],
avg(AUDITORIUM_CAPACITY) [Средняя вместимость],
sum(AUDITORIUM_CAPACITY) [Суммарная вместимость],
count(*) [Кол-во аудиторий]
from AUDITORIUM join AUDITORIUM_TYPE
on AUDITORIUM.AUDITORIUM_TYPE = AUDITORIUM_TYPE.AUDITORIUM_TYPE
group by AUDITORIUM_TYPE.AUDITORIUM_TYPE
