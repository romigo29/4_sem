begin try
	alter table SUBJECT_COPY
	drop column SUBJECT
end try
begin catch
print ERROR_NUMBER()
print ERROR_MESSAGE() 
print ERROR_LINE ()
print ERROR_PROCEDURE()
print ERROR_SEVERITy()
print ERROR_STATE ()
end catch


