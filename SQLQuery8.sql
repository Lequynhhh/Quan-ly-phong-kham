create database qlpk
use qlpk
CREATE TABLE BacSi (
    MaBacSi NVARCHAR(25) PRIMARY KEY,
    HoTen NVARCHAR(25),
    GioiTinh NVARCHAR(10) ,
    NgaySinh DATE,
    SDT VARCHAR(20),
    Email VARCHAR(50),
    ChuyenMon NVARCHAR(30)
);
-- Giả sử bạn đã có tên đăng nhập và mật khẩu từ người dùng
DECLARE @TenDangNhap NVARCHAR(50);
DECLARE @MatKhau NVARCHAR(50);

-- Gán giá trị cho biến (trong trường hợp thực tế, giá trị này sẽ được lấy từ form đăng nhập)
SET @TenDangNhap = N'lequynh'; -- Tên đăng nhập người dùng nhập vào
SET @MatKhau = N'22022003'; -- Mật khẩu người dùng nhập vào

-- Lấy thông tin bác sĩ dựa trên tên đăng nhập và mật khẩu
SELECT bs.MaBacSi, bs.HoTen, bs.GioiTinh, bs.NgaySinh, bs.SDT, bs.Email, bs.ChuyenMon
FROM BacSi bs
INNER JOIN TaiKhoanBacSi tkbs ON bs.MaBacSi = tkbs.MaBacSi
WHERE tkbs.TenDangNhap = @TenDangNhap AND tkbs.MatKhau = @MatKhau;
select *from BacSi
use qlpk
SELECT
    dt.MaDonThuoc,
    dt.MaPhieuKham,
    dt.MaBacSi,
    bs.HoTen AS HoTenBacSi,
    dt.NgayKeDon
FROM DonThuoc dt
JOIN BacSi bs ON dt.MaBacSi = bs.MaBacSi;
CREATE TABLE BenhNhan (
    MaBenhNhan NVARCHAR(25) PRIMARY KEY,
    HoTen NVARCHAR(25),
    GioiTinh NVARCHAR(10) CHECK ,
    NgaySinh DATE,
    DiaChi NVARCHAR(255),
    SDT VARCHAR(20)
);

CREATE TABLE DichVu (
    MaDichVu NVARCHAR(25) PRIMARY KEY,
    TenDichVu NVARCHAR(50),
    DonGia DECIMAL(18, 2)
);

CREATE TABLE PhongKham (
    MaPhongKham NVARCHAR(25)  PRIMARY KEY,
    TenPhong NVARCHAR(50),
);

CREATE TABLE Thuoc (
    MaThuoc  NVARCHAR(25) PRIMARY KEY,
    TenThuoc NVARCHAR(100),
    NSX DATE,
    HSD DATE,
    DonViTinh NVARCHAR(20),
    DonGia DECIMAL(18, 2)
);
CREATE TABLE PhieuKham (
    MaPhieuKham NVARCHAR(25) PRIMARY KEY,
    MaBenhNhan NVARCHAR(25),
    MaDichVu NVARCHAR(25),
    MaPhongKham NVARCHAR(25),
    SoThuTu INT,
    NgayKham DATE,
    TrieuChung NVARCHAR(255),
    ChanDoan NVARCHAR(255),
    FOREIGN KEY (MaBenhNhan) REFERENCES BenhNhan(MaBenhNhan),
    FOREIGN KEY (MaDichVu) REFERENCES DichVu(MaDichVu),
    FOREIGN KEY (MaPhongKham) REFERENCES PhongKham(MaPhongKham)
);
CREATE TABLE DonThuoc (
    MaDonThuoc NVARCHAR(25) PRIMARY KEY,
    MaBacSi NVARCHAR(25),
    MaPhieuKham NVARCHAR(25),
    NgayKeDon DATE,
    FOREIGN KEY (MaBacSi) REFERENCES BacSi(MaBacSi),
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham)
);
CREATE TABLE ChiTietDonThuoc (
    MaDonThuoc NVARCHAR(25),
    MaThuoc NVARCHAR(25),
    SoLuong INT,
    CachDung NVARCHAR(255),
    DonGia DECIMAL(18, 2),
    FOREIGN KEY (MaDonThuoc) REFERENCES DonThuoc(MaDonThuoc),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc)
);
select *from TaiKhoanBacSi
CREATE TABLE TaiKhoanBacSi (
    MaTaiKhoan NVARCHAR(25) PRIMARY KEY,
    MaBacSi NVARCHAR(25),
   
    TenDangNhap NVARCHAR(50) UNIQUE,
    MatKhau NVARCHAR(50),
    FOREIGN KEY (MaBacSi) REFERENCES BacSi(MaBacSi),
   
);

CREATE TABLE HoaDon (
    MaHoaDon NVARCHAR(36) PRIMARY KEY,
    MaDonThuoc NVARCHAR(25),
    MaPhieuKham NVARCHAR(25),
    MaBenhNhan NVARCHAR(25),
    NgayLap DATE,
    TongTien DECIMAL(18, 2),
    FOREIGN KEY (MaDonThuoc) REFERENCES DonThuoc(MaDonThuoc),
    FOREIGN KEY (MaPhieuKham) REFERENCES PhieuKham(MaPhieuKham),
    FOREIGN KEY (MaBenhNhan) REFERENCES BenhNhan(MaBenhNhan)
);
drop table hoadon
drop table ChiTietDonThuoc

WITH TongTienDonThuoc AS (
    SELECT dt.MaDonThuoc, SUM(ctdt.SoLuong * ctdt.DonGia) AS TongTien
    FROM DonThuoc dt
    JOIN ChiTietDonThuoc ctdt ON dt.MaDonThuoc = ctdt.MaDonThuoc
    GROUP BY dt.MaDonThuoc
),
TienDichVu AS (
    SELECT pk.MaPhieuKham, dv.DonGia AS TienDichVu
    FROM PhieuKham pk
    JOIN DichVu dv ON pk.MaDichVu = dv.MaDichVu
),
TongTienHoaDon AS (
    SELECT pk.MaPhieuKham, ISNULL(tt.TongTien, 0) + ISNULL(dv.TienDichVu, 0) AS TongTienHoaDon
    FROM PhieuKham pk
    LEFT JOIN DonThuoc dt ON pk.MaPhieuKham = dt.MaPhieuKham
    LEFT JOIN TongTienDonThuoc tt ON dt.MaDonThuoc = tt.MaDonThuoc
    LEFT JOIN TienDichVu dv ON pk.MaPhieuKham = dv.MaPhieuKham
)

--Tạo hóa đơn với tổng tiền
INSERT INTO HoaDon(MaHoaDon, MaDonThuoc, MaPhieuKham, MaBenhNhan, NgayLap, TongTien)
SELECT CAST(NEWID() AS NVARCHAR(36)) AS MaHoaDon,
       dt.MaDonThuoc,
       pk.MaPhieuKham,
       pk.MaBenhNhan,
       dt.NgayKeDon AS NgayLap,
       tt.TongTienHoaDon
FROM PhieuKham pk
LEFT JOIN DonThuoc dt ON pk.MaPhieuKham = dt.MaPhieuKham
LEFT JOIN TongTienHoaDon tt ON pk.MaPhieuKham = tt.MaPhieuKham
WHERE NOT EXISTS (
    SELECT 1
    FROM HoaDon hd
    WHERE hd.MaPhieuKham = pk.MaPhieuKham
    AND hd.MaDonThuoc = dt.MaDonThuoc
);

select *from bacsi
            select* from HoaDon;
		
	use qlpk
	drop table hoadon

SELECT
    MaThuoc,
    TenThuoc,
    HSD,
    CASE
        WHEN HSD < GETDATE() THEN N'Hết hạn'
        WHEN HSD <= DATEADD(day, 30, GETDATE()) THEN N'Sắp hết hạn'
        ELSE N'Còn hiệu lực'
    END AS TrangThai
FROM Thuoc;
INSERT INTO BacSi (MaBacSi, HoTen, GioiTinh, NgaySinh, SDT, Email, ChuyenMon)
VALUES

    ('BS001', N'Nguyễn Văn Anh', N'Nam', '1980-05-10', '0123456789', 'nguyenvana@email.com', N'Nội khoa'),
    ('BS002', N'Trần Thị Bích', N'Nữ', '1985-08-15', '0987654321', 'tranthib@email.com', N'Nhi khoa'),
    ('BS003', N'Lê Văn Cường', N'Nam', '1990-03-20', '0365478963', 'levanc@email.com', N'Da liễu'),
    ('BS004', N'Phạm Thị Bảo', N'Nữ', '1982-12-25', '0999888777', 'phamthid@email.com', N'Mắt'),
    ('BS005', N'Hoàng Văn Dũng', N'Nam', '1978-07-05', '0111222333', 'hoangvane@email.com', N'Răng hàm mặt'),
	('BS006', N'Lê Thị Quỳnh', N'Nữ', '1978-07-19', '0989316405', 'lequynh@email.com', N'Răng hàm mặt'),
	('BS007', N'Trần Bảo Châu', N'Nữ', '1972-03-05', '0989316405', 'Chau@email.com', N'Tai mũi họng');

INSERT INTO BenhNhan (MaBenhNhan, HoTen, GioiTinh, NgaySinh, DiaChi, SDT)
VALUES
    ('BN001', N'Nguyễn Thị Ngọc', N'Nữ', '1995-04-12', N'123 Đường Hai Bà Trưng , Quận 1, TP.HCM', '0988777666'),
    ('BN002', N'Trần Văn Giang', 'Nam', '1990-06-20', N'456 Đường Trần Quốc Tuấn, Quận 2, TP.HCM', '0977666555'),
    ('BN003', N'Lê Thị Hòa', N'Nữ', '2000-01-05', N'789 Đường Trương Định, Hoàng Mai, TP.Hà Nội', '0943301553'),
    ('BN004', N'Phạm Văn I', 'Nam', '1988-09-15', N'101 Đường Thống Nhất, Quận 4, TP.HCM', '0966555444'),
    ('BN005', N'Hoàng Thị Khánh', N'Nữ', '1987-03-30', N'222 Đường Trần Mai Ninh, Quận 5, TP.Thanh Hóa', '0989456763');
INSERT INTO DichVu (MaDichVu, TenDichVu, DonGia)
VALUES
    ('DV001', N'Khám sức khỏe tổng quát', 800000.00),
    ('DV002', N'Siêu âm ', 1000000.000),
    ('DV003', N'Xét nghiệm máu', 400000.000),
    ('DV004', N'Chụp X-quang', 250000.000),
    ('DV005', N'Khám mắt', 120000.000);
	delete from Thuoc
INSERT INTO PhongKham (MaPhongKham, TenPhong)
VALUES
    ('PK001', N'Phòng khám nội khoa'),
    ('PK002', N'Phòng khám nhi khoa'),
    ('PK003', N'Phòng khám da liễu'),
    ('PK004', N'Phòng khám mắt'),
    ('PK005', N'Phòng khám răng hàm mặt'),
	('PK006', N'Phòng khám tai mũi họng'),
	('PK007', N'Phòng khám tổng quát');
	select *from thuoc

INSERT INTO PhieuKham (MaPhieuKham, MaBenhNhan, MaDichVu, MaPhongKham, SoThuTu, NgayKham, TrieuChung, ChanDoan)
VALUES
    ('PK001', 'BN001', 'DV001', 'PK001', 1, '2023-01-08', N'Sốt và đau đầu', N'Cảm nhiễm'),
    ('PK002', 'BN002', 'DV002', 'PK002', 2, '2024-01-08', N'Buồn nôn và nôn mửa', N'Nhiễm khuẩn đường tiêu hóa'),
    ('PK003', 'BN003', 'DV003', 'PK003', 3, '2024-01-08', N'Sưng và ngứa da', N'Dị ứng da liễu'),
    ('PK004', 'BN004', 'DV004', 'PK004', 4, '2023-01-09', N'Đau bên trái ngực', N'Viêm phổi'),
    ('PK005', 'BN005', 'DV005', 'PK005', 5, '2023-04-08', N'Khó thở và sưng mắt', N'Viêm kết mạc'),
	('PK006', 'BN001', 'DV005', 'PK004', 5, '2024-01-08', N'Mắt ngứa và đỏ', N'Viêm kết mạc');
;
INSERT INTO DonThuoc (MaDonThuoc, MaBacSi, MaPhieuKham, NgayKeDon)
VALUES
    ('DT001', 'BS001', 'PK001', '2024-01-08'),
    ('DT002', 'BS002', 'PK002', '2024-01-08'),
    ('DT003', 'BS003', 'PK003', '2024-01-08'),
    ('DT004', 'BS004', 'PK004', '2023-01-09'),
    ('DT005', 'BS005', 'PK005', '2023-04-08');
INSERT INTO Thuoc (MaThuoc, TenThuoc, NSX, HSD, DonViTinh, DonGia)
VALUES
    ('T001', 'Paracetamol', '2023-01-15', '2025-01-15', N'Vỉ', 10000.00),
    ('T002', 'Amoxicillin', '2023-03-10', '2025-03-10', N'Hộp', 150000.000),
    ('T003', 'Ibuprofen', '2023-02-20', '2025-02-20', N'Viên', 8000.000),
    ('T004', 'Loratadine', '2023-04-05', '2025-04-05', N'Vỉ', 12000.00),
    ('T005', 'Cetirizine', '2023-06-15', '2025-06-15', N'Hộp', 90000.000),
	('T006', 'Vacxin', '2023-02-20', '2024-01-20', N'Viên', 8000.00),
    ('T007', 'Allerphast', '2023-04-05', '2023-12-23', N'Viên', 12000.00),
    ('T008', 'Danapha', '2023-06-15', '2024-01-1', N'Viên', 9000.00);
	delete from Thuoc
INSERT INTO ChiTietDonThuoc (MaDonThuoc, MaThuoc, SoLuong, CachDung, DonGia)
VALUES
    ('DT001', 'T001', 10, N'Uống 1 viên mỗi ngày', 10000.00),
    ('DT001', 'T002', 5, N'Uống 2 viên mỗi ngày', 150000.000),
    ('DT002', 'T003', 7, N'Uống 1 viên mỗi 8 giờ', 8000.000),
    ('DT002', 'T004', 4, N'Uống 1 viên trước bữa ăn', 12000.00),
    ('DT003', 'T005', 10, N'Uống 1 viên mỗi tối', 90000.000),
    ('DT004', 'T006', 3, N'Uống 1 viên khi cảm thấy đau', 8000.00),
    ('DT005', 'T007', 5, N'Uống 1 viên mỗi 12 giờ', 12000.00),
    ('DT005', 'T008', 6, N'Uống 1 viên sau bữa ăn', 9000.00);
INSERT INTO TaiKhoanBacSi (MaTaiKhoan, MaBacSi, TenDangNhap, MatKhau)
VALUES
    ('TKBS001', 'BS001', 'bsnguyenvana', 'password1'),
    ('TKBS002', 'BS002', 'bstranthib', 'password2'),
    ('TKBS003', 'BS003', 'bslevanc', 'password3'),
    ('TKBS004', 'BS004', 'bsphamthid', 'password4'),
    ('TKBS005', 'BS005', 'bshoangvane', 'password5'),
    ('TKBS006', 'BS006', 'lequynh', '22022003');

	use qlpk
	delete from hoadon
	delete from ChiTietDonThuoc
	delete from DonThuoc
	delete from Thuoc
	delete from PhieuKham
	delete from DichVu
	delete from BenhNhan
	delete from TaiKhoanBacSi
	delete from BacSi
	delete from PhongKham
SELECT 
    NgayLap AS [Ngày],
    SUM(TongTien) AS [Doanh Thu]
FROM HoaDon
GROUP BY NgayLap
SELECT 
  DATEPART(MONTH, NgayLap) AS [Tháng],
    DATEPART(YEAR, NgayLap) AS [Năm],
   
    SUM(TongTien) AS [Doanh Thu]
FROM HoaDon
GROUP BY  DATEPART(MONTH, NgayLap),DATEPART(YEAR, NgayLap)
ORDER BY [Tháng],[Năm]

