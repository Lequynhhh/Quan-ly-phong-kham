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
    public partial class Benhnhan : Form
    {
     
            SqlConnection conect;
            SqlCommand comman;
            string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            void loaddata()
            {
                comman = conect.CreateCommand();
                comman.CommandText = "select *from BenhNhan";
                adapter.SelectCommand = comman;
                dt.Clear();
                adapter.Fill(dt);
                dgvListBN.DataSource = dt;

            }
            public Benhnhan()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Benhnhan_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

        private void dgvListBN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
                txtMabn.Text = dgvListBN[0, dgvListBN.CurrentRow.Index].Value.ToString();
                txtHoTen.Text = dgvListBN[1, dgvListBN.CurrentRow.Index].Value.ToString();
                cmbGioitinh.Text = dgvListBN[2, dgvListBN.CurrentRow.Index].Value.ToString();
                dateNgaysinh.Text = dgvListBN[3, dgvListBN.CurrentRow.Index].Value.ToString();
                txtDiachi.Text = dgvListBN[4, dgvListBN.CurrentRow.Index].Value.ToString();
                txtSdt.Text = dgvListBN[5, dgvListBN.CurrentRow.Index].Value.ToString();
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "INSERT INTO BenhNhan (MaBenhNhan, HoTen,GioiTinh, NgaySinh, Diachi, SDT) VALUES (@Mabn, @HoTen,@GioiTinh, @NgaySinh, @Diachi, @SDT)";


            // Replace 'Column1', 'Column2', etc., with the actual column names of your 'Thuoc' table.

            // Adding parameters to avoid SQL Injection
            comman.Parameters.AddWithValue("@Mabn", txtMabn.Text);
            comman.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
            comman.Parameters.AddWithValue("@GioiTinh", cmbGioitinh.Text);
            comman.Parameters.AddWithValue("@NgaySinh", dateNgaysinh.Text);
            comman.Parameters.AddWithValue("@Diachi", dateNgaysinh.Text);
            comman.Parameters.AddWithValue("@SDT", txtSdt.Text);
       

            // Execute the query
            comman.ExecuteNonQuery();

            // Reload or refresh data
            loaddata();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "delete from BenhNhan where MaBenhNhan= ('" + txtMabn.Text + "')";
            comman.ExecuteNonQuery();
            loaddata();
        }

        private void btntEdit_Click(object sender, EventArgs e)
        {

            comman = conect.CreateCommand();
            comman.CommandText = "UPDATE BenhNhan SET HoTen = @TenThuoc, Gioitinh=@GioiTinh, NgaySinh=@Ngaysinh, DiaChi = @DiaChi, SDT=@SDT WHERE MaBenhNhan= @MaBN";

            comman.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
            comman.Parameters.AddWithValue("@GioiTinh", cmbGioitinh.Text);
            comman.Parameters.AddWithValue("@NgaySinh", dateNgaysinh.Text);
            comman.Parameters.AddWithValue("@Diachi", dateNgaysinh.Text);
            comman.Parameters.AddWithValue("@SDT", txtSdt.Text);
            comman.Parameters.AddWithValue("@Mabn", txtMabn.Text);

            comman.ExecuteNonQuery();

            loaddata();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra kết quả của hộp thoại xác nhận
            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Yes", thực hiện xóa dữ liệu
                comman = conect.CreateCommand();
                comman.CommandText = "delete from BenhNhan where MaBenhNhan= ('" + txtMabn.Text + "')";
                comman.ExecuteNonQuery();
                loaddata();
            }
            else
            {
                // Nếu người dùng chọn "No" hoặc đóng hộp thoại xác nhận, không thực hiện xóa
                // Có thể thêm mã lệnh hoặc thông báo khác nếu cần
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtMabn.Text))
            {
                comman = conect.CreateCommand();
                comman.CommandText = "SELECT * FROM BenhNhan WHERE MaBenhNhan LIKE @Mabn";

                // Adding parameters to avoid SQL Injection
                comman.Parameters.AddWithValue("@Mabn", "%" + txtMabn.Text + "%");

                // Execute the command and load data into the grid view
                adapter.SelectCommand = comman;
                dt.Clear();
                adapter.Fill(dt);
                dgvListBN.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy bệnh nhân nào phù hợp với tìm kiếm của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập Mã bệnh nhân cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    

        private void button4_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "INSERT INTO BenhNhan (MaBenhNhan, HoTen, GioiTinh, NgaySinh, Diachi, SDT) VALUES (@Mabn, @HoTen, @GioiTinh, @NgaySinh, @Diachi, @SDT)";

            // Adding parameters to avoid SQL Injection
            comman.Parameters.AddWithValue("@Mabn", txtMabn.Text);
            comman.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
            comman.Parameters.AddWithValue("@GioiTinh", cmbGioitinh.Text);
            comman.Parameters.AddWithValue("@NgaySinh", dateNgaysinh.Value); // Sử dụng Value của DateTimePicker để lấy ngày
            comman.Parameters.AddWithValue("@Diachi", txtDiachi.Text);
            comman.Parameters.AddWithValue("@SDT", txtSdt.Text);

            // Execute the query
            comman.ExecuteNonQuery();
            MessageBox.Show("Thêm bệnh nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Xóa dữ liệu đang hiển thị trong các trường nhập liệu
            txtMabn.Clear();
            txtHoTen.Clear();
            cmbGioitinh.SelectedIndex = -1;
            dateNgaysinh.Value = DateTime.Now; // Đặt ngày thành ngày hiện tại
            txtDiachi.Clear();
            txtSdt.Clear();

            // Reload hoặc refresh dữ liệu (nếu bạn muốn)
            loaddata();
            // Reload or refresh data
            loaddata();
        }

        private void button2_Click(object sender, EventArgs e)
        {


            if (!string.IsNullOrEmpty(txtMabn.Text))
            {
                comman = conect.CreateCommand();
                comman.CommandText = "UPDATE BenhNhan SET HoTen = @HoTen, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, Diachi = @Diachi, SDT = @SDT WHERE MaBenhNhan = @MaBenhNhan";

                // Thêm tham số cho câu lệnh SQL
                comman.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                comman.Parameters.AddWithValue("@GioiTinh", cmbGioitinh.Text);
                comman.Parameters.AddWithValue("@NgaySinh", dateNgaysinh.Value); // Sử dụng Value của DateTimePicker để lấy ngày
                comman.Parameters.AddWithValue("@Diachi", txtDiachi.Text);
                comman.Parameters.AddWithValue("@SDT", txtSdt.Text);
                comman.Parameters.AddWithValue("@MaBenhNhan", txtMabn.Text);

                // Thực hiện câu lệnh cập nhật
                comman.ExecuteNonQuery();

                // Sau khi cập nhật thành công, bạn có thể thông báo cho người dùng
                MessageBox.Show("Sửa thông tin bệnh nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload hoặc refresh dữ liệu (nếu bạn muốn)
                loaddata();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân cần sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
