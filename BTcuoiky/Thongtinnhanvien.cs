using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace BTcuoiky
{
    public partial class Thongtinnhanvien : Form
    {
        SqlConnection connect;
        SqlCommand command;
        string connectionString = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();

        public Thongtinnhanvien()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (connect == null || connect.State != ConnectionState.Open)
            {
                connect = new SqlConnection(connectionString);
                connect.Open();
            }

            command = connect.CreateCommand();
            command.CommandText = "UPDATE BacSi SET HoTen = @Hoten, GioiTinh = @Gioitinh, NgaySinh = @Ngaysinh, " +
                                  "SDT = @sdt, Email = @email WHERE Mabacsi = @Mabs";

            command.Parameters.AddWithValue("@Mabs", txtMabs.Text);
            command.Parameters.AddWithValue("@Hoten", txtHoten.Text);
            command.Parameters.AddWithValue("@Gioitinh", cmbGioiTinh.Text);
            command.Parameters.AddWithValue("@Ngaysinh", datengaysinh.Value);
            command.Parameters.AddWithValue("@sdt", txtsdt.Text);
            command.Parameters.AddWithValue("@email", txtemail.Text);

            int rowsUpdated = command.ExecuteNonQuery();

            if (rowsUpdated > 0)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không có bản ghi nào được cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void Thongtinnhanvien_Load_1(object sender, EventArgs e)
        {
            try
            {
                connect = new SqlConnection(connectionString);
                connect.Open();


                // Sử dụng truy vấn parameterized để ngăn chặn SQL Injection
                string query = @"SELECT bs.MaBacSi, bs.HoTen, bs.GioiTinh, bs.NgaySinh, bs.SDT, bs.Email, bs.ChuyenMon
                        FROM BacSi bs
                        INNER JOIN TaiKhoanBacSi tkbs ON bs.MaBacSi = tkbs.MaBacSi
                        WHERE tkbs.TenDangNhap = @TenDangNhap AND tkbs.MatKhau = @MatKhau;";

                using (command = new SqlCommand(query, connect))
                {
                    // Thêm tham số vào câu lệnh SQL
                    command.Parameters.AddWithValue("@TenDangNhap", "lequynh");
                    command.Parameters.AddWithValue("@MatKhau", "2202");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Đảm bảo tên cột đúng như trong cơ sở dữ liệu
                            txtMabs.Text = reader["MaBacSi"].ToString();
                            txtHoten.Text = reader["HoTen"].ToString();
                            cmbGioiTinh.Text = reader["GioiTinh"].ToString();
                            datengaysinh.Value = Convert.ToDateTime(reader["NgaySinh"]);
                            txtsdt.Text = reader["SDT"].ToString();
                            txtemail.Text = reader["Email"].ToString();
                            cmbchuyenmon.Text = reader["ChuyenMon"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin tài khoản.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}

