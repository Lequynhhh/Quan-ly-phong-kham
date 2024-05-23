using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BTcuoiky
{
    public partial class Form1 : Form
    {
        public static string Tendangnhap;
        public static string Matkhau;

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True");
            {
                con.Open();
                string UserName = txttendangnhap.Text;
                string pass = txtMatkhau.Text;
                string sql = "select *from TaiKhoanBacSi where TenDangNhap='" + UserName + "' and MatKhau='" + pass + "' ";
                SqlCommand cmd = new SqlCommand(sql,con);
                SqlDataReader dta= cmd.ExecuteReader();

                if(dta.Read() == true)
                {
                    this.Hide();
                    Trangchu trangchu = new Trangchu();
                    trangchu.FormClosed += Trangchu_FormClosed;  // Thêm trình xử lý sự kiện này
                    trangchu.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Đăng nhập không thành công");
                }

            }           

        }

        private void Trangchu_FormClosed(object sender, FormClosedEventArgs e)
        {
           this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Quenmatkhau quenMatKhauForm = new Quenmatkhau();
            quenMatKhauForm.FormClosed += new FormClosedEventHandler(quenMatKhauForm_FormClosed); // xử lý sự kiện khi form đóng
            quenMatKhauForm.Show();
            this.Hide(); // ẩn Form1
        }

        private void quenMatKhauForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show(); // hiển thị lại Form1 khi Quenmatkhau đóng
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtMatkhau.UseSystemPasswordChar = true;
            }
            else
            {
                txtMatkhau.UseSystemPasswordChar=false;
            }
        }


        // Giả sử bạn có biến `username` chứa tên đăng nhập sau khi xác thực thành công


    }
}
