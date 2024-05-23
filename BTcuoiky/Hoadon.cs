using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace BTcuoiky
{
    public partial class Hoadon : Form
    {

        

        private int currentPageIndex = 0;
        private int rowsPerPage = 20;
        private int columnIndex = 0;
        private int rowIndex = 0;
        private const int startX = 10;
        private const int startY = 10;
        private const int offset = 40;
        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = @"
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
       CASE 
           WHEN dt.NgayKeDon IS NOT NULL THEN dt.NgayKeDon
           ELSE pk.NgayKham
       END AS NgayLap,
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

select *from Hoadon


";

            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
            dgvHD.DataSource = dt;
        }

        public Hoadon()
        {
            InitializeComponent();
            printPreviewDialog1.Document = printDocument1;

        }

        private void Hoadon_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (KiemTraTonTaiMaphieukham(txtphieukham.Text ))
            {
                comman = conect.CreateCommand();
                comman.CommandText = "INSERT INTO HoaDon (MaHoaDon,MaDonThuoc, MaPhieuKham,MaBenhNhan,NgayLap) VALUES (@MaHoaDon, @MaDonThuoc, @MaPhieuKham, @MaBenhNhan, @NgayLap)";

                // Adding parameters to avoid SQL Injection
                comman.Parameters.AddWithValue("@MaHoaDon", txtMaHD.Text);
                comman.Parameters.AddWithValue("@MaDonThuoc", txtdonthuoc.Text);
                comman.Parameters.AddWithValue("@MaPhieuKham", txtphieukham.Text);
                comman.Parameters.AddWithValue("@MaBenhNhan", txtBenhNhan.Text);
                comman.Parameters.AddWithValue("@NgayLap", dtNgaylap.Text);

                // Execute the query
                comman.ExecuteNonQuery();

                // Reload or refresh data
                loaddata();
            }
            else
            {
                MessageBox.Show("Mã phiếu khám không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraTonTaiMaphieukham(string mapk)
        {
            string query = "SELECT COUNT(*) FROM PhieuKham WHERE MaPhieuKham = @Mapk";

            using (SqlCommand command = new SqlCommand(query, conect))
            {
                command.Parameters.AddWithValue("@Mapk", mapk);
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "delete from HoaDon where MaHoaDon= ('" + txtMaHD.Text + "')";
            comman.ExecuteNonQuery();
            loaddata();
        }

        private void dgvHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaHD.Text = dgvHD[0, dgvHD.CurrentRow.Index].Value.ToString();
            txtphieukham.Text = dgvHD[1, dgvHD.CurrentRow.Index].Value.ToString();
           txtdonthuoc.Text = dgvHD[2, dgvHD.CurrentRow.Index].Value.ToString();
            txtBenhNhan.Text = dgvHD[3, dgvHD.CurrentRow.Index].Value.ToString();
            dtNgaylap.Text= dgvHD[4, dgvHD.CurrentRow.Index].Value.ToString();
            txtTongtien.Text = dgvHD[5, dgvHD.CurrentRow.Index].Value.ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            // Ensure you declare and initialize the boldFont if you haven't already
            Font boldFont = new Font("Arial", 12, FontStyle.Bold); // Adjust size as needed

            using (Font headerFont = new Font("Arial", 18, FontStyle.Bold))
            using (Font titleFont = new Font("Arial", 16, FontStyle.Bold))
            using (Font bodyFont = new Font("Arial", 12))
            {
                float lineHeight = bodyFont.GetHeight();
                float pageWidth = e.PageBounds.Width;
                float pageHeight = e.PageBounds.Height;
                float y = 60; // Top margin, adjust if needed
                Font signatureFont = new Font("Arial", 12, FontStyle.Italic); // Font cho chữ ký

                // Centered header text
                string header = "Hóa Đơn Phòng Khám";
                float headerWidth = graphics.MeasureString(header, headerFont).Width;
                float headerX = (pageWidth - headerWidth) / 2;
                graphics.DrawString(header, headerFont, Brushes.Blue, headerX, y);
                y += lineHeight + 10; // Adjust space after header

                // Left-aligned text
                float indentX = 100; // Left margin for invoice details

               

                graphics.DrawString("Mã HĐ: " + txtMaHD.Text, titleFont, Brushes.Black, indentX, y);
                y += graphics.MeasureString("Mã HĐ: ", titleFont).Height;

                graphics.DrawString("Mã Phiếu Khám: " + txtphieukham.Text, bodyFont, Brushes.Black, indentX, y);
                y += lineHeight;

                graphics.DrawString("Mã Đơn Thuốc: " + txtdonthuoc.Text, bodyFont, Brushes.Black, indentX, y);
                y += lineHeight;

                graphics.DrawString("Mã Bệnh Nhân: " + txtBenhNhan.Text, bodyFont, Brushes.Black, indentX, y);
                y += lineHeight;

                graphics.DrawString("Ngày Lập: " + dtNgaylap.Text, bodyFont, Brushes.Black, indentX, y);
                y += lineHeight;

                graphics.DrawString("Tổng Tiền: " + txtTongtien.Text, titleFont, Brushes.Black, indentX, y);

                // Signature line at the bottom right
                string signatureText = "Chữ ký (Bệnh nhân):";
                SizeF signatureSize = graphics.MeasureString(signatureText, signatureFont);
                float signatureX = pageWidth - signatureSize.Width - 80; // 80 là lề phải, bạn có thể điều chỉnh
                float signatureY = pageHeight - signatureSize.Height - 650; // 50 là lề dưới, bạn có thể điều chỉnh

                // Vẽ dòng chữ ký
                graphics.DrawString(signatureText, signatureFont, Brushes.Black, signatureX, signatureY);

                // Vẽ đường cho chữ ký nếu bạn muốn
                graphics.DrawLine(Pens.Black, signatureX, signatureY + signatureSize.Height, pageWidth - 80, signatureY + signatureSize.Height);

                e.HasMorePages = false;
            }
        }
    
        
    }
    
}