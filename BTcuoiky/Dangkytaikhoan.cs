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
    public partial class Dangkytaikhoan : Form
    {

        SqlConnection conect;
        SqlCommand comman;

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True");
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        public Dangkytaikhoan()
        {
            InitializeComponent();
        }
      
        

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                if (txtMatk.Text.Trim() == "" || txtMabs.Text.Trim() == "" || txthoten.Text.Trim() == "" || txttendangnhap.Text.Trim() == "" ||
                txtMatkhau.Text.Trim() == "" || cmbgioitinh.Text.Trim() == "" || dateNgaysinh.Text.Trim() == "" ||
                cmbchuyenmon.Text.Trim() == "" || txtsdt.Text.Trim() == "" || txtemail.Text.Trim() == ""
               )
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin đăng ký.");
                }

                else
                {
                    // Thực hiện đăng ký tài khoản và nhân viên
                    SqlCommand insertNhanVienCmd = new SqlCommand("INSERT INTO Bacsi (MaBacsi, HoTen, Gioitinh, NgaySinh, SDT, Email, Chuyenmon) " +
                                                                   "VALUES (@Mabacsi, @HoTen, @GioiTinh, @Ngaysinh, @SDT, @Email, @chuyenmon)", con);

                    insertNhanVienCmd.Parameters.AddWithValue("@Mabacsi", txtMabs.Text);
                    insertNhanVienCmd.Parameters.AddWithValue("@HoTen", txthoten.Text);

                    insertNhanVienCmd.Parameters.AddWithValue("@SDT", txtsdt.Text);
                    insertNhanVienCmd.Parameters.AddWithValue("@GioiTinh", cmbgioitinh.Text);
                    insertNhanVienCmd.Parameters.AddWithValue("@Ngaysinh", dateNgaysinh.Text);
                    insertNhanVienCmd.Parameters.AddWithValue("@Email", txtemail.Text);
                    insertNhanVienCmd.Parameters.AddWithValue("@chuyenmon", cmbchuyenmon.Text);
                    insertNhanVienCmd.ExecuteNonQuery();

                    // Thêm tài khoản và mật khẩu vào bảng TaiKhoanMatKhau với TK là khóa ngoại
                    SqlCommand insertTaiKhoanCmd = new SqlCommand("INSERT INTO TaiKhoanBacSi ( MaTaiKhoan,TenDangNhap, MatKhau) " +
                                                                   "VALUES (@Matk,@TK, @MK)", con);
                    insertTaiKhoanCmd.Parameters.AddWithValue("@Matk", txtMatk.Text);
                    insertTaiKhoanCmd.Parameters.AddWithValue("@TK", txttendangnhap.Text);
                    insertTaiKhoanCmd.Parameters.AddWithValue("@MK", txtMatkhau.Text);

                    insertTaiKhoanCmd.ExecuteNonQuery();

                    MessageBox.Show("Đăng ký tài khoản và nhân viên thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}