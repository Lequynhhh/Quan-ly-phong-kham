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
    public partial class Nhanvien : Form
    {
        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = "select *from BacSi";
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
         dgvListBS.DataSource = dt;

        }
        public Nhanvien()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvListBS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMabs.Text = dgvListBS[0, dgvListBS.CurrentRow.Index].Value.ToString();
            txtHoten.Text = dgvListBS[1,dgvListBS.CurrentRow.Index].Value.ToString();
            cmbGioiTinh.Text = dgvListBS[2,dgvListBS.CurrentRow.Index].Value.ToString();
            dateNgaysinh.Text = dgvListBS[3, dgvListBS.CurrentRow.Index].Value.ToString();
            txtSdt.Text = dgvListBS[4,dgvListBS.CurrentRow.Index].Value.ToString();
            txtEmail.Text = dgvListBS[5, dgvListBS.CurrentRow.Index].Value.ToString();
            cmbChuyenMon.Text = dgvListBS[6, dgvListBS.CurrentRow.Index].Value.ToString();
        }

       

        private void Nhanvien_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "INSERT INTO BacSi (MaBacSi, HoTen,GioiTinh, NgaySinh, SDT, Email,ChuyenMon) VALUES (@MaBacSi, @HoTen,@GioiTinh, @NgaySinh, @SDT, @Email,@ChuyenMon)";


            // Replace 'Column1', 'Column2', etc., with the actual column names of your 'Thuoc' table.

            // Adding parameters to avoid SQL Injection
            comman.Parameters.AddWithValue("@MaBacSi", txtMabs.Text);
            comman.Parameters.AddWithValue("@HoTen", txtHoten.Text);
            comman.Parameters.AddWithValue("@GioiTinh", cmbGioiTinh.Text);
            comman.Parameters.AddWithValue("@NgaySinh", dateNgaysinh.Text);
            comman.Parameters.AddWithValue("@SDT", txtSdt.Text);
            comman.Parameters.AddWithValue("@Email",txtEmail.Text);
            comman.Parameters.AddWithValue("@ChuyenMon", cmbChuyenMon.Text);

            // Execute the query
            comman.ExecuteNonQuery();

            // Reload or refresh data
            loaddata();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "SELECT * FROM BacSi WHERE MaBacSi LIKE @Mabs";

            // Add parameter to prevent SQL injection
            comman.Parameters.AddWithValue("@Mabs", "%" + txtMabs.Text + "%");

            // Execute the command and load data into the grid view
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
           dgvListBS.DataSource = dt;
        }
    }
}
