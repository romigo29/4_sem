exec SP_HELPINDEX 'ПОКУПАТЕЛЬ' 
exec SP_HELPINDEX 'ТОВАР' 
exec SP_HELPINDEX 'СКЛАД' 
exec SP_HELPINDEX 'УСЛУГА' 

select * into ТОВАР_copy from ТОВАР

checkpoint; --фиксация БД
DBCC DROPCLEANBUFFERS; --очистить буферный кэш
select * from ТОВАР_copy
where цена >= 1500 and цена <= 3000 
order by Цена

create clustered index #ТОВАР_clustered on Товар_copy(Цена, наименование_товара)
drop index #ТОВАР_clustered on Товар_copy

create index #ТОВАР_nonclu on ТОВАР_copy(цена, Наименование_товара)
drop index #ТОВАР_nonclu on ТОВАР_copy

create index #ТОВАР_nonclu2 on ТОВАР_copy(цена) include (Наименование_товара)
drop index #ТОВАР_nonclu2 on ТОВАР_copy

create index #ТОВАР_nonclu3 on ТОВАР_copy(цена) where (цена >= 1500 and цена <= 3000)
drop index #ТОВАР_nonclu3 on ТОВАР_copy

create index #ТОВАР_nonclu4 on ТОВАР_copy(цена) with (fillfactor=80)
drop index #ТОВАР_nonclu4 on ТОВАР_copy
