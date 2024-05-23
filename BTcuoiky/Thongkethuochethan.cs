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
    public partial class Thongkethuochethan : Form
    {

        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = "SELECT MaThuoc, TenThuoc, HSD, " +
                              "CASE " +
                                  "WHEN HSD < GETDATE() THEN N'Hết hạn' " +
                                  "WHEN HSD <= DATEADD(day, 30, GETDATE()) THEN N'Sắp hết hạn' " +
                                  "ELSE N'Còn hiệu lực' " +
                              "END AS TrangThai " +
                              "FROM Thuoc;";
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
            dggthuochethan.DataSource = dt;
        }
            public Thongkethuochethan()
        {
            InitializeComponent();
        }

        private void Thongkethuochethan_Load(object sender, EventArgs e)
        {

            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
        }

      

      

      

       

        private void txtMaThuoc_TextChanged(object sender, EventArgs e)
        {

        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            string maThuoc = txtMaThuoc.Text.Trim(); // The TextBox for MaThuoc input
            string selectedStatus = cmbtrangthai.SelectedItem?.ToString(); // The ComboBox for TrangThai input

            // Starting the query
            string query = @"
        SELECT
            MaThuoc,
            TenThuoc,
            HSD,
            CASE
                WHEN HSD < GETDATE() THEN N'Hết hạn'
                WHEN HSD <= DATEADD(day, 30, GETDATE()) THEN N'Sắp hết hạn'
                ELSE N'Còn hiệu lực'
            END AS TrangThai
        FROM Thuoc
        WHERE 1 = 1"; // Dummy condition to simplify appending further conditions

            // Building the query based on inputs
            if (!string.IsNullOrEmpty(maThuoc))
            {
                query += " AND MaThuoc = @MaThuoc";
            }
            if (!string.IsNullOrEmpty(selectedStatus))
            {
                switch (selectedStatus)
                {
                    case "Hết hạn":
                        query += " AND HSD < GETDATE()";
                        break;
                    case "Sắp hết hạn":
                        query += " AND HSD <= DATEADD(day, 30, GETDATE()) AND HSD >= GETDATE()";
                        break;
                    case "Còn hiệu lực":
                        query += " AND HSD > DATEADD(day, 30, GETDATE())";
                        break;
                }
            }

            // If neither MaThuoc nor TrangThai is provided, show a message and return.
            if (string.IsNullOrEmpty(maThuoc) && string.IsNullOrEmpty(selectedStatus))
            {
                MessageBox.Show("Please enter a MaThuoc or select a TrangThai to search.");
                return;
            }

            string connectionString = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine(@"
        SELECT
            MaThuoc,
            TenThuoc,
            HSD,
            CASE
                WHEN HSD < GETDATE() THEN N'Hết hạn'
                WHEN HSD <= DATEADD(day, 30, GETDATE()) THEN N'Sắp hết hạn'
                ELSE N'Còn hiệu lực'
            END AS TrangThai
        FROM Thuoc
        WHERE 1=1");

            if (!string.IsNullOrEmpty(maThuoc))
            {
                queryBuilder.AppendLine("AND MaThuoc = @MaThuoc");
            }
            if (!string.IsNullOrEmpty(selectedStatus))
            {
                queryBuilder.AppendLine("AND (CASE WHEN HSD < GETDATE() THEN N'Hết hạn' WHEN HSD <= DATEADD(day, 30, GETDATE()) THEN N'Sắp hết hạn' ELSE N'Còn hiệu lực' END) = @TrangThai");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection);
                if (!string.IsNullOrEmpty(maThuoc))
                {
                    command.Parameters.AddWithValue("@MaThuoc", maThuoc);
                }
                if (!string.IsNullOrEmpty(selectedStatus))
                {
                    command.Parameters.AddWithValue("@TrangThai", selectedStatus);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                try
                {
                    adapter.Fill(dt);

                    // Assuming dgvHD is the DataGridView where you want to display the results
                    dggthuochethan.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while searching: " + ex.Message);
                }
            }  
        }
    }
}
