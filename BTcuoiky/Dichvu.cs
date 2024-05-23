using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTcuoiky
{
    public partial class Dichvu : Form
    {
        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = "select *from DichVu";
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
            dgvDichvu.DataSource = dt;

        }
        public Dichvu()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "INSERT INTO DichVu (MaDichVu, TenDichVu, DonGia) VALUES (@MaDichVu, @TenDichVu, @DonGia)";


            // Replace 'Column1', 'Column2', etc., with the actual column names of your 'Thuoc' table.

            // Adding parameters to avoid SQL Injection
            comman.Parameters.AddWithValue("@MaDichVu", txtMaDichvu.Text);
            comman.Parameters.AddWithValue("@TenDichVu",txtTenDichvu.Text);
           
            comman.Parameters.AddWithValue("@DonGia", txtDongia.Text);

            // Execute the query
            comman.ExecuteNonQuery();

            // Reload or refresh data
            loaddata();
        }

        private void Dichvu_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

        private void dgvDichvu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaDichvu.Text = dgvDichvu[0, dgvDichvu.CurrentRow.Index].Value.ToString();
            txtTenDichvu.Text = dgvDichvu[1, dgvDichvu.CurrentRow.Index].Value.ToString();
           
            txtDongia.Text = dgvDichvu[2, dgvDichvu.CurrentRow.Index].Value.ToString();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            string searchMaDichVu = txtMaDichvu.Text.Trim(); // Lấy giá trị Mã thuốc từ trường nhập liệu

            if (!string.IsNullOrEmpty(searchMaDichVu))
            {
                // Tạo câu lệnh SQL để tìm kiếm dựa vào Mã thuốc
                string sqlQuery = "SELECT * FROM DichVu WHERE MaDichVu LIKE @MaDV";

                // Tạo và thiết lập đối tượng SqlCommand
                comman = conect.CreateCommand();
                comman.CommandText = sqlQuery;
                comman.Parameters.AddWithValue("@MaDV", "%" + searchMaDichVu + "%"); // Sử dụng tham số để tránh SQL Injection

                // Thực hiện truy vấn và cập nhật DataGridView
                adapter.SelectCommand = comman;
                dt.Clear();
                adapter.Fill(dt);
                dgvDichvu.DataSource = dt;

                // Kiểm tra xem có dòng dữ liệu nào được tìm thấy hay không
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy Dịch vụ nào phù hợp với tìm kiếm của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập Mã dịch vụ cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Kiểm tra kết quả của hộp thoại xác nhận
            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Yes", thực hiện xóa dữ liệu
                comman = conect.CreateCommand();
                comman.CommandText = "delete from DichVu where MaDichVu= ('" + txtMaDichvu.Text + "')";
                comman.ExecuteNonQuery();
                loaddata();
            }
            else
            {
                // Nếu người dùng chọn "No" hoặc đóng hộp thoại xác nhận, không thực hiện xóa
                // Có thể thêm mã lệnh hoặc thông báo khác nếu cần
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            

            comman = conect.CreateCommand();
            comman.CommandText = "INSERT INTO DichVu (MaDichVu, TenDichVu, DonGia) VALUES (@MaDichVu, @TenDichVu, @DonGia)";

            // Thêm các tham số để tránh tình trạng SQL Injection
            comman.Parameters.AddWithValue("@MaDichVu", txtMaDichvu.Text);
            comman.Parameters.AddWithValue("@TenDichVu", txtTenDichvu.Text);
            comman.Parameters.AddWithValue("@DonGia", txtDongia.Text);

            // Thực hiện truy vấn
            int rowsInserted = comman.ExecuteNonQuery();

            if (rowsInserted > 0)
            {
                MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Thêm dịch vụ thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Xóa dữ liệu đang nhập trong các trường
            txtMaDichvu.Clear();
            txtTenDichvu.Clear();
            txtDongia.Clear();

            // Refresh hoặc reload dữ liệu nếu cần
            loaddata();
        }


        private void button2_Click(object sender, EventArgs e)
        {

            if (dgvDichvu.CurrentRow != null)
            {
                // Lấy thông tin dịch vụ từ các trường nhập liệu
                string maDichVu = txtMaDichvu.Text;
                string tenDichVu = txtTenDichvu.Text;
                decimal donGia;

                if (decimal.TryParse(txtDongia.Text, out donGia))
                {
                    // Tạo câu lệnh SQL để cập nhật thông tin dịch vụ
                    comman = conect.CreateCommand();
                    comman.CommandText = "UPDATE DichVu SET TenDichVu = @TenDichVu, DonGia = @DonGia WHERE MaDichVu = @MaDichVu";

                    // Thêm các tham số để tránh tình trạng SQL Injection
                    comman.Parameters.AddWithValue("@TenDichVu", tenDichVu);
                    comman.Parameters.AddWithValue("@DonGia", donGia);
                    comman.Parameters.AddWithValue("@MaDichVu", maDichVu);

                    // Thực hiện truy vấn cập nhật
                    int rowsUpdated = comman.ExecuteNonQuery();

                    if (rowsUpdated > 0)
                    {
                        MessageBox.Show("Sửa thông tin dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Sửa thông tin dịch vụ thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Refresh hoặc reload dữ liệu nếu cần
                    loaddata();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập giá trị đơn giá hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng dịch vụ cần sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    }

