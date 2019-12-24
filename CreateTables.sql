drop table Deserts
drop table Drinks
drop table Appetizers
drop table MainDishes
drop table Stats
drop table Orders
drop table Staff


create table Staff
(
	Id int primary key identity(1,1) not null,
	Name nvarchar(50),
	Surname nvarchar(50),
	Password nvarchar(50),
	Access int not null,
	Status nvarchar(50)
)

create table Stats
(
	Id int primary key not null identity(1,1) foreign key references Staff (Id),
	Name nvarchar(50),
	Surname nvarchar(50),
	Earnings float default 0,
	Orders int default 0 
)


create table Orders
(
	Id int primary key identity(1,1) not null,
	OrderName nvarchar(50),
	PersonId int not null foreign key references Staff (Id),
	Name nvarchar(50),
	Surname nvarchar(50),
	Total float default 0,
	Paid float,
	Change float,
	Date date,
	Time nvarchar(50)
)

create table Deserts
(
	Name nvarchar(50) primary key,
	Price float,
	Image varbinary(Max),
	Type nvarchar(50),
	Path nvarchar(200)
)
create table Drinks
(
	Name nvarchar(50) primary key,
	Price float,
	Image varbinary(Max),
	Type nvarchar(50),
	Path nvarchar(200)
)
create table Appetizers
(
	Name nvarchar(50) primary key,
	Price float,
	Image varbinary(Max),
	Type nvarchar(50),
	Path nvarchar(200)
)
create table MainDishes
(
	Name nvarchar(50) primary key,
	Price float,
	Image varbinary(Max),
	Type nvarchar(50),
	Path nvarchar(200)
)

--Vladimir Gitsarev
insert into Staff(Name, Surname, Access, Password, Status) values ('Vladimir', 'Gitsarev', 1, '262612821' , 'Working')
insert into Stats(Name, Surname) values ('Vladimir', 'Gitsarev')

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('VG1', 1, 'Vladimir', 'Gitsarev', 13.3, 14, 0.7, '25.05.2019', '13:42:18.1234567');
update Stats set Earnings += 13.3, Orders +=1 where Id = 1;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('VG2', 1, 'Vladimir', 'Gitsarev', 9.8, 10, 0.2, '26.05.2019', '18:13:17.1234567');
update Stats set Earnings += 9.8, Orders +=1 where Id = 1;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('VG3', 1, 'Vladimir', 'Gitsarev', 14.9, 15, 0.1, '27.05.2019', '13:42:18.1234567');
update Stats set Earnings += 14.9, Orders +=1 where Id = 1;

--Alexey Tsiabuk
insert into Staff(Name, Surname, Access, Password, Status) values ('Alexey', 'Tsiabuk', 0, '-1663141469' , 'Working')
insert into Stats(Name, Surname) values ('Alexey', 'Tsiabuk')

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('AT1', 2, 'Alexey', 'Tsiabuk', 17.2, 20, 2.8, '24.05.2019', '15:34:12.1234567');
update Stats set Earnings += 17.2, Orders +=1 where Id = 2;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('AT2', 2, 'Alexey', 'Tsiabuk', 9.7, 9.7, 0, '25.05.2019', '19:59:12.1234567');
update Stats set Earnings += 9.7, Orders +=1 where Id = 2;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('AT3', 2, 'Alexey', 'Tsiabuk', 14.9, 15, 0.1, '26.05.2019', '15:34:12.1234567');
update Stats set Earnings += 17.2, Orders +=1 where Id = 2;

--Leonid Matveenko
insert into Staff(Name, Surname, Access, Password, Status) values ('Leonid', 'Matveenko', 0, '-779763920' , 'Working')
insert into Stats(Name, Surname) values ('Leonid', 'Matveenko')

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('LM1', 3, 'Leonid', 'Matveenko', 16.7, 20, 3.4, '23.05.2019', '17:42:18.1234567');
update Stats set Earnings += 16.7, Orders +=1 where Id = 3;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('LM2', 3, 'Leonid', 'Matveenko', 6.7, 6.7, 0, '24.05.2019', '17:14:18.1234567');
update Stats set Earnings += 6.7, Orders +=1 where Id = 3;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('LM3', 3, 'Leonid', 'Matveenko', 9.5, 10, 0.5, '25.05.2019', '17:42:18.1234567');
update Stats set Earnings += 9.5, Orders +=1 where Id = 3;

--Daniil Marchuk
insert into Staff(Name, Surname, Access, Password, Status) values ('Daniil', 'Marchuk', 0, '-397719107' , 'Working')
insert into Stats(Name, Surname) values ('Daniil', 'Marchuk')

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('DM1', 4, 'Daniil', 'Marchuk', 11.5, 11.5, 0, '22.05.2019', '9:42:18.1234567');
update Stats set Earnings += 11.5, Orders +=1 where Id = 4;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('DM2', 4, 'Daniil', 'Marchuk', 17, 20, 3, '23.05.2019', '11:42:18.1234567');
update Stats set Earnings += 17, Orders +=1 where Id = 4;

insert into Orders(OrderName, PersonId, Name, Surname, Total, Paid, Change, Date, Time)
values ('DM3', 4, 'Daniil', 'Marchuk', 6.4, 6.4, 0, '24.05.2019', '9:42:18.1234567');
update Stats set Earnings += 6.4, Orders +=1 where Id = 4

select * from Stats
select * from Staff
select * from Orders