create database SFMProject
go

use SFMProject
go

create table products(
	id int primary key identity,
	name varchar(250) not null,
	price float,
	quantity int,
	Units varchar(250) -- don vi tinh
)
go

create table import(
	id int primary key identity,
	created_date date
)
go

create table import_detail(
	id_import_detail int primary key identity,
	id_import int,
	id_product int,
	quantity int,
	foreign key(id_import) references import(id),
	foreign key(id_product) references products(id)
)
go



create table agent(
	id int primary key identity,
	agent_name varchar(250) not null,
	phone int,
	address varchar(250)
)
go

create table order_product(
	id int primary key identity,
	agent_id int not null,
	order_at date not null,
	status_order bit not null, --1 la dang chuyen, 0 la dang xu li
	status_pay bit not null, --1 la thanh toan roi, 0 la chua thanh toan,
	method_pay varchar(50),
	foreign key(agent_id) references agent(id)
)
go

create table order_detail(
	id int primary key identity,
	order_id int not null,
	product_id int not null,
	quantity int not null,
	foreign key(order_id) references order_product(id),
	foreign key(product_id) references products(id)
)
go

create table selled(
	id int primary key identity,
	product_id int not null,
	quantity int not null,
	foreign key(product_id) references products(id)
)
go

 
insert into products(name,quantity,price,Units) values('Vitamin A',30,12000,'Vien'),
							('Vitamin B',30,11000,'Vien'),
							('Omega 3',30,8000,'Vien'),
							('Omega 6',30,3000,'Vien'),
							('prebiotic',30,6000,'Vien'),
							('probiotic',30,17000,'Vien'),
							('Whey protein',30,16000,'Vien'),
							('protein casein',30,14000,'Vien'),
							('BCAA',30,15000,'Vien'),
							('Creatine',30,25000,'Vien')
go							 

insert into import(created_date) values('2022-12-12'),('2022-11-21'),('2022-01-12'),('2022-02-12')
										,('2022-03-12'),('2022-04-12'),('2022-05-12'),('2022-07-12')
										,('2022-08-12'),('2022-10-12'),('2022-12-02')
go

insert into import_detail(id_import,id_product,quantity) values(1,2,9),(2,1,10),(3,4,5),(4,5,20),(5,6,17),
																(6,3,12),(7,8,21),(8,7,13),(9,10,24),(10,9,14),
																(11,1,14)
go

insert into agent(agent_name, phone,address ) values('Nha Thuoc Long Chau','113', 'SGN'),
													('Nha Thuoc An Khang','115', 'SGN'),
													('Pharmacity','114','HN')
go



insert into order_product(agent_id,order_at,status_order,status_pay,method_pay) 
	values(1,'2021-05-3',0,1,'Momo'),(1,'2021-06-3',0,1,'Zalo Pay'),(1,'2021-07-4',1,0,'Cash'),(2,'2021-08-3',0,0,'Zalo Pay'),
			(2,'2021-11-3',1,1,'Cash'),(3,'2022-02-3',1,0,'Zalo Pay'),
			(3,'2022-04-3',0,0,'Momo'),(3,'2022-10-3',1,1,'Zalo Pay'),(3,'2022-12-13',0,1,'Cash')
go

insert into order_detail(order_id ,product_id,quantity) 
	values (1,2,2),(1,1,4),(1,4,5),(1,5,7),(1,7,3),
			(2,1,5),(2,3,2),(2,5,1),(2,7,3),(2,9,7),
			(3,5,3),(3,2,1),(3,3,5),(3,7,7),(3,7,4),(3,9,2),
			(4,1,8),(4,3,7),(4,5,6),(4,7,3),(4,9,2),
			(5,9,3),(2,1,7),(2,5,8),(2,7,9),(2,10,8),
			(6,4,3),(6,1,7),(6,2,8),(6,8,9),(6,9,10),
			(7,1,5),(7,4,3),(7,6,1),(7,3,3),(7,9,7),
			(8,1,2),(8,3,1),(8,5,7),(8,7,4),(8,9,8),
			(9,8,4),(9,3,2),(9,1,3),(9,10,4),(9,4,3)							
go

insert into selled(product_id,quantity) values(9,3),(1,7),(5,8),(7,9),(10,8),
												(1,2),(3,1),(5,7),(7,4),(9,8)	
go





select * from products
select * from import
select * from import_detail
select * from agent
select * from order_product
select * from order_detail
select * from selled





	



