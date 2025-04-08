declare @totalCapacity int = (select sum(AUDITORIUM.AUDITORIUM_CAPACITY) from AUDITORIUM)

if @totalCapacity > 500
print 'Большая вместительность'
else 
print 'Малая вместительность' 