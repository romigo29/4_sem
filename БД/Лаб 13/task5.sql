create procedure SUBJECT_REPORT @p varchar(20)
as 
begin try
	declare @rc int = 0, @sn varchar(20)='', @t varchar(200) = ''
	declare subj_cur cursor local static for
	(select SUBJECT_NAME from SUBJECT where SUBJECT.PULPIT = @p)

	if not exists (select SUBJECT_NAME from SUBJECT where SUBJECT.PULPIT = @p)
		raiserror('������', 11, 1)

	else 
		open subj_cur
		fetch subj_cur into @sn
		print '��������� ����������:'
		while @@FETCH_STATUS = 0
		begin
		set @t += rtrim(@sn) + ', ';  
        set @rc = @rc + 1;       
         fetch subj_cur into @sn; 
     end;   
	 print @t;        
	 close subj_cur;
     return @rc;
end try  
   begin catch              
        print '������ � ����������' 
        if error_procedure() is not null   
  print '��� ��������� : ' + error_procedure();
        return @rc;
   end catch; 

declare @count int;
exec @count = SUBJECT_REPORT @p = '����'
print '���������� ���������=' + cast(@count as varchar(3))

drop procedure SUBJECT_REPORT

select * from SUBJECT