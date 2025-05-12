use Univer
go

create procedure PAUDITORIUM_INSERTX
    @a char(20), @n varchar(50), @c int = 0, @t char(10), @tn varchar(50)                            
as
begin try 
    set transaction isolation level SERIALIZABLE;          
    begin tran
    insert into AUDITORIUM_TYPE
	values (@t, @tn)
	exec PAUDITORIUM_INSERT @a, @n, @c, @t 
    commit tran;            
end try
begin catch 
    print '����� ������  : ' + cast(error_number() as varchar(6));
    print '���������     : ' + error_message();
    print '�������       : ' + cast(error_severity()  as varchar(6));
    print '�����         : ' + cast(error_state()   as varchar(8));
    print '����� ������  : ' + cast(error_line()  as varchar(8));
    if error_procedure() is not  null   
                     print '��� ��������� : ' + error_procedure();
     if @@trancount > 0 rollback tran ; 
     return -1;	  
end catch;

declare @rc int;
exec @rc = PAUDITORIUM_INSERTX @a='322-1',  @n='322-1', @c=15, @t='��', @tn = '��'

select * from AUDITORIUM
select * from AUDITORIUM_TYPE
drop procedure PAUDITORIUM_INSERTX

delete from AUDITORIUM where AUDITORIUM = '322-1'
delete from AUDITORIUM_TYPE where AUDITORIUM_TYPE = '��' 

