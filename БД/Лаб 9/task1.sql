declare @c char = 'c',
		@vc varchar = 'v',
		@dt datetime,
		@t time,
		@i int,
		@si smallint,
		@ti tinyint,
		@n numeric(12, 5)

set @dt = getdate()
set @t = sysdatetime() 

select @i = 2010210, 
@si = 32767,
@ti = 255,
@n = 4.7;

print 'Дата:	' + convert(varchar, @dt, 120) 
print 'Время:	' + convert(varchar, @t, 108)

select @c c, 
@vc vc, @i i, @ti ti, @n n