CREATE TABLE ������(
�����_������ int PRIMARY KEY,
������������_������ nvarchar(20) foreign key references ������(������������),
����_������� real,
���������� int not null,
����_�������� date not null,
�������� nvarchar(20) foreign key references ���������(������������_�����))
