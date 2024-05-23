using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTcuoiky
{
    public partial class Phongkham : Form
    {
        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = "select *from Phongkham";
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
           dgvPhongkham.DataSource = dt;

        }
        public Phongkham()
        {
            InitializeComponent();
        }

        private void dgvPhongkham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMapk.Text = dgvPhongkham[0, dgvPhongkham.CurrentRow.Index].Value.ToString();
            txtTenpk.Text = dgvPhongkham[1, dgvPhongkham.CurrentRow.Index].Value.ToString();

        }

        private void Phongkham_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "SELECT * FROM PhongKham WHERE MaPhongKham LIKE @Mapk";

            // Add parameter to prevent SQL injection
            comman.Parameters.AddWithValue("@Mapk", "%" + txtMapk.Text + "%");

            // Execute the command and load data into the grid view
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
            dgvPhongkham.DataSource = dt;
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
                comman.CommandText = "delete from PhongKham where MaPhongKham= ('" + txtMapk.Text + "')";
                comman.ExecuteNonQuery();
                loaddata();
            }
            else
            {
                // Nếu người dùng chọn "No" hoặc đóng hộp thoại xác nhận, không thực hiện xóa
                // Có thể thêm mã lệnh hoặc thông báo khác nếu cần
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            comman = conect.CreateCommand();
            comman.CommandText = "INSERT INTO PhongKham (MaPhongKham, TenPhongKham) VALUES (@MaPhongKham, @TenPhongKham)";

            // Thêm các tham số để tránh tình trạng SQL Injection
            comman.Parameters.AddWithValue("@MaPhongKham", txtMapk.Text);
            comman.Parameters.AddWithValue("@TenPhongKham", txtTenpk.Text);

            // Thực hiện truy vấn thêm
            int rowsInserted = comman.ExecuteNonQuery();

            if (rowsInserted > 0)
            {
                MessageBox.Show("Thêm phòng khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Thêm phòng khám thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Xóa dữ liệu đang nhập trong các trường
            txtMapk.Clear();
            txtTenpk.Clear();

            // Refresh hoặc reload dữ liệu nếu cần
            loaddata();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            comman = conect.CreateCommand();
            comman.CommandText = "UPDATE PhongKham SET TenPhongKham = @TenPhongKham WHERE MaPhongKham = @MaPhongKham";

            // Thêm các tham số để tránh tình trạng SQL Injection
            comman.Parameters.AddWithValue("@TenPhongKham", txtTenpk.Text);
            comman.Parameters.AddWithValue("@MaPhongKham", txtMapk.Text);

            // Thực hiện truy vấn sửa thông tin
            int rowsUpdated = comman.ExecuteNonQuery();

            if (rowsUpdated > 0)
            {
                MessageBox.Show("Sửa thông tin phòng khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Sửa thông tin phòng khám thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Refresh hoặc reload dữ liệu nếu cần
            loaddata();
        }
    }
}
