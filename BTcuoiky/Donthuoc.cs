using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BTcuoiky
{
    public partial class Donthuoc : Form
    {
        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter donThuocAdapter = new SqlDataAdapter();
        SqlDataAdapter chiTietAdapter = new SqlDataAdapter();
        DataTable donThuocDt = new DataTable();
        DataTable chiTietDt = new DataTable();

        void loadDonThuocData()
        {
            comman = conect.CreateCommand();
            comman.CommandText = @"
                SELECT
                    dt.MaDonThuoc,
                    dt.MaPhieuKham,
                    dt.MaBacSi,
                    bs.HoTen AS HoTenBacSi,
                    dt.NgayKeDon
                FROM DonThuoc dt
                JOIN BacSi bs ON dt.MaBacSi = bs.MaBacSi;
            ";
            donThuocAdapter.SelectCommand = comman;
            donThuocDt.Clear();
            donThuocAdapter.Fill(donThuocDt);
            dgvDonThuoc.DataSource = donThuocDt;
        }

        void loadChiTietData()
        {
            comman = conect.CreateCommand();
            comman.CommandText = "SELECT * FROM ChiTietDonThuoc";
            chiTietAdapter.SelectCommand = comman;
            chiTietDt.Clear();
            chiTietAdapter.Fill(chiTietDt);
            dgvChiTiet.DataSource = chiTietDt;
        }

        public Donthuoc()
        {
            InitializeComponent();
        }

        private void dgvDonThuoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaDonThuoc.Text = dgvDonThuoc[0, dgvDonThuoc.CurrentRow.Index].Value.ToString();
            txtPK.Text = dgvDonThuoc[1, dgvDonThuoc.CurrentRow.Index].Value.ToString();
            txtBacSi.Text = dgvDonThuoc[2, dgvDonThuoc.CurrentRow.Index].Value.ToString();
            dtNgayKe.Text = dgvDonThuoc[4, dgvDonThuoc.CurrentRow.Index].Value.ToString();
        }

        private void Donthuoc_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loadDonThuocData();
            loadChiTietData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (KiemTraTonTaiMaphieukham(txtPK.Text))
            {
                comman = conect.CreateCommand();
                comman.CommandText = "INSERT INTO DonThuoc (MaDonThuoc, MaPhieuKham, MaBacSi, NgayKeDon) VALUES (@MaDonThuoc, @MaPhieuKham, @MaBacSi, @NgayKeDon)";

                // Adding parameters to avoid SQL Injection
                comman.Parameters.AddWithValue("@MaDonThuoc", txtMaDonThuoc.Text);
                comman.Parameters.AddWithValue("@MaPhieuKham", txtPK.Text);
                comman.Parameters.AddWithValue("@MaBacSi", txtBacSi.Text);
                comman.Parameters.AddWithValue("@NgayKeDon", dtNgayKe.Text);

                // Execute the query
                comman.ExecuteNonQuery();

                // Reload or refresh data
                loadDonThuocData();
            }
            else
            {
                MessageBox.Show("Mã phiếu khám không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraTonTaiMaphieukham(string mapk)
        {
            string query = "SELECT COUNT(*) FROM PhieuKham WHERE MaPhieuKham = @Mapk";

            using (SqlCommand command = new SqlCommand(query, conect))
            {
                command.Parameters.AddWithValue("@Mapk", mapk);
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "delete from DonThuoc where MaDonThuoc= ('" + txtMaDonThuoc.Text + "')";
            comman.ExecuteNonQuery();
          loadDonThuocData();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            
                string maDonThuoc = textBox1.Text;

                if (KiemTraTonTaiMaDonThuoc(maDonThuoc))
                {
                    // Mã đơn thuốc tồn tại, tiến hành thêm dữ liệu
                    comman = conect.CreateCommand();
                    comman.CommandText = "INSERT INTO ChiTietDonThuoc (MaDonThuoc, MaThuoc, SoLuong, CachDung, DonGia) VALUES (@MaDonThuoc, @MaThuoc, @SoLuong, @CachDung, @DonGia)"; ;

                // Adding parameters to avoid SQL Injection
                comman.Parameters.AddWithValue("@MaDonThuoc", textBox1.Text);
                comman.Parameters.AddWithValue("@MaThuoc",cmbThuoc.Text);
                comman.Parameters.AddWithValue("@SoLuong", Convert.ToInt32(txtSL.Text));
                comman.Parameters.AddWithValue("@CachDung", txtCD.Text);
                comman.Parameters.AddWithValue("@DonGia", Convert.ToDecimal(txtDonGia.Text));


                // Execute the query
                comman.ExecuteNonQuery();

                    // Reload or refresh data
                    loadChiTietData();
                }
                else
                {
                    MessageBox.Show("Mã đơn thuốc không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private bool KiemTraTonTaiMaDonThuoc(string maDonThuoc)
            {
                string query = "SELECT COUNT(*) FROM DonThuoc WHERE MaDonThuoc = @MaDonThuoc";

                using (SqlCommand command = new SqlCommand(query, conect))
                {
                    command.Parameters.AddWithValue("@MaDonThuoc", maDonThuoc);
                    int count = (int)command.ExecuteScalar();

                    // Nếu count > 0, tức là mã đơn thuốc tồn tại
                    return count > 0;
                }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            comman = conect.CreateCommand();
            comman.CommandText = "delete from ChiTietDonThuoc where MaDonThuoc= ('" + textBox1.Text + "')";
            comman.ExecuteNonQuery();
            loadChiTietData();
        }
    }
    }
