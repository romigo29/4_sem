create function FSUBJECTS(@p varchar(20)) returns varchar(300)
as begin
	declare @subj_name char(20);  
    declare @t varchar(300) = '';  
    declare subj_cur CURSOR LOCAL
    for select s.SUBJECT from SUBJECT s 
	where s.PULPIT = @p

    open subj_cur;	  
    fetch  subj_cur into @subj_name;   	 
    while @@fetch_status = 0    
	begin 
         set @t += rtrim(@subj_name) + ', ';         
         FETCH  subj_cur into @subj_name; 
     end;    
     return @t;
     end;  

	 select s.PULPIT, dbo.FSUBJECTS(s.PULPIT) as Дисциплины from SUBJECT s
	 group by s.PULPIT

