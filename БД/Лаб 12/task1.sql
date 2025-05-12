set nocount on
if  exists (select * from  SYS.OBJECTS        
	where OBJECT_ID= object_id(N'DBO.Y') )
drop table Y;           

declare @c int, @flag char = 'c';   

SET IMPLICIT_TRANSACTIONS  ON   

CREATE table Y(text nvarchar(50));                          
INSERT Y values ('ааа'),('ббб'),('ввв');
set @c = (select count(*) from Y);

print 'количество строк в таблице Y: ' + cast( @c as varchar(2));

if @flag = 'c'  commit;                
else   rollback;                                 
SET IMPLICIT_TRANSACTIONS  OFF   
	
if  exists (select * from  SYS.OBJECTS       
	where OBJECT_ID= object_id(N'DBO.Y') )
print 'таблица Y есть';  
else print 'таблицы Y нет'
