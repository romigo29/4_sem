declare @totalCapacity int = (select sum(AUDITORIUM_CAPACITY) from AUDITORIUM),
@auditoriumAmount int,
@averageCapacity real,
@auditoriumAmountLessAverage real

if @totalCapacity > 200
begin
	select @auditoriumAmount = (select count(*) from AUDITORIUM),
	@averageCapacity = (select avg(AUDITORIUM_CAPACITY) from AUDITORIUM)

	set @auditoriumAmountLessAverage = (select count(*) from AUDITORIUM
		where AUDITORIUM_CAPACITY < @averageCapacity)

	select @auditoriumAmount '����������',
	@averageCapacity '������� �����������',
	@auditoriumAmountLessAverage '���-��, ��� ������� ����������� < �������',
	(@auditoriumAmountLessAverage / @auditoriumAmount) * 100 '�������'
end

else 
	print '����� ����������� - ' + convert(varchar, @totalCapacity)

