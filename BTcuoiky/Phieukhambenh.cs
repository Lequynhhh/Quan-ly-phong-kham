using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTcuoiky
{
    public partial class Phieukhambenh : Form
    {
        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = "select *from PhieuKham";
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
            dgvListPK.DataSource = dt;

        }
        public Phieukhambenh()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dgvListPK_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmapk.Text = dgvListPK[0, dgvListPK.CurrentRow.Index].Value.ToString();
            txtMabn.Text = dgvListPK[1, dgvListPK.CurrentRow.Index].Value.ToString();
            cmbDv.Text = dgvListPK[2, dgvListPK.CurrentRow.Index].Value.ToString();
            cmbphongkham.Text = dgvListPK[3, dgvListPK.CurrentRow.Index].Value.ToString();
            txtstt.Text = dgvListPK[4, dgvListPK.CurrentRow.Index].Value.ToString();
            dateNgaykham.Text = dgvListPK[5, dgvListPK.CurrentRow.Index].Value.ToString();
            txttrieuchung.Text = dgvListPK[6, dgvListPK.CurrentRow.Index].Value.ToString();
            txtchuandoan.Text = dgvListPK[7, dgvListPK.CurrentRow.Index].Value.ToString();
        }

        private void Phieukhambenh_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Donthuoc donthuoc = new Donthuoc();
            donthuoc.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem mã bệnh nhân có tồn tại không
            if (KiemTraTonTaiMaBenhNhan(txtMabn.Text))
            {
                comman = conect.CreateCommand();
                comman.CommandText = "INSERT INTO PhieuKham (MaPhieukham, MaBenhNhan, MaDichVu, MaPhongKham, Sothutu, NgayKham, TrieuChung, ChanDoan) VALUES (@Maphieukham, @MaBN, @Madv, @mapk, @Stt, @Ngaykham, @Trieuchung, @Chandoan)";

                // Adding parameters to avoid SQL Injection
                comman.Parameters.AddWithValue("@Maphieukham", txtmapk.Text);
                comman.Parameters.AddWithValue("@MaBN", txtMabn.Text);
                comman.Parameters.AddWithValue("@Madv", cmbDv.Text);
                comman.Parameters.AddWithValue("@mapk", cmbphongkham.Text);
                comman.Parameters.AddWithValue("@Stt", txtstt.Text);
                comman.Parameters.AddWithValue("@Ngaykham", dateNgaykham.Text);
                comman.Parameters.AddWithValue("@Trieuchung", txttrieuchung.Text);
                comman.Parameters.AddWithValue("@Chandoan", txtchuandoan.Text);

                // Execute the query
                comman.ExecuteNonQuery();

                // Reload or refresh data
                loaddata();
            }
            else
            {
                MessageBox.Show("Mã bệnh nhân không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraTonTaiMaBenhNhan(string maBenhNhan)
        {
            // Thực hiện truy vấn kiểm tra xem mã bệnh nhân có tồn tại trong cơ sở dữ liệu không
            // Đây là một ví dụ giả định, bạn cần thay thế nó bằng truy vấn thực sự kiểm tra trong cơ sở dữ liệu của bạn.
            string query = "SELECT COUNT(*) FROM BenhNhan WHERE MaBenhNhan = @MaBenhNhan";

            using (SqlCommand command = new SqlCommand(query, conect))
            {
                command.Parameters.AddWithValue("@MaBenhNhan", maBenhNhan);
                int count = (int)command.ExecuteScalar();

                // Nếu count > 0, tức là mã bệnh nhân tồn tại
                return count > 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra kết quả của hộp thoại xác nhận
            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Yes", thực hiện xóa dữ liệu
                comman = conect.CreateCommand();
                comman.CommandText = "delete from PhieuKham where MaPhieuKham= ('" + txtmapk.Text + "')";
                comman.ExecuteNonQuery();
                loaddata();
            }
            else
            {
                // Nếu người dùng chọn "No" hoặc đóng hộp thoại xác nhận, không thực hiện xóa
                // Có thể thêm mã lệnh hoặc thông báo khác nếu cần
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvListPK.SelectedRows.Count > 0)
            {
                comman = conect.CreateCommand();
                comman.CommandText = "UPDATE PhieuKham SET MaBenhNhan = @MaBN, MaDichVu = @Madv, MaPhongKham = @mapk, Sothutu = @Stt, NgayKham = @Ngaykham, TrieuChung = @Trieuchung, ChanDoan = @Chandoan WHERE MaPhieuKham = @MaPhieukham";

                // Thêm tham số cho câu lệnh SQL
                comman.Parameters.AddWithValue("@MaBN", txtMabn);
                comman.Parameters.AddWithValue("@Madv", cmbDv);
                comman.Parameters.AddWithValue("@mapk", cmbphongkham);
                comman.Parameters.AddWithValue("@Stt", txtstt);
                comman.Parameters.AddWithValue("@Ngaykham", dateNgaykham);
                comman.Parameters.AddWithValue("@Trieuchung", txttrieuchung);
                comman.Parameters.AddWithValue("@Chandoan", txtchuandoan);
                comman.Parameters.AddWithValue("@MaPhieukham", txtmapk);

                // Thực hiện câu lệnh cập nhật
                comman.ExecuteNonQuery();

                // Sau khi cập nhật thành công, bạn cần reload hoặc refresh dữ liệu
                loaddata();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu khám cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

}