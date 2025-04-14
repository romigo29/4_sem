exec SP_HELPINDEX 'ÏÎÊÓÏÀÒÅËÜ' 
exec SP_HELPINDEX 'ÒÎÂÀĞ' 
exec SP_HELPINDEX 'ÑÊËÀÄ' 
exec SP_HELPINDEX 'ÓÑËÓÃÀ' 

checkpoint; --ôèêñàöèÿ ÁÄ
DBCC DROPCLEANBUFFERS; --î÷èñòèòü áóôåğíûé êıø

select * from ÒÎÂÀĞ
where Öåíà between 200 and 1000 order by Öåíà
create index #ÒÎÂÀĞ_nonclu on ÒÎÂÀĞ(Íàèìåíîâàíèå_òîâàğà, öåíà)
drop index #ÒÎÂÀĞ_nonclu on ÒÎÂÀĞ

create index #ÒÎÂÀĞ_nonclu2 on ÒÎÂÀĞ(öåíà) include (Íàèìåíîâàíèå_òîâàğà)
drop index #ÒÎÂÀĞ_nonclu2 on ÒÎÂÀĞ

create index #ÒÎÂÀĞ_nonclu3 on ÒÎÂÀĞ(öåíà) where (öåíà >= 1500 and öåíà <= 3000)
drop index #ÒÎÂÀĞ4_nonclu3 on ÒÎÂÀĞ

create index #ÒÎÂÀĞ_nonclu4 on ÒÎÂÀĞ(öåíà) with (fillfactor=80)
drop index #ÒÎÂÀĞ_nonclu4 on ÒÎÂÀĞ


