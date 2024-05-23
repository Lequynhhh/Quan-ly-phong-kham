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
    public partial class DoiMK : Form
    {
        public DoiMK()
        {
            InitializeComponent();
        }

        private void btnDoimk_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True");

            try
            {
                con.Open();

                // Sử dụng tham số để tránh SQL Injection
                SqlCommand checkPasswordCmd = new SqlCommand("SELECT * FROM TaikhoanBacsi WHERE TenDangNhap = @TK AND MatKhau = @MK", con);
                checkPasswordCmd.Parameters.AddWithValue("@TK", txttendn.Text);
                checkPasswordCmd.Parameters.AddWithValue("@MK", txtmatkhaucu.Text);

                SqlDataAdapter sda = new SqlDataAdapter(checkPasswordCmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    if (txtmatkhaumoi.Text == txtnhaplai.Text)
                    {
                        // Sử dụng tham số trong câu lệnh SQL
                        SqlCommand updatePasswordCmd = new SqlCommand("UPDATE TaikhoanBacsi SET MatKhau = @NewPassword WHERE TenDangNhap = @TK AND MatKhau = @OldPassword", con);
                        updatePasswordCmd.Parameters.AddWithValue("@NewPassword", txtnhaplai.Text);
                        updatePasswordCmd.Parameters.AddWithValue("@TK", txttendn.Text);
                        updatePasswordCmd.Parameters.AddWithValue("@OldPassword", txtmatkhaucu.Text);

                        updatePasswordCmd.ExecuteNonQuery();
                        MessageBox.Show("Đổi mật khẩu thành công");
                    }

                    else
                    {
                        MessageBox.Show("Mật khẩu mới không trùng khớp.");
                    }
                }

                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu cũ không đúng.");
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