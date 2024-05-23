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
    public partial class Quenmatkhau : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True");

        public Quenmatkhau()
        {
            InitializeComponent();
        }



        private void btnLaylaimatkhau_Click(object sender, EventArgs e)
        {
            string mataikhoan = textBox1.Text;
            string query = "SELECT MatKhau FROM TaiKhoanBacSi WHERE MaTaiKhoan = @MaTaiKhoan";

          
            
                con.Open();

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@MaTaiKhoan", mataikhoan);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string password = reader["MatKhau"].ToString();
                    textBox2.Text = password;
                }
                    else
                    {
                        MessageBox.Show("Email không tồn tại");
                    }
                }
            
        }
        public event EventHandler Back;
      

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); // đóng form Quenmatkhau
        }
    }
}