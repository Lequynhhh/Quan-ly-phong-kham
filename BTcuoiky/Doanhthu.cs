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
    public partial class Doanhthu : Form
    {

        SqlConnection conect;
        SqlCommand comman;
        string str = @"Data Source=DESKTOP-C5CIFU4\SQLEXPRESS;Initial Catalog=qlpk;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlDataAdapter thangAdapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable doanhthuthang = new DataTable();
        void loaddata()
        {
            comman = conect.CreateCommand();
            comman.CommandText = @"SELECT 
                     NgayLap AS [Ngày],
                     SUM(TongTien) AS [Doanh Thu]
                     FROM HoaDon
                     GROUP BY NgayLap;";
            adapter.SelectCommand = comman;
            dt.Clear();
            adapter.Fill(dt);
            dgvdoanhthu.DataSource = dt;
   
        }
        void loaddatadoanhthuthang()
        {
            comman = conect.CreateCommand();
            comman.CommandText = @"SELECT 
  DATEPART(MONTH, NgayLap) AS [Tháng],
    DATEPART(YEAR, NgayLap) AS [Năm],
   
    SUM(TongTien) AS [Doanh Thu]
FROM HoaDon
GROUP BY  DATEPART(MONTH, NgayLap),DATEPART(YEAR, NgayLap)
ORDER BY [Tháng],[Năm]
";
            thangAdapter.SelectCommand = comman;
            doanhthuthang.Clear();
            thangAdapter.Fill(doanhthuthang); // Sử dụng thangAdapter để điền dữ liệu vào doanhthuthang
            dgvthang.DataSource = doanhthuthang;

        }
        public Doanhthu()
        {
            InitializeComponent();
           
        }

        private void Doanhthu_Load(object sender, EventArgs e)
        {
            conect = new SqlConnection(str);
            conect.Open();
            loaddata();
            loaddatadoanhthuthang();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                DateTime ngayDuocChon = datengaybatdau.Value.Date; // Giả sử bạn có một DateTimePicker tên là dateTimePickerNgay
                comman = conect.CreateCommand();
                comman.CommandText = @"SELECT 
                                 NgayLap AS [Ngày],
                                 SUM(TongTien) AS [Doanh Thu]
                               FROM HoaDon
                               WHERE NgayLap = @NgayDuocChon
                               GROUP BY NgayLap;";
                comman.Parameters.AddWithValue("@NgayDuocChon", ngayDuocChon);

                adapter.SelectCommand = comman;
                DataTable dtTheoNgay = new DataTable();
                adapter.Fill(dtTheoNgay); // Sử dụng adapter để điền dữ liệu vào dtTheoNgay
                dgvdoanhthu.DataSource = dtTheoNgay; // Giả sử bạn có một DataGridView tên là dgvdoanhthu
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
