create database QuanLyQuanCafe
go
use QuanLyQuanCafe
go
-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfo
create table TableFood
(
	id int identity primary key,
	name nvarchar(100) default N'Chua dat ten',
	status nvarchar(100) default N'Trong' --Trong || Co nguoi
)
go
create table Account
(
	UserName nvarchar(100) primary key,
	DisplayName nvarchar(100) not null default N'qt',
	PassWord nvarchar(1000) not null default 0,
	type int not null -- 1: admin || 0: staff
)
go
create table FoodCategory
(
	id int identity primary key,
	name nvarchar(100) not null default N'Chua dat ten'
)
go
create table Food
(
	id int identity primary key,
	name nvarchar(100) not null default N'Chua dat ten',
	idCategory int not null,
	price float not null default 0

	foreign key(idCategory) references FoodCategory(id)
)
go

create table Bill
(
	id int identity primary key,
	DateCheckIn date not null,
	DataCheckOut date not null,
	idTable int not null,
	status int not null default 0-- 1: Da thanh toan && 0: Chua thanh toan

	foreign key(idTable) references TableFood(id)
)
go
create table BillInfo
(
	id int identity primary key,
	idBill int not null,
	idFood int  not null,
	count int  not null default 0

	foreign key(idBill) references Bill(id),
	foreign key(idFood) references Food(id)
)
go

insert into Account
(
	UserName,
	DisplayName,
	PassWord,
	type
) values 
(
	'Admin',
	'Trinh',
	'1',
	1
)
go
insert into Account
(
	UserName,
	DisplayName,
	PassWord,
	type
) values 
(
	'Staff',
	'Gus',
	'1',
	0
)
go
create proc USP_GetAccountByUserName
@username nvarchar(100)
as
begin
	select * from Account where UserName = @username
end
go

exec USP_GetAccountByUserName @username = 'admin'
go
create proc USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
as
begin
	select * from Account where UserName = @userName and PassWord = @passWord
end
go

exec USP_Login @userName = 'staff', @passWord = '1'

go
-- thêm 10 bàn vào csdl
declare @i int = 0
while @i <= 10
begin
	insert TableFood (name) values(N'Bàn ' + cast(@i as nvarchar(100)))
	set @i = @i +1
end
go
create proc USP_GetListTable
as
begin
	select * from TableFood
end
go
exec USP_GetListTable
go
update TableFood set status = N'Có người' where id = 21
go
select * from FoodCategory
go
insert into FoodCategory(name) values(N'CAFE')
insert into FoodCategory(name) values(N'SINH TỐ')
insert into FoodCategory(name) values(N'NƯỚC TRÁI CÂY')
insert into FoodCategory(name) values(N'TRÀ')
insert into FoodCategory(name) values(N'BÁNH NGỌT')
go
select * from Food
go
insert into Food (name, idCategory, price) values(N'Cafe đen nóng', 1, 15000)
insert into Food (name, idCategory, price) values(N'Cafe đen đá', 1, 17000)
insert into Food (name, idCategory, price) values(N'Cafe sữa nóng', 1, 16000)
insert into Food (name, idCategory, price) values(N'Cafe sữa', 1, 25000)
insert into Food (name, idCategory, price) values(N'Cafe xay Chocolate', 1, 255000)
go
update Food
set price = 25000
where price = 255000
go

insert into Food (name, idCategory, price) values(N'Sinh tố bơ', 2, 30000)
insert into Food (name, idCategory, price) values(N'Sinh tố xoài', 2, 35000)
insert into Food (name, idCategory, price) values(N'Sinh tố dâu', 2, 32000)
insert into Food (name, idCategory, price) values(N'Sinh đu đủ', 2, 25000)
insert into Food (name, idCategory, price) values(N'Sinh dưa gang', 2, 20000)
go
insert into Food (name, idCategory, price) values(N'Thơm', 3, 20000)
insert into Food (name, idCategory, price) values(N'Cam', 3, 15000)
insert into Food (name, idCategory, price) values(N'Carot - Cam', 3, 30000)
insert into Food (name, idCategory, price) values(N'Dưa hấu', 3, 20000)
insert into Food (name, idCategory, price) values(N'Dưa lê', 3, 25000)
insert into Food (name, idCategory, price) values(N'Táo', 3, 15000)
go

insert into Food (name, idCategory, price) values(N'Trà gừng nóng', 4, 15000)
insert into Food (name, idCategory, price) values(N'Trà chanh gừng đá', 4, 18000)
insert into Food (name, idCategory, price) values(N'Trà xoài', 4, 15000)
insert into Food (name, idCategory, price) values(N'Trà Lipton đá', 4, 25000)
insert into Food (name, idCategory, price) values(N'Trà Lipton nóng', 4, 25000)
go
insert into Food (name, idCategory, price) values(N'Bánh Opera', 5, 25000)
insert into Food (name, idCategory, price) values(N'Bánh chuối', 5, 25000)
insert into Food (name, idCategory, price) values(N'Bánh trà xanh', 5, 25000)
insert into Food (name, idCategory, price) values(N'Bánh kem bơ', 5, 25000)
insert into Food (name, idCategory, price) values(N'Bánh ngọt', 5, 25000)
go
select * from Bill
go
alter table Bill
alter column DateCheckIn date
go
alter table Bill
alter column DataCheckOut date
go
SP_RENAME 'Bill.DataCheckOut', 'DateCheckOut', 'COLUMN'
go
go
insert into Bill (DateCheckIn, DateCheckOut, idTable, status) values(GETDATE(), null, 12, 0)
insert into Bill (DateCheckIn, DateCheckOut, idTable, status) values(GETDATE(), null, 15, 0)
insert into Bill (DateCheckIn, DateCheckOut, idTable, status) values(GETDATE(), GETDATE(), 12, 1)
insert into Bill (DateCheckIn, DateCheckOut, idTable, status) values(GETDATE(), null, 18, 0)
go
insert into BillInfo(idBill, idFood, count) values(3, 6, 2)
insert into BillInfo(idBill, idFood, count) values(2, 4, 3)
insert into BillInfo(idBill, idFood, count) values(2, 11, 6)
insert into BillInfo(idBill, idFood, count) values(4, 22, 1)
insert into BillInfo(idBill, idFood, count) values(5, 20, 3)
go
select * from BillInfo
go 

select f.name, bi.count, f.price, f.price * bi.count as TotalPricer from BillInfo as bi,Bill as b, Food as f
where bi.idBill = b.id and bi.idFood = f.id and b.idTable = 21
go

select * from BillInfo
select * from Bill
select * from TableFood
select * from FoodCategory
select * from Food


go
-- thêm món ăn vào bill từ bàn được chọn
create proc USP_InsertBill
@idTable int
as
begin
	insert Bill(DateCheckIn, DateCheckOut, idTable, status, discount) values(GETDATE(), null, @idTable, 0, 0)
end
go

-- thêm vào thông tin hóa đơn
alter proc USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExitsBillInfo int
	declare @foodCount int = 1
	select @isExitsBillInfo = id, @foodCount = count from BillInfo where idBill = @idBill and idFood = @idFood

	if(@isExitsBillInfo > 0)
	begin
		declare @newCount int = @foodCount + @count
		if(@newCount > 0)
			update BillInfo set count = @foodCount + @count where idFood = @idFood
		else
			delete BillInfo where idBill = @idBill and idFood = @idFood
	end
	else
	begin
		insert BillInfo(idBill, idFood, count)values(@idBill, @idFood, @count)
	end
end
go

delete BillInfo
delete Bill
delete TableFood
-- tao trigger update billinfo
go
alter trigger UTG_UpdateBillInfo
on BillInfo for insert, update
as
begin
	declare @idBill int
	select @idBill = idBill from inserted
	declare @idTable int
	select @idTable = idTable from Bill where id = @idBill and status = 0

	declare @count int
	select @count = count(*) from BillInfo where idBill = @idBill
	if(@count > 0)
		update TableFood set status = N'Có người' where id = @idTable
	else
		update TableFood set status = N'Trống' where id = @idTable
end
go
create trigger UTG_UpdateBill
on Bill for update
as
begin
	declare @idBill int
	select @idBill = id from inserted
	declare @idTable int
	select @idTable = idTable from Bill where id = @idBill
	declare @count int = 0
	select @count = count(*) from Bill where idTable = @idTable and status = 0
	if(@count = 0)
		update TableFood set status = N'Trống' where id = @idTable
end
go
-- sửa bảng Bill thêm phần giảm giá 
alter table Bill
add discount int 
select * from Bill
update Bill set discount = 0

go
-- không cần tới
select * from Bill
declare @idBillNew int = 17
select id into IDBillInfoTable from BillInfo where idBill = @idBillNew
declare @idBillOld int = 17
update BillInfo set idBill = @idBillOld where id in(select * from IDBillInfoTable)
--end
go 
alter proc USP_SwitchTable
@idTable1 int, @idTable2 int
as
begin
	declare @idFirstBill int
	declare @idSecondBill int

	declare @isFirstTableEmpty int = 1
	declare @isSecondTableEmpty int = 1

	select @idSecondBill = id from Bill where idTable = @idTable2 and status = 0
	select @idFirstBill = id from Bill where idtable = @idTable1 and status = 0

	if(@idFirstBill is null)
	begin
		insert Bill(DateCheckIn, DateCheckOut, idTable, status) values(GETDATE(), null, @idTable1, 0)
		select @idFirstBill = max(id) from Bill where idTable = @idTable1 and status = 0
		-- set @isFirstTableEmpty = 1
	end
	select @isFirstTableEmpty = count(*) from BillInfo where idBill = @idFirstBill
	if(@idSecondBill is null)
	begin
		insert Bill(DateCheckIn, DateCheckOut, idTable, status) values (GETDATE(), null, @idTable2, 0)
		select @idSecondBill = max(id) from Bill where idTable = @idTable2 and status = 0
		-- set @isSecondTableEmpty = 1
	end
	select @isSecondTableEmpty = count(*) from BillInfo where idBill = @idSecondBill
	select id into IDBillInfoTable from BillInfo where idBill = @idSecondBill
	update BillInfo set idBill = @idSecondBill where idBill = @idFirstBill
	update BillInfo set idBill = @idFirstBill where id in(select * from IDBillInfoTable)
	drop table IDBillInfoTable
	if(@isFirstTableEmpty = 0)
		update TableFood set status = N'Trống' where id = @idTable2
	if(@isSecondTableEmpty = 0)
		update TableFood set status = N'Trống' where id = @idTable1
end
go 
-- update dữ liệu trạng thái 
update TableFood set status = N'Trống'

select * from Bill
delete BillInfo
delete Bill

alter table Bill add TotalPrice float
go
-- thuc thi lay hoa don theo ngay thang nao
create proc USP_GetListBillByDate
@checkIn date, @checkOut date
as
begin
	select tb.name as [Tên bàn], b.TotalPrice as[Tổng tiền], DateCheckIn as[Ngày vào], DateCheckOut as[Ngày thanh toán], discount as [Giảm giá] from Bill as b, TableFood as tb
	where DateCheckIn >= @checkIn and DateCheckOut <= @checkOut and b.status = 1
	and tb.id = b.idTable
end
go
create proc USP_UpdateAccount
@username nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0
	select @isRightPass = count(*) from Account where UserName = @username and PassWord = @password

	if(@isRightPass = 1)
	begin
		if(@newPassword is null or @newPassword = '')
		begin
			update Account set DisplayName = @displayName where UserName = @username
		end
		else
			update Account set DisplayName = @displayName, PassWord = @newPassword where UserName = @username
	end
end
go
select * from Food
go
create trigger UTG_DeleteBillInfo
on BillInfo for delete
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id, @idBill = deleted.idBill from deleted

	declare @idTable int
	select @idTable = idTable from Bill where id = @idBill

	declare @count int = 0
	select @count = count(*) from BillInfo as bi, Bill as b where b.id = bi.idBill and  b.id = @idBill and b.status = 0
	if(@count = 0)
		update TableFood set status = N'Trống' where id = @idTable
end
go
-- Hàm chuyển đổi tiếng việt có dấu thành không dấu cho mục đích tìm kiếm gần đúng nhất
CREATE FUNCTION fuConvertToUnsign1 ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
go
select Username, DisplayName, type from Account
go
select id as [Mã thức ăn], name as[Tên thức ăn], idCategory as[Danh mục] , price as[Giá]from Food
go
select * from Account
go