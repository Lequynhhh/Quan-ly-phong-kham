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
using System.Runtime.Remoting.Contexts;

namespace BTcuoiky
{
    public partial class Thuoc : Form
    {
        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = "select *from Thuoc";
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
            dgvThuoc.DataSource = dt;

        }
        public Thuoc()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Assuming 'conect' is your database connection object and it's already opened.
            comman = conect.CreateCommand();
            comman.CommandText = "INSERT INTO Thuoc (MaThuoc, TenThuoc, NSX, HSD, DonViTinh, DonGia) VALUES (@MaThuoc, @TenThuoc, @NSX, @HSD, @DonViTinh, @DonGia)";


            // Replace 'Column1', 'Column2', etc., with the actual column names of your 'Thuoc' table.

            // Adding parameters to avoid SQL Injection
            comman.Parameters.AddWithValue("@MaThuoc", txtMaThuoc.Text);
            comman.Parameters.AddWithValue("@TenThuoc", txtTenThuoc.Text);
            comman.Parameters.AddWithValue("@NSX", dtNSX.Text);
            comman.Parameters.AddWithValue("@HSD", dtHSD.Text);
            comman.Parameters.AddWithValue("@DonViTinh", cmbDonvi.Text);
            comman.Parameters.AddWithValue("@DonGia", txtDonGia.Text);

            // Execute the query
            comman.ExecuteNonQuery();

            // Reload or refresh data
            loaddata();
        }


        private void Thuoc_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

        private void dgvThuoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaThuoc.Text = dgvThuoc[0,dgvThuoc.CurrentRow.Index].Value.ToString();
            txtTenThuoc.Text = dgvThuoc[1, dgvThuoc.CurrentRow.Index].Value.ToString();
            dtNSX.Text = dgvThuoc[2, dgvThuoc.CurrentRow.Index].Value.ToString();
            dtHSD.Text = dgvThuoc[3, dgvThuoc.CurrentRow.Index].Value.ToString();
           cmbDonvi.Text = dgvThuoc[4, dgvThuoc.CurrentRow.Index].Value.ToString();
           txtDonGia.Text = dgvThuoc[5, dgvThuoc.CurrentRow.Index].Value.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

                comman = conect.CreateCommand();
                comman.CommandText = "UPDATE Thuoc SET TenThuoc = @TenThuoc, NSX=@NSX, HSD=@HSD, DonViTinh = @DonViTinh, DonGia=@DonGia WHERE MaThuoc= @MaThuoc";

                comman.Parameters.AddWithValue("@TenThuoc", txtTenThuoc.Text);
                comman.Parameters.AddWithValue("@NSX", dtNSX.Text);
                comman.Parameters.AddWithValue("@HSD", dtHSD.Text);
                comman.Parameters.AddWithValue("@DonViTinh", cmbDonvi.Text);
                comman.Parameters.AddWithValue("@DonGia", txtDonGia.Text);
                comman.Parameters.AddWithValue("@MaThuoc", txtMaThuoc.Text);
            comman.ExecuteNonQuery();

            loaddata();
            }

        

        private void btncheck_Click(object sender, EventArgs e)
        {


            string searchMaThuoc = txtMaThuoc.Text.Trim(); // Lấy giá trị Mã thuốc từ trường nhập liệu

            if (!string.IsNullOrEmpty(searchMaThuoc))
            {
                // Tạo câu lệnh SQL để tìm kiếm dựa vào Mã thuốc
                string sqlQuery = "SELECT * FROM Thuoc WHERE MaThuoc LIKE @MaThuoc";

                // Tạo và thiết lập đối tượng SqlCommand
                comman = conect.CreateCommand();
                comman.CommandText = sqlQuery;
                comman.Parameters.AddWithValue("@MaThuoc", "%" + searchMaThuoc + "%"); // Sử dụng tham số để tránh SQL Injection

                // Thực hiện truy vấn và cập nhật DataGridView
                adapter.SelectCommand = comman;
                dt.Clear();
                adapter.Fill(dt);
                dgvThuoc.DataSource = dt;

                // Kiểm tra xem có dòng dữ liệu nào được tìm thấy hay không
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy Mã thuốc nào phù hợp với tìm kiếm của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập Mã thuốc cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            
                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Kiểm tra kết quả của hộp thoại xác nhận
                if (result == DialogResult.Yes)
                {
                    // Nếu người dùng chọn "Yes", thực hiện xóa dữ liệu
                    comman = conect.CreateCommand();
                    comman.CommandText = "delete from Thuoc where MaThuoc= ('" + txtMaThuoc.Text + "')";
                    comman.ExecuteNonQuery();
                    loaddata();
                }
                else
                {
                    // Nếu người dùng chọn "No" hoặc đóng hộp thoại xác nhận, không thực hiện xóa
                    // Có thể thêm mã lệnh hoặc thông báo khác nếu cần
                }
            }
        }
    }
    
